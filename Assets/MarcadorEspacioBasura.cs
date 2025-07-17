using UnityEngine;

public class MarcadorEspacioBasura : MonoBehaviour
{
    public GameManager gameManager;             // Asigna tu objeto GameManager
    public InstanciarBasura instanciarBasura;   // Asigna tu objeto InstanciarBasura
    public Animator animator;               // Asigna tu objeto Animator    

    [Range(0f, 1f)]
    public float valorSlider = 0.05f;              // Se actualizará automáticamente

    public float valorBasura;
    public float valorLimite;

    public float relacionBasura;
    public float minY = 0.0f;
    public float maxY = 0.95f;

    // Guarda los valores actuales suavizados (inicialízalos igual a transform.localScale.y y transform.localPosition.y)
    private float escalaYSuave;
    private float posicionYSuave;

    public float velocidadSuavizado = 5f;  // Ajusta este valor para que sea más o menos rápido el cambio

    void Start()
    {
        escalaYSuave = transform.localScale.y;
        posicionYSuave = transform.localPosition.y;
        animator = GetComponent<Animator>();
    }

    void Update()
    {      

        valorBasura = instanciarBasura.contadorBasura;
        valorLimite = gameManager.valorLimiteLlena;

        relacionBasura = valorBasura / valorLimite;

        animator.SetFloat("RelacionBasura", relacionBasura);

        if (valorBasura <= 0)
        {
            valorSlider = 0.05f;
        }
        else if (valorBasura > 0)
        {
            valorSlider = Mathf.Clamp01(relacionBasura);
        }

        // Calculamos valores objetivo
        float escalaYObjetivo = Mathf.Lerp(0.05f, 1f, valorSlider);
        float posicionYObjetivo = Mathf.Lerp(maxY, minY, (escalaYObjetivo - 0.05f) / (1f - 0.05f));

        // Suavizamos con Lerp usando Time.deltaTime
        escalaYSuave = Mathf.Lerp(escalaYSuave, escalaYObjetivo, Time.deltaTime * velocidadSuavizado);
        posicionYSuave = Mathf.Lerp(posicionYSuave, posicionYObjetivo, Time.deltaTime * velocidadSuavizado);

        // Aplicamos valores suavizados a la escala y posición
        transform.localScale = new Vector3(
            transform.localScale.x,
            escalaYSuave,
            transform.localScale.z
        );

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            posicionYSuave,
            transform.localPosition.z
        );
    }
}
