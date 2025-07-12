using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Controlador_Puntos : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Combos")]
    public bool reprodujoSonidoMalestar;
    public int combo;
    public int combo_i = 0;
    public float timerRestartCombo;
    public float timerRestartCombo_i;

    [Header("Valores")]
    public bool tieneHambre;
    public int puntaje = 0;
    public float tiempo = 0;

    [Header("Varios")]
    public string eventoMalestar;
    [SerializeField] private TextMeshProUGUI puntajeText;

    [Header("Malestar")]
    public bool estaEnfermo;
    public bool estaVomitando;
    public int malestar = 0;

    [Header("Contadores Enfermo")]
    public float actualTimerEnfermo;
    public float actualTimerAcumulacion;

    [Header("Llenura")]
    public bool estaLleno;
    public float llenura = 0;
    [Tooltip("Este Valor es Factor Reduccion del script Controlador_Fases")]
    public float factorReduccion;
    public Image fondoEstomago;
    [SerializeField] private Slider llenuraSlider;

    [Header("Lista Objetos Escena")]
    public List<ClickObjetosPuntos> objetosComida = new List<ClickObjetosPuntos>();

    [Header("Referencias")]
    public GameManager gameManager;
    public GameOver_Controller gameOverController;
    public Controlador_Fases controladorFases;
    public Controlador_Combos controladorCombos;
    [SerializeField] private Controlador_Instancias controladorInstancias;
    [SerializeField] private Controlador_Banda controladorBanda;
    #endregion

    [SerializeField] private GameObject prefabParticula;

    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();
        controladorBanda = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Banda>();

        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("Comida");

        foreach (GameObject obj in objetosConTag)
        {
            ClickObjetosPuntos componente = obj.GetComponent<ClickObjetosPuntos>();

            if (componente != null)
            {
                objetosComida.Add(componente);
            }
            else
            {
                if (depuracion)
                    Debug.LogWarning($"{gameObject.name}: El objeto {obj.name} con tag 'Comida' no tiene el componente ClickObjetosPuntos.");
            }
        }

        if (depuracion)
            Debug.Log($"{gameObject.name}: Se encontraron {objetosComida.Count} objetos con el tag 'Comida'.");

        if (puntajeText != null)
            puntajeText.text = puntaje.ToString("D2");

        if (llenuraSlider != null)
            llenuraSlider.value = llenura;

        if (fondoEstomago != null)
            fondoEstomago.fillAmount = llenura;

        actualTimerEnfermo = gameManager.timerEnfermo;
        actualTimerAcumulacion = gameManager.timerAcumulacionEnfermo;

        factorReduccion = controladorFases.factorReduccion;

        combo = combo_i;
        timerRestartCombo = timerRestartCombo_i;
    }

    void Update()
    {
        if (controladorFases.enPausa)
            return;

        fondoEstomago.fillAmount = llenura;

        ReduccionLlenura();
        ContadorMalestar();
        ActualizarFase();
        ActualizarMarcadorGUI();

        if (llenura >= 1.2 && !estaLleno)
        {
            StartCoroutine(DesLlenura(gameManager.valorReduccion, gameManager.valorVelReduccion));
        }

        if (estaEnfermo || estaLleno)
        {
            if (!reprodujoSonidoMalestar)
            {
                AudioImp.Instance.Reproducir(eventoMalestar);
                estaVomitando = true;
                reprodujoSonidoMalestar = true;
            }
        }
        else
        {
            estaVomitando = false;
            reprodujoSonidoMalestar = false;
        }

        if (combo > 0)
        {
            timerRestartCombo -= Time.deltaTime;

            if (timerRestartCombo <= 0)
            {
                combo = 0;
                timerRestartCombo = 0;
                if (depuracion)
                    Debug.Log($"{gameObject.name}: Combo reiniciado por inactividad");
            }
        }

        if (combo == 10) Controlador_EmotesT.Instance.ReproducirEmoji("Feliz");
        if (combo == 40) Controlador_EmotesT.Instance.ReproducirEmoji("Feliz");
        if (combo == 80) Controlador_EmotesT.Instance.ReproducirEmoji("Feliz");
        if (combo == 120) Controlador_EmotesT.Instance.ReproducirEmoji("Feliz");
    }

    public void SumarPuntos(int puntos, float llenuraComida, int id, int comboClick, int malestarComida = 0)
    {
        puntaje += puntos;
        malestar += malestarComida;

        if (comboClick > 0)
        {
            combo += comboClick;
            timerRestartCombo = timerRestartCombo_i;
        }

        SumarLlenura(llenuraComida);
        controladorInstancias.InstanciarPlatos(id);
        controladorCombos.MostrarCombo();
    }

    public void SumarLlenura(float llenuraComida)
    {
        if (llenura <= 1.2f)
        {
            llenura += llenuraComida;
        }
    }

    public void ReduccionLlenura()
    {
        factorReduccion = controladorFases.factorReduccion;

        if (llenura > 0)
        {
            llenura = Mathf.Max(0, llenura - factorReduccion * Time.deltaTime);
        }

        if (llenura <= 0.0001)
        {
            tieneHambre = true;
            Controlador_EmotesT.Instance.ReproducirEmoji("Hambre");
        }
        else
        {
            tieneHambre = false;
        }
    }

    public void ActualizarFase()
    {
        if (puntaje >= 30 && gameManager.fase == 1)
        {
            gameManager.fase = 2;
        }

        if (puntaje >= 60 && gameManager.fase == 2)
        {
            gameManager.fase = 3;
        }
    }

    public IEnumerator DesLlenura(float valorObjetivo, float velocidad)
    {
        estaLleno = true;

        while (llenura > valorObjetivo)
        {
            llenura -= velocidad * Time.deltaTime;

            if (llenura < valorObjetivo)
            {
                llenura = valorObjetivo;
            }
            yield return null;
        }

        estaVomitando = false;
        estaLleno = false;
    }

    public void ContadorMalestar()
    {
        if (malestar > 0 && malestar < gameManager.valorMalestarMaximo)
        {
            if (actualTimerAcumulacion >= 0)
            {
                actualTimerAcumulacion -= Time.deltaTime;

                if (actualTimerAcumulacion <= 0)
                {
                    malestar = 0;
                    actualTimerAcumulacion = gameManager.timerAcumulacionEnfermo;
                }
            }
        }
        else if (malestar == 3)
        {
            actualTimerAcumulacion = gameManager.timerAcumulacionEnfermo;
        }

        if (malestar == 3)
        {
            estaEnfermo = true;

            if (actualTimerEnfermo >= 0)
            {
                actualTimerEnfermo -= Time.deltaTime;

                if (actualTimerEnfermo <= 0)
                {
                    estaEnfermo = false;
                    malestar = 0;
                    actualTimerEnfermo = gameManager.timerEnfermo;
                }
            }
        }
    }

    public void ActualizarMarcadorGUI()
    {
        if (puntajeText != null)
        {
            puntajeText.text = puntaje.ToString("D2");
        }

        if (llenuraSlider != null)
        {
            llenuraSlider.value = llenura;
        }
    }

    public void MostrarParticula(Transform ubicacionClick)
    {
        GameObject particula = Instantiate(prefabParticula, ubicacionClick.position, Quaternion.identity);
    }
}