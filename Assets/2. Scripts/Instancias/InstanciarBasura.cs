using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstanciarBasura : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Variables")]
    public bool basuraLlena = false;
    public bool sonidoAlerta;
    public int contadorBasura = 0;
    public float contStrikeNoComer;
    public float timerStrikeBasura_i;
    public float timerStrikeBasura;
    public string eventoTirarBasura;
    public GameObject imagenBoton;

    [Header("Referencias")]
    public GameManager gameManager;
    public Controlador_Instancias controladorInstancias;
    public Controlador_Fases controladorFases;
    public LimpiarBasura limpiarBasura;

    [Header("Listas")]
    public List<Transform> puntoBasura = new List<Transform>();
    public List<GameObject> objetosBasura = new List<GameObject>();

    [Header("Disparo")]
    public Transform puntoDeDisparo;                 // Punto desde donde se dispara
    public Vector3 direccionDisparo = new Vector3(0, 1, 1); // Dirección configurable desde inspector
    public float fuerzaMin = 5f;                      // Fuerza mínima para disparo
    public float fuerzaMax = 15f;                     // Fuerza máxima para disparo

    [Header("Simulación de Trayectoria")]
    public LayerMask capaColision;                    // Capas para detectar colisión en la simulación
    public float tiempoSimulacion = 5f;               // Tiempo máximo a simular
    public float intervaloSimulacion = 0.1f;          // Intervalo de simulación (paso)

    [Header("Testeo")]
    public InputActionReference accionDisparar;
    public GameObject prefabTestDisparo;               // Prefab para disparar en test y simular trayectoria
    #endregion

    private Vector3 puntoImpacto;                      // Punto donde se detecta impacto
    private bool impactoDetectado = false;

    void OnEnable()
    {
        if (accionDisparar != null)
            accionDisparar.action.performed += OnDispararPerformed;

        accionDisparar?.action.Enable();
    }

    void OnDisable()
    {
        if (accionDisparar != null)
            accionDisparar.action.performed -= OnDispararPerformed;

        accionDisparar?.action.Disable();
    }


    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();

        contStrikeNoComer = 0;
        timerStrikeBasura_i = controladorFases.timerNoComer;
        timerStrikeBasura = timerStrikeBasura_i;
    }

    void Update()
    {
        // Lógica original
        if (contadorBasura >= gameManager.valorLimiteLlena)
        {
            basuraLlena = true;
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            basuraLlena = false;
            this.GetComponent<BoxCollider>().isTrigger = true;
        }

        if (contadorBasura >= gameManager.valorLimiteLlena - 2)
        {
            Controlador_EmotesT.Instance.ReproducirEmoji("Bravo");
            imagenBoton.SetActive(true);

            if (!sonidoAlerta)
            {
                AudioImp.Instance.Reproducir("gatoStrikes");
                sonidoAlerta = true;
            }
        }
        else
        {
            sonidoAlerta = false;
            imagenBoton.SetActive(false);
        }

        if (limpiarBasura.estaPresionando)
        {
            if (depuracion)
                Debug.Log($"{gameObject.name}: Ocultar Boton al presionar la basura");
            imagenBoton.SetActive(false);
        }

        if (contStrikeNoComer > 0)
        {
            timerStrikeBasura -= Time.deltaTime;

            if (timerStrikeBasura <= 0)
            {
                contStrikeNoComer = 0;
            }
        }
    }

    private void OnDispararPerformed(InputAction.CallbackContext context)
    {
        if (depuracion)
            Debug.Log("Acción Disparar recibida por Input System");

        if (prefabTestDisparo != null && puntoDeDisparo != null)
        {
            DispararBasura(prefabTestDisparo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Comida"))
        {
            int indiceAleatorio = Random.Range(0, puntoBasura.Count);
            contadorBasura++;

            contStrikeNoComer++;
            timerStrikeBasura_i = controladorFases.timerNoComer;
            timerStrikeBasura = timerStrikeBasura_i;

            AudioImp.Instance.Reproducir(eventoTirarBasura);

            // Reducir escala del objeto original
            other.transform.localScale = other.transform.localScale * 1f;
            other.GetComponent<BoxCollider>().size = Vector3.one * 0.6f;

            // Instanciar en disparo            
            //objetosBasura.Add(other.gameObject);

            DispararBasura(other.gameObject);

            // Mover el objeto original a un punto de basura (mantener lógica actual)
            // other.transform.position = puntoBasura[indiceAleatorio].position;
            Destroy(other.gameObject);


        }
    }

    /// <summary>
    /// Instancia y dispara el objeto basura desde puntoDeDisparo con fuerza aleatoria entre fuerzaMin y fuerzaMax
    /// </summary>
    /// <param name="objeto">El objeto a instanciar y disparar</param>
    private void DispararBasura(GameObject objeto)
    {
        if (puntoDeDisparo != null)
        {
            GameObject basuraInstanciada = Instantiate(objeto, puntoDeDisparo.position, Quaternion.identity);
            objetosBasura.Add(basuraInstanciada);

            Rigidbody rb = basuraInstanciada.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = basuraInstanciada.AddComponent<Rigidbody>();
            }

            // Fuerza lineal aleatoria en la dirección configurada
            float fuerzaAleatoria = Random.Range(fuerzaMin, fuerzaMax);
            Vector3 fuerza = direccionDisparo.normalized * fuerzaAleatoria;
            rb.AddForce(fuerza, ForceMode.Impulse);

            // --- NUEVO: Aplicar torque aleatorio para rotación ---
            // Define un rango de torque para cada eje (puedes ajustar estos valores)
            float torqueX = Random.Range(-1f, 1f);
            float torqueY = Random.Range(-1f, 1f);
            float torqueZ = Random.Range(-1f, 1f);
            Vector3 torque = new Vector3(torqueX, torqueY, torqueZ);

            rb.AddTorque(torque, ForceMode.Impulse);

            objetosBasura.Add(basuraInstanciada);
        }
        else
        {
            if (depuracion)
                Debug.LogWarning("puntoDeDisparo no está asignado en InstanciarBasura");
        }
    }


    public void BorrarBasura()
    {
        foreach (GameObject obj in objetosBasura)
        {
            Destroy(obj);
        }

        objetosBasura.RemoveAll(item => item == null);
        contadorBasura = 0;
        contStrikeNoComer = 0;
    }

    // Dibuja la trayectoria simulada y el punto de impacto con un gizmo
    private void OnDrawGizmos()
    {
        if (puntoDeDisparo == null || prefabTestDisparo == null)
            return;

        Vector3 posicion = puntoDeDisparo.position;

        // Para visualización usamos la fuerza media
        float fuerzaAleatoria = (fuerzaMin + fuerzaMax) / 2f;
        Vector3 velocidadInicial = direccionDisparo.normalized * fuerzaAleatoria;
        Vector3 gravedad = Physics.gravity;

        Vector3 posicionAnterior = posicion;
        impactoDetectado = false;

        for (float t = 0; t < tiempoSimulacion; t += intervaloSimulacion)
        {
            Vector3 nuevaPosicion = posicion + velocidadInicial * t + 0.5f * gravedad * t * t;

            // Raycast para detectar colisión en el camino
            if (Physics.Raycast(posicionAnterior, nuevaPosicion - posicionAnterior, out RaycastHit hit, (nuevaPosicion - posicionAnterior).magnitude, capaColision))
            {
                puntoImpacto = hit.point;
                impactoDetectado = true;
                break;
            }

            posicionAnterior = nuevaPosicion;
        }

        // Dibujar trayectoria
        Gizmos.color = Color.yellow;
        posicionAnterior = puntoDeDisparo.position;
        for (float t = 0; t < tiempoSimulacion; t += intervaloSimulacion)
        {
            Vector3 nuevaPosicion = puntoDeDisparo.position + velocidadInicial * t + 0.5f * gravedad * t * t;
            Gizmos.DrawLine(posicionAnterior, nuevaPosicion);
            posicionAnterior = nuevaPosicion;

            if (impactoDetectado && Vector3.Distance(nuevaPosicion, puntoImpacto) < 0.1f)
                break;
        }

        // Dibujar esfera roja en el punto de impacto si se detectó
        if (impactoDetectado)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(puntoImpacto, 0.1f);
        }
    }
}
