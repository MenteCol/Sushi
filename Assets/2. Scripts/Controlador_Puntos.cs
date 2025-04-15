using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Controlador_Puntos : MonoBehaviour
{
    public GameManager gameManager;
           
    [Header("Valores")]
    [SerializeField] private TextMeshProUGUI puntajeText;
    public float tiempo = 0;
    public int puntaje = 0;
    [Header("Malestar")]
    public bool estaEnfermo;
    public int malestar = 0;
    [Header("Contadores Enfermo")]    
    public float actualTimerEnfermo;    
    public float actualTimerAcumulacion;
    [Header("Llenura")]
    public bool estaLleno;
    public float llenura = 0;
    public float factorReduccion;
    [SerializeField] private Slider llenuraSlider;
    [Header("Referencias")]
    [SerializeField] private Controlador_Instancias controladorInstancias;
    [SerializeField] private Controlador_Banda controladorBanda;
    [Header("Lista Objetos Escena")]
    public List<ClickObjetosPuntos> objetosComida = new List<ClickObjetosPuntos>();

    void Start()
    {
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

        actualTimerEnfermo = gameManager.timerEnfermo;
        actualTimerAcumulacion = gameManager.timerAcumulacionEnfermo;

        factorReduccion = gameManager.fr_llenura1;
    }
    
    void Update()
    {        
        ReduccionLlenura();
        ActualizarFase();

        if (puntajeText != null)
        {
            puntajeText.text = puntaje.ToString("D2");
        }

        if (llenuraSlider != null)
        {
            llenuraSlider.value = llenura;
        }

        #region Malestar

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
        #endregion

        #region Llenura
        if (llenura >= 1.19 && !estaLleno)
        {           
            StartCoroutine(DesLlenura(gameManager.valorReduccion, gameManager.valorVelReduccion));            
        }

        #endregion

    }

    public void SumarPuntos(int puntos, float llenuraComida, int malestarComida = 0)
    {
        puntaje += puntos;
        malestar += malestarComida;

        if (llenura <= 1.3f)
        {
            llenura += llenuraComida;
        }

        if (gameManager.timerInstanciaComida > 0.5f) // Valor minimo instancia
        {
            gameManager.timerInstanciaComida -= 0.05f;
        }

        if (controladorBanda.velocidadBanda < 9) //Valor maximo velocidad banda
        {
            controladorBanda.velocidadBanda += 0.05f;
        }

        controladorInstancias.InstanciarPlatos();
    }

    public void ReduccionLlenura()
    {
        if (llenura > 0)
        {            
            llenura = Mathf.Max(0, llenura - factorReduccion * Time.deltaTime);
        }
    }

    public void ActualizarFase()
    {
        if (puntaje >= 30 && gameManager.fase == 1)
        {
            gameManager.fase = 2;
            factorReduccion = gameManager.fr_llenura2;
        }

        if (puntaje >= 60 && gameManager.fase == 2)
        {
            gameManager.fase = 3;
            factorReduccion = gameManager.fr_llenura3;
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
        
        estaLleno = false;
    }
}
