using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LimpiarBasura : MonoBehaviour
{
    [Header("Depuración")]
    public bool mostrarDebug = true;

    public float sostenerLimpiarTimer;
    public float basuraTimer;
    public float basuraTimer_i;
    public float contadorLimpiar = 5;
    public bool estaPresionando;
    public bool flagReducir;
    public bool flagAumentar;

    [Header("De Sonido")]
    public string eventoPlay;
    public string eventoStop;
    public bool reproduciendoEvento;

    [Header("Referencias")]
    private InstanciarBasura instanciarBasura;
    public GameManager gameManager;
    public Controlador_Fases controladorFases;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable(); // Añadir en tu script inicial
    }
    private void OnDisable() => EnhancedTouchSupport.Disable();    

    private void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        instanciarBasura = GameObject.Find("ColliderBasura").GetComponent<InstanciarBasura>();
        basuraTimer = basuraTimer_i;
        if (mostrarDebug) Debug.Log($"[LimpiarBasura] Inicio con contador: {contadorLimpiar}");
    }

    private void Update()
    {
        // Auto-aumentar contador cuando no se esté presionando
        if (basuraTimer > 0 && contadorLimpiar < 4 && !estaPresionando)
        {
            basuraTimer -= Time.deltaTime;
            if (basuraTimer <= 0 && !flagAumentar)
            {
                AumentarContadorLimpiar();
                flagAumentar = true;
            }
        }
        else
        {
            basuraTimer = basuraTimer_i;
            flagAumentar = false;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null)
        {
            var mouse = Mouse.current;
            // Al pulsar Mouse Down, raycast para comprobar colisión con este objeto
            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (controladorFases.enPausa) return;
                Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    estaPresionando = true;
                    if (!reproduciendoEvento)
                    {
                        AudioImp.Instance.Reproducir(eventoPlay);
                        reproduciendoEvento = true;
                    }
                }
            }
            // Mientras mantengo, acumulo tiempo
            else if (mouse.leftButton.isPressed && estaPresionando)
            {
                sostenerLimpiarTimer += Time.deltaTime;
                basuraTimer = basuraTimer_i;
            }
            // Al soltar, reset
            else if (mouse.leftButton.wasReleasedThisFrame && estaPresionando)
            {
                sostenerLimpiarTimer = 0;
                estaPresionando = false;
                flagReducir = false;
                reproduciendoEvento = false;
                AudioImp.Instance.Reproducir(eventoStop);
            }
        }
#elif UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (controladorFases.enPausa) return;
        foreach (var touch in Touch.activeTouches)
        {
            // Al iniciar el toque, raycast en la pantalla
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    estaPresionando    = true;
                    if (!reproduciendoEvento)
                    {
                        AudioImp.Instance.Reproducir(eventoPlay);
                        reproduciendoEvento = true;
                    }
                }
            }
            // Mientras mantengo el dedo, acumulo tiempo
            else if ((touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                      touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                     && estaPresionando)
            {
                sostenerLimpiarTimer += Time.deltaTime;
                basuraTimer          = basuraTimer_i;
                if (sostenerLimpiarTimer >= gameManager.tiempoLimpiarBasura
                    && contadorLimpiar > 0)
                {
                    ReiniciarBasura();
                }
            }
            // Al levantar el dedo, reset
            else if ((touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                      touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                     && estaPresionando)
            {
                sostenerLimpiarTimer = 0;
                estaPresionando      = false;
                flagReducir          = false;
                reproduciendoEvento  = false;
                AudioImp.Instance.Reproducir(eventoStop);
            }
        }
#endif

        // Si supero el tiempo de limpieza, reinicio basura
        if (estaPresionando &&
            sostenerLimpiarTimer >= gameManager.tiempoLimpiarBasura &&
            contadorLimpiar > 0)
        {
            ReiniciarBasura();
        }
    }

    public void ReiniciarBasura()
    {
        instanciarBasura.BorrarBasura();
        if (!flagReducir)
        {
            ReducirContadorLimpiar();
            flagReducir = true;
        }
        if (mostrarDebug) Debug.Log("[LimpiarBasura] Basura limpiada");
    }

    public void ReducirContadorLimpiar() => contadorLimpiar--;
    public void AumentarContadorLimpiar() => contadorLimpiar++;
}
