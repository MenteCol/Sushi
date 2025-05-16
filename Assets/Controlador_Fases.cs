using UnityEngine;

public class Controlador_Fases : MonoBehaviour
{
    [Header("Valores de Fases")]
    [Space]
    [Range(0, 1)]
    public float fase;

    [Header("Configuración de Puntaje")]
    public float puntajeMaximoParaFaseUno;

    [Header("Curvas de Progreso")]
    [Tooltip("Curva para la velocidad de la banda. El eje X representa el progreso (0-1) y el eje Y el valor normalizado (0-1).")]
    public AnimationCurve curvaVelocidadBanda = AnimationCurve.Linear(0, 0, 1, 1);

    [Tooltip("Curva para el factor de reducción. El eje X representa el progreso (0-1) y el eje Y el valor normalizado (0-1).")]
    public AnimationCurve curvaFactorReduccion = AnimationCurve.Linear(0, 0, 1, 1);

    [Tooltip("Curva para el intervalo de instancias. El eje X representa el progreso (0-1) y el eje Y el valor normalizado (0-1).")]
    public AnimationCurve curvaIntervaloInstancias = AnimationCurve.Linear(0, 0, 1, 1);

    [Tooltip("Curva para el timer de no comer. El eje X representa el progreso (0-1) y el eje Y el valor normalizado (0-1).")]
    public AnimationCurve curvaTimerNoComer = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Valores Iniciales")]
    [SerializeField] private float velocidadBandaMin = 1f;
    [SerializeField] private float factorReduccionMin = 0.05f;
    [SerializeField] private float intervaloInstanciasMin = 3f;
    [SerializeField] private float timerNoComerMin = 3f;

    [Header("Valores Maximos")]
    [SerializeField] private float velocidadBandaMax = 10f;
    [SerializeField] private float factorReduccionMax = 0.16f;
    [SerializeField] private float intervaloInstanciasMax = 0.5f;
    [SerializeField] private float timerNoComerMax;

    [Header("Valores de Pausa")]
    [SerializeField] private float velocidadBandaPausa = 0f;
    [SerializeField] private float factorReduccionPausa = 0f;

    [Header("Estado")]
    public bool enPausa = false;

    [Header("Valores Actuales")]
    public float velocidadBanda;
    public float factorReduccion;
    public float intervaloInstancias;
    public float timerNoComer;

    [Header("Referencias")]
    public GameManager gameManager;
    public MenuPausaManager menuPausaManager;

    private void Start()
    {
        // Busca las referencias necesarias
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        menuPausaManager = GameObject.Find("Controlador_MenuPausa").GetComponent<MenuPausaManager>();

        // Inicializa los valores
        ActualizarValores();
    }

    void Update()
    {
        if (!enPausa)
        {
            ActualizarValores();
        }
        else
        {
            ValoresPausa();
        }
    }

    public void ActualizarValores()
    {
        // Calcula el progreso de la fase basado en los puntos (normalizado entre 0 y 1)
        fase = Mathf.Clamp01(gameManager.puntos / puntajeMaximoParaFaseUno);

        // Usa cada curva para determinar el valor normalizado para cada parámetro
        float factorVelocidadBanda = curvaVelocidadBanda.Evaluate(fase);
        float factorFactorReduccion = curvaFactorReduccion.Evaluate(fase);
        float factorIntervaloInstancias = curvaIntervaloInstancias.Evaluate(fase);
        float factorTimerNoComer = curvaTimerNoComer.Evaluate(fase);

        // Aplica los valores interpolados según las curvas respectivas
        velocidadBanda = Mathf.Lerp(velocidadBandaMin, velocidadBandaMax, factorVelocidadBanda);
        factorReduccion = Mathf.Lerp(factorReduccionMin, factorReduccionMax, factorFactorReduccion);
        intervaloInstancias = Mathf.Lerp(intervaloInstanciasMin, intervaloInstanciasMax, factorIntervaloInstancias);
        timerNoComer = Mathf.Lerp(timerNoComerMin, timerNoComerMax, factorTimerNoComer);
    }

    public void ValoresPausa()
    {
        velocidadBanda = velocidadBandaPausa;
        factorReduccion = factorReduccionPausa;
    }
}
