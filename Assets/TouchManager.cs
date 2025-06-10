using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Camera))]
public class TouchManager : MonoBehaviour
{
    public static TouchManager Instancia { get; private set; }

    [Header("Configuración de Marcadores")]
    [Tooltip("Prefab que se instanciará en la posición del toque")]
    [SerializeField] private GameObject prefabMarcadorToque;

    [Tooltip("Distancia default desde la cámara para instanciar marcadores")]
    [SerializeField] private float distanciaDefault = 5f;

    [Tooltip("Activar para visualizar marcadores de toque")]
    [SerializeField] private bool testGizmo = false;

    private Camera camaraPrincipal;
    private HashSet<int> dedosRastreando = new HashSet<int>();

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            camaraPrincipal = GetComponent<Camera>();

            if (camaraPrincipal == null)
                camaraPrincipal = Camera.main;

            if (camaraPrincipal == null)
                Debug.LogError("No se encontró la cámara principal con tag 'MainCamera'");

            InicializarSistemaTouch();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarSistemaTouch()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += ManejarToqueIniciado;
        Touch.onFingerUp += ManejarToqueFinalizado;

#if UNITY_EDITOR
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
#endif
    }

    private void OnDestroy()
    {
        Touch.onFingerDown -= ManejarToqueIniciado;
        Touch.onFingerUp -= ManejarToqueFinalizado;
        EnhancedTouchSupport.Disable();

#if UNITY_EDITOR
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Disable();
#endif
    }

    private void ManejarToqueIniciado(Finger dedo)
    {
        if (dedosRastreando.Contains(dedo.index))
            return;

        dedosRastreando.Add(dedo.index);
        ProcesarToque(dedo.currentTouch.screenPosition, dedo.index);
    }

    private void ManejarToqueFinalizado(Finger dedo)
    {
        dedosRastreando.Remove(dedo.index);
    }

    private void ProcesarToque(Vector2 posicionPantalla, int indiceDedo)
    {
        if (EstaSobreUI(posicionPantalla))
            return;

        var rayo = camaraPrincipal.ScreenPointToRay(posicionPantalla);
        bool impacto = Physics.Raycast(rayo, out RaycastHit hit);

        Vector3 posicionInstancia = impacto ?
            hit.point + Vector3.up * 0.1f :
            camaraPrincipal.transform.position + camaraPrincipal.transform.forward * distanciaDefault;

        if (testGizmo)
            InstanciarMarcador(posicionInstancia);

        if (impacto)
        {
            var clickable = hit.collider.GetComponent<ClickObjetosPuntos>();
            clickable?.RegistrarToque(indiceDedo);
        }

        // Opcional: Visualizar el rayo en escena para depuración
        Debug.DrawRay(rayo.origin, rayo.direction * 10f, Color.cyan, 1f);
    }

    private bool EstaSobreUI(Vector2 posicionPantalla)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = posicionPantalla
        };

        List<RaycastResult> resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultados);

        return resultados.Count > 0;
    }

    private void InstanciarMarcador(Vector3 posicion)
    {
        if (prefabMarcadorToque != null)
        {
            var instancia = Instantiate(prefabMarcadorToque, posicion, Quaternion.identity);
            Destroy(instancia, 1f);
        }
        else
        {
            Debug.LogWarning("Prefab marcador no asignado en TouchManager");
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcesarToque(Mouse.current.position.ReadValue(), -1);
        }
    }
#endif
}
