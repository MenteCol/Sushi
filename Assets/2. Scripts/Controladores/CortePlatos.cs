using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using PDollarGestureRecognizer;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CortePlatos : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region Variables
    [Header("Depuracion")]
    public bool mostrarLogs;
    public bool test;

    [Header("Configuración del área de dibujo (definir desde el Inspector)")]
    public Rect drawArea;
    public float x, y, w, h;

    [Header("Activar/Desactivar detección")]
    public bool Detectar = true;

    [Header("Número mínimo de puntos para detectar un gesto")]
    [SerializeField] private int minPuntosParaDetectar = 5;
    [SerializeField] private float samplingDistance = 10f;

    [Header("Umbral mínimo para reconocer el gesto (puntuación)")]
    [SerializeField] private float minScoreThreshold = 0.8f;

    [Header("Longitud mínima del trazo para evaluar el gesto")]
    [SerializeField] private float minLineLength = 50f;

    [Header("Visualización del Área de Dibujo")]
    public bool showDrawArea = true;
    private bool isDrawing = false;
    public List<Point> points = new List<Point>();
    private List<Gesture> trainingSet = new List<Gesture>();

    [Header("Eventos FMOD")]
    [SerializeField] private string eventoCorte;
    [SerializeField] private string eventoPlatosRotos;

    [Header("Varios")]
    [SerializeField] private RectTransform lineaUI;
    public bool primeraVez = false;
    public GameObject tutoCorte;

    [Header("Proporción de escala de la línea UI")]
    [SerializeField, Range(0.1f, 5f)] private float proporcionEscalaLinea = 1.0f;

    [Header("Referencias")]
    [SerializeField] private Controlador_Instancias controladorInstancias;
    public GameManager gameManager;
    #endregion

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();

        #region Carga de Gestos

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet");
        foreach (var gestureXml in gesturesXml)
        {
            if (mostrarLogs)
                Debug.Log($"{gameObject.name}: [PDollar] Loaded gesture XML: {gestureXml.name}");
            Gesture gesture = GestureIO.ReadGestureFromXML(gestureXml.text);
            trainingSet.Add(gesture);
            if (mostrarLogs)
                Debug.Log($"{gameObject.name}: [PDollar] Parsed gesture template: {gesture.Name}");
        }

        foreach (var filePath in Directory.GetFiles(Application.persistentDataPath, "*.xml"))
        {
            Gesture gesture = GestureIO.ReadGestureFromFile(filePath);
            trainingSet.Add(gesture);
            if (mostrarLogs)
                Debug.Log($"{gameObject.name}: [PDollar] Loaded user gesture from: {Path.GetFileName(filePath)} (Name: {gesture.Name})");
        }

        if (mostrarLogs)
            Debug.Log($"{gameObject.name}: [PDollar] Total gestures loaded: {trainingSet.Count}");

        foreach (var g in trainingSet)
        {
            if (mostrarLogs)
                Debug.Log($"{gameObject.name}: [PDollar] Gesture in training set: {g.Name}");
        }

        #endregion

        OcultarLineaUI();
    }

    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();

    private void Update()
    {
        drawArea = new Rect(x, y, w, h);

        if (!primeraVez && controladorInstancias.areaActivada)
        { 
            tutoCorte.SetActive(true);
            primeraVez = true;
        }

#if UNITY_ANDROID || UNITY_IOS
        foreach (var touch in Touch.activeTouches)
        {
            Vector2 pos = touch.screenPosition;
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began && drawArea.Contains(pos) && controladorInstancias.areaActivada)
            {
                StartDrawing();
                AddPoint(pos);
            }
            else if ((touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                      touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary) && isDrawing && drawArea.Contains(pos))
            {
                AddIntermediatePoints(pos);
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                EndDrawing();
            }
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            if (Mouse.current.leftButton.wasPressedThisFrame && drawArea.Contains(pos) && controladorInstancias.areaActivada)
            {
                StartDrawing();
                AddPoint(pos);
            }
            else if (Mouse.current.leftButton.isPressed && isDrawing && drawArea.Contains(pos))
            {
                AddIntermediatePoints(pos);
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                EndDrawing();
            }
        }
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        if (drawArea.Contains(pos) && controladorInstancias.areaActivada)
        {
            StartDrawing();
            AddPoint(pos);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        if (isDrawing && drawArea.Contains(pos))
        {
            AddIntermediatePoints(pos);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        EndDrawing();
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
                if (mostrarLogs)
                    Debug.Log($"{gameObject.name}: Gesto: {gestureResult.GestureClass} | Puntuación: {gestureResult.Score}");

                if ((gestureResult.GestureClass == "line" ||
                    gestureResult.GestureClass == "line2" ||
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
                    if (mostrarLogs)
                        Debug.Log($"{gameObject.name}: Gesto no reconocido o la puntuación es demasiado baja.");
                }
            }
            else
            {
                if (mostrarLogs)
                    Debug.Log($"{gameObject.name}: La longitud del trazo es insuficiente para evaluar el gesto.");
            }
        }
    }
    private void AddPoint(Vector2 pos)
    {
        points.Add(new Point(pos.x, -pos.y, 0));
    }
    private void AddIntermediatePoints(Vector2 pos)
    {
        Vector2 prev = new Vector2(points[^1].X, points[^1].Y);
        float dist = Vector2.Distance(prev, pos);
        if (dist > samplingDistance)
        {
            int steps = Mathf.FloorToInt(dist / samplingDistance);
            for (int i = 1; i < steps; i++)
            {
                Vector2 inter = Vector2.Lerp(prev, pos, (float)i / steps);
                points.Add(new Point(inter.x, -inter.y, 0));
            }
        }
        AddPoint(pos);
    }

    public void ActivarAreaCorte() => showDrawArea = !showDrawArea;

    public void CortePlatosAccion()
    {
        if (tutoCorte.activeSelf) tutoCorte.SetActive(false);
        MostrarLineaUI();
        controladorInstancias.DestruirPlatos();
        AudioImp.Instance.Reproducir(eventoCorte);
        AudioImp.Instance.Reproducir(eventoPlatosRotos);
        Invoke(nameof(OcultarLineaUI), 0.5f);
    }

    private void MostrarLineaUI()
    {
        if (lineaUI == null || points.Count < 2) return;

        Vector2 primerPunto = new Vector2(points[0].X, -points[0].Y);
        Vector2 ultimoPunto = new Vector2(points[^1].X, -points[^1].Y);
        Vector2 direccion = ultimoPunto - primerPunto;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        float longitud = direccion.magnitude * proporcionEscalaLinea;

        Vector2 uiPos;
        RectTransform parentRect = lineaUI.parent as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            primerPunto,
            null,
            out uiPos
        );
        lineaUI.anchoredPosition = uiPos;
        lineaUI.rotation = Quaternion.Euler(0, 0, angulo);
        lineaUI.sizeDelta = new Vector2(longitud, lineaUI.sizeDelta.y);
        lineaUI.gameObject.SetActive(true);
    }

    private void OcultarLineaUI()
    {
        if (lineaUI != null)
            lineaUI.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(new Vector3(point.X, -point.Y, 0), 5f);
        }
    }
#endif
}