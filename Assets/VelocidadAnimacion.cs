using UnityEngine;

public class VelocidadAnimacion : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Varios")]
    public GameObject botonBasura;
    public Material materialBoton;

    [Header("Animacion")]
    public float velocidadAnimacionMin = 1.0f;
    public float velocidadAnimacionMax = 3.0f;
    public Animator animatorBoton;
    public Animator animatorPanza;

    [Header("Referencias")]
    public InstanciarBasura instanciarBasura;
    public GameManager gameManager;
    public LimpiarBasura limpiarBasura;
    #endregion

    public bool normal = true;
    public bool presionado = false;
    public bool reproducirSonido = false;

    void Start()
    {
        materialBoton = botonBasura.GetComponent<MeshRenderer>().material;
        animatorBoton = botonBasura.GetComponent<Animator>();

        if (depuracion)
            Debug.Log($"{gameObject.name}: Inicializacion de referencias de boton y animador.");
    }

    void Update()
    {
        if (instanciarBasura == null || gameManager == null)
            return;
                
        int contador = instanciarBasura.contadorBasura;
        int limite = gameManager.valorLimiteLlena;
                
        int rangoMax = Mathf.Max(1, limite - 2); // Evita división por cero
                
        float t = Mathf.Clamp01((float)contador / rangoMax);
                
        float velocidad = Mathf.Lerp(velocidadAnimacionMin, velocidadAnimacionMax, t);

        if (animatorBoton != null)
            animatorBoton.speed = velocidad;

        if (animatorPanza != null)
            animatorPanza.speed = velocidad;

        normal = contador <= (limite - 2);

        presionado = limpiarBasura.estaPresionando;

        if (presionado && !reproducirSonido)
        { 
            AudioImp.Instance.Reproducir("click");            
            reproducirSonido = true;
            animatorBoton.SetBool("Presionado", true);
        }
        else if (!presionado)
        {
            reproducirSonido = false;
            animatorBoton.SetBool("Presionado", false);
        }        

        if (depuracion)
        {
            Debug.Log($"{gameObject.name}: contadorBasura={contador}, velocidadAnimacion={velocidad:F2}, normal={normal}");
        }
    }
}