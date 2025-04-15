using UnityEngine;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;

public class CortePlatos : MonoBehaviour
{
    [Header("Depuracion")]
    public bool test;

    [Header("Configuración del área de dibujo (definir desde el Inspector)")]
    public Rect drawArea;
    [Space]
    public float x;
    public float y;
    public float w;
    public float h;

    [Header("Activar/Desactivar detección")]
    public bool Detectar = true;

    [Header("Número mínimo de puntos para detectar un gesto")]
    [SerializeField] private int minPuntosParaDetectar = 10;

    [Header("Umbral mínimo para reconocer el gesto (puntuación)")]
    [SerializeField] private float minScoreThreshold = 0.8f;

    [Header("Longitud mínima del trazo para evaluar el gesto")]
    [SerializeField] private float minLineLength = 100f;

    [Header("Visualización del Área de Dibujo")]    
    public bool showDrawArea = true;                   // Permite activar/desactivar la visualización    
    public KeyCode toggleDrawAreaKey;        // Tecla para alternar la visualización

    // Lista para almacenar los puntos trazados por el usuario
    private List<Point> points = new List<Point>();
    private bool isDrawing = false;

    // Conjunto de gestos de entrenamiento cargados desde XML
    private List<Gesture> trainingSet = new List<Gesture>();

    [SerializeField] private string audioEvent;
    [SerializeField] private Controlador_Instancias controladorInstancias;

    private void Start()
    {
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();
                
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        // Opcional: cargar gestos personalizados almacenados en la carpeta persistente
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    void Update()
    {
        drawArea = new Rect(x, y, w, h);

        if (Input.GetKeyDown(toggleDrawAreaKey))
        {
            ActivarAreaCorte();
        }

        // Para dispositivos móviles (Android, iOS)
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos2 = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    if (drawArea.Contains(pos2))
                        StartDrawing();
                }
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (isDrawing && drawArea.Contains(pos2))
                        points.Add(new Point(pos2.x, -pos2.y, 0));
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    EndDrawing();
                }
            }
        }

        Vector3 pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (drawArea.Contains(pos))
                StartDrawing();
        }
        else if (Input.GetMouseButton(0))
        {
            if (isDrawing && drawArea.Contains(pos))
                points.Add(new Point(pos.x, -pos.y, 0));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }

    }

    private void StartDrawing()
    {
        isDrawing = true;
        points.Clear();
    }

    private void EndDrawing()
    {
        if (!isDrawing)
            return;
        isDrawing = false;
                
        if (Detectar && points.Count >= minPuntosParaDetectar && controladorInstancias.areaActivada)
        {        
            Vector2 inicio = new Vector2(points[0].X, points[0].Y);
            Vector2 fin = new Vector2(points[points.Count - 1].X, points[points.Count - 1].Y);
            float longitudTramo = Vector2.Distance(inicio, fin);

            if (longitudTramo >= minLineLength && controladorInstancias.areaActivada)
            {         
                Gesture candidate = new Gesture(points.ToArray());             
                Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                Debug.Log("Gesto: " + gestureResult.GestureClass + " | Puntuación: " + gestureResult.Score);
                                
                if ((gestureResult.GestureClass == "line" ||
                    gestureResult.GestureClass == "I" ||
                    gestureResult.GestureClass == "lineD1" ||
                    gestureResult.GestureClass == "lineD2" ||
                    gestureResult.GestureClass == "lineD3" ||
                    gestureResult.GestureClass == "lineD4" ||
                    gestureResult.GestureClass == "lineH1" ||
                    gestureResult.GestureClass == "lineV1" ||
                    gestureResult.GestureClass == "lineV2") &&
                    gestureResult.Score >= minScoreThreshold)
                {
                    CortePlatosAccion();
                }
                else
                {
                    Debug.Log("Gesto no reconocido o la puntuación es demasiado baja.");                    
                }
            }
            else
            {
                Debug.Log("La longitud del trazo es insuficiente para evaluar el gesto.");                
            }
        }
    }
       
    void OnGUI()
    {
        if (showDrawArea)
        {
            // Deshabilita la interacción para que el área solo se pinte y no bloquee el click
            GUI.enabled = false;
            GUI.Box(drawArea, "AreaCorte");
            GUI.enabled = true;
        }
    }

    public void ActivarAreaCorte()
    {
        showDrawArea = !showDrawArea;
    }

    public void CortePlatosAccion()
    {
        Debug.Log("¡Gesto reconocido! Ejecutando acción de corte.");
        controladorInstancias.DestruirPlatos();
        AudioImp.Instance.Reproducir(audioEvent);
    }
}
