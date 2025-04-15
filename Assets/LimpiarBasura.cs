using UnityEngine;

public class LimpiarBasura : MonoBehaviour
{
    public GameManager gameManager;
    public float contador;    
    private InstanciarBasura instanciarBasura;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        instanciarBasura = GameObject.Find("ColliderBasura").GetComponent<InstanciarBasura>();
    }

    private void Update()
    {
        if (contador >= gameManager.tiempoLimpiarBasura)
        {
            ReiniciarBasura();
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    // Estas funciones se usan en el Editor o en plataformas de escritorio
    private void OnMouseDrag()
    {
        if (contador <= gameManager.tiempoLimpiarBasura)
        {
            contador += Time.deltaTime;
        }
    }

    private void OnMouseUp()
    {
        contador = 0;
    }

#elif UNITY_ANDROID || UNITY_IOS
    // Se usa para dispositivos móviles
    private bool isDragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Toma el primer toque (en caso de multitouch)
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Se realiza un raycast para ver si el toque inició sobre este objeto
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        if (contador <= gameManager.tiempoLimpiarBasura)
                        {
                            contador += Time.deltaTime;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                    {
                        contador = 0;
                        isDragging = false;
                    }
                    break;
            }
        }
    }
#else
    // Fallback para otras plataformas, usando la entrada de ratón
    private void OnMouseDrag()
    {
        if (contador <= gameManager.tiempoLimpiarBasura)
        {
            contador += Time.deltaTime;
        }
    }

    private void OnMouseUp()
    {
        contador = 0;
    }
#endif

    public void ReiniciarBasura()
    {
        instanciarBasura.BorrarBasura();
    }
}
