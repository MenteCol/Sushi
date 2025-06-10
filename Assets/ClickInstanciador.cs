using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;  // Alias para evitar ambigüedad

public class ClickInstanciador : MonoBehaviour
{
    public static ClickInstanciador Instancia { get; private set; }

    public Camera camaraPrincipal;
    public GameObject prefabParaInstanciar;

    // Distancia a la que se instanciará el prefab si no hay colisión
    public float distanciaDefault = 5f;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            if (camaraPrincipal == null)
                camaraPrincipal = Camera.main;
            if (camaraPrincipal == null)
                Debug.LogError("No se encontró la cámara principal con tag 'MainCamera'");

            EnhancedTouchSupport.Enable();

#if UNITY_EDITOR
            UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Enable();
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();

#if UNITY_EDITOR
        UnityEngine.InputSystem.EnhancedTouch.TouchSimulation.Disable();
#endif
    }

    private void Update()
    {
        foreach (var toque in Touch.activeTouches)
        {
            if (toque.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                InstanciarEnToque(toque.screenPosition);
            }
        }
    }

    private void InstanciarEnToque(Vector2 posicionPantalla)
    {
        if (camaraPrincipal == null || prefabParaInstanciar == null)
        {
            Debug.LogWarning("Falta asignar cámara principal o prefab para instanciar.");
            return;
        }

        Ray rayo = camaraPrincipal.ScreenPointToRay(posicionPantalla);
        RaycastHit hit;

        Vector3 posicionInstancia;

        // Si el rayo golpea algún collider, instancia en ese punto
        if (Physics.Raycast(rayo, out hit))
        {
            posicionInstancia = hit.point + Vector3.up * 0.1f; // Leve offset para que no quede dentro del objeto
        }
        else
        {
            // Si no golpea nada, instancia a una distancia fija frente a la cámara
            posicionInstancia = camaraPrincipal.transform.position + camaraPrincipal.transform.forward * distanciaDefault;
        }

        Instantiate(prefabParaInstanciar, posicionInstancia, Quaternion.identity);
        Debug.Log($"Prefab instanciado en {posicionInstancia}");
    }
}
