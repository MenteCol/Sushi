using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Controlador_Puntos : MonoBehaviour
{
    #region Variables

    [Header("Valores")]
    [SerializeField] private TextMeshProUGUI puntajeText;
    public float tiempo = 0;
    public int puntaje = 0;
    public bool tieneHambre;
    public string eventoMalestar;
    public bool reprodujoSonidoMalestar;
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
    public float factorReduccion;
    [SerializeField] private Slider llenuraSlider;
    public Image fondoEstomago;
    [Header("Lista Objetos Escena")]
    public List<ClickObjetosPuntos> objetosComida = new List<ClickObjetosPuntos>();
    [Header("Referencias")]
    public GameManager gameManager;
    public GameOver_Controller gameOverController;
    [SerializeField] private Controlador_Instancias controladorInstancias;
    [SerializeField] private Controlador_Banda controladorBanda;
    public Controlador_Fases controladorFases;

    #endregion

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
                Debug.LogWarning("El objeto " + obj.name + " con tag 'Comida' no tiene el componente ClickObjetosPuntos.");
            }
        }
        
        Debug.Log("Se encontraron " + objetosComida.Count + " objetos con el tag 'Comida'.");
                
        if (puntajeText != null)
            puntajeText.text = puntaje.ToString("D2");
        
        if (llenuraSlider != null)
            llenuraSlider.value = llenura;

        if (fondoEstomago != null)
            fondoEstomago.fillAmount = llenura;

        actualTimerEnfermo = gameManager.timerEnfermo;
        actualTimerAcumulacion = gameManager.timerAcumulacionEnfermo;

        factorReduccion = gameManager.fr_llenura1;
    }
    
    void Update()
    {        
        if(controladorFases.enPausa)
            return;

        fondoEstomago.fillAmount = llenura;

        ReduccionLlenura();
        ContadorMalestar();
        ActualizarFase();
        ActualizarMarcadorGUI();

        if (llenura >= 1.2 && !estaLleno) // PENALIZACION LLENURA
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

    }

    public void SumarPuntos(int puntos, float llenuraComida, int id, int malestarComida = 0)
    {
        puntaje += puntos;
        malestar += malestarComida;

        SumarLlenura(llenuraComida);
        controladorInstancias.InstanciarPlatos(id);
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
        else if (malestar == 3) // Reiniciar Timer
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
}
