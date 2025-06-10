using System.Collections.Generic;
using UnityEngine;

public class InstanciarBasura : MonoBehaviour
{
    [Header("Variables")]
    public int contadorBasura = 0;
    public bool basuraLlena = false;
    public string eventoTirarBasura;
    public float contStrikeNoComer;
    public float timerStrikeBasura_i;
    public float timerStrikeBasura;
    public bool sonidoAlerta;
    public GameObject imagenBoton;
    [Header("Referencias")]
    public GameManager gameManager;
    public Controlador_Instancias controladorInstancias;
    public Controlador_Fases controladorFases;
    public LimpiarBasura limpiarBasura;
    [Header("Listas")]
    public List<Transform> puntoBasura = new List<Transform>();
    public List<GameObject> objetosBasura = new List<GameObject>();

    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();

        contStrikeNoComer = 0;
        timerStrikeBasura_i = controladorFases.timerNoComer;
        timerStrikeBasura = timerStrikeBasura_i;
    }

    void Update()
    {
        if (contadorBasura >= gameManager.valorLimiteLlena)
        {
            basuraLlena = true;
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            basuraLlena = false;
            this.GetComponent<BoxCollider>().isTrigger = true;
        }

        if (contadorBasura >= gameManager.valorLimiteLlena - 2)
        {
            Controlador_EmotesT.Instance.ReproducirEmoji("Bravo");
            imagenBoton.SetActive(true);

            if (!sonidoAlerta)
            {
                AudioImp.Instance.Reproducir("gatoStrikes");
                sonidoAlerta = true;
            }
        }
        else
        {
            sonidoAlerta = false;
            imagenBoton.SetActive(false);
        }

        if (limpiarBasura.estaPresionando)
        {
            Debug.Log("[InstanciarBasura] Ocultar Boton al presionar la basura");
            imagenBoton.SetActive(false);
        }

        ////

        if (contStrikeNoComer > 0)
        {            
            timerStrikeBasura -= Time.deltaTime;

            if (timerStrikeBasura <= 0)
            {
                contStrikeNoComer = 0;            
            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Comida"))
        {
            int indiceAleatorio = Random.Range(0, puntoBasura.Count);
            contadorBasura++;

            contStrikeNoComer++;
            timerStrikeBasura_i = controladorFases.timerNoComer;
            timerStrikeBasura = timerStrikeBasura_i;

            AudioImp.Instance.Reproducir(eventoTirarBasura);
            other.transform.localScale = other.transform.localScale * 0.8f;            
            other.transform.position = puntoBasura[indiceAleatorio].position;
            objetosBasura.Add(other.gameObject);
        }
    }

    public void BorrarBasura()
    {
        foreach (GameObject obj in objetosBasura)
        { 
            Destroy(obj);
        }

        objetosBasura.RemoveAll(item => item == null);
        contadorBasura = 0;
        contStrikeNoComer = 0;
    }
}
