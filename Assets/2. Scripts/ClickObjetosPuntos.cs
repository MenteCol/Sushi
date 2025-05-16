using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ClickObjetosPuntos : MonoBehaviour
{
    [Header("Identificador Asignado")]
    [SerializeField] public int ID;

    [Header("Valor Instancia")]
    public float puntosLlenuraDif;

    [Header("Valores Generales")]
    [SerializeField] public int puntosComida;
    [SerializeField] public float puntosLlenura;
    [SerializeField] public int puntosMalestar;

    [Header("Audio")]
    [SerializeField] private string audioComer;
    [SerializeField] private string audioNoPuede;

    [Header("Referencias")]
    public Controlador_Fases controladorFases;
    [SerializeField] private Controlador_Puntos controladorPuntos;
    [SerializeField] private InstanciarBasura instanciarBasura;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        controladorPuntos = GameObject.Find("Controlador_Puntaje").GetComponent<Controlador_Puntos>();
        instanciarBasura = GameObject.Find("ColliderBasura").GetComponent<InstanciarBasura>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        EventSystem.current.pixelDragThreshold = 0;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (controladorFases.enPausa)
            return;

#if UNITY_ANDROID
        // Lógica de toque móvil
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                ProcessTouch(touch.screenPosition);
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        // Lógica de clic en PC/Editor
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            ProcessTouch(mouse.position.ReadValue());
        }
#endif
    }

    private void ProcessTouch(Vector2 screenPos)
    {
        // Evitar interacción sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            HandleClick();
    }

    private void HandleClick()
    {
        if (!controladorPuntos.estaEnfermo && !controladorPuntos.estaLleno && !instanciarBasura.basuraLlena)
        {
            switch (ID)
            {
                case 1:
                    puntosComida = 1;
                    puntosLlenura = 0.1f;
                    break;
                case 2:
                    puntosComida = 1;
                    puntosLlenura = 0.2f;
                    puntosMalestar = 1;
                    break;
                case 3:
                    puntosComida = 1;
                    puntosLlenura = 0.05f;
                    gameManager.ReducirVelocidad();
                    break;
                default:
                    return;
            }

            controladorPuntos.SumarPuntos(
                puntosComida,
                puntosLlenura * puntosLlenuraDif,
                ID,
                puntosMalestar);

            AudioImp.Instance.Reproducir(audioComer);
            Destroy(gameObject);
        }
        else
        {
            AudioImp.Instance.Reproducir(audioNoPuede);
        }

        if (controladorPuntos.estaEnfermo)
            AudioImp.Instance.Reproducir(audioNoPuede);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banda"))
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banda"))
            transform.SetParent(null);
    }

    public void AsignarID(int nuevoID)
    {
        ID = nuevoID;
    }
}
