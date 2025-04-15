using System.Collections.Generic;
using UnityEngine;

public class Controlador_Instancias : MonoBehaviour
{
    public GameManager gameManager;
    [Header("Depuración")]
    public bool test;
    public KeyCode teclaPruebaComida;
    public KeyCode teclaPruebaPlatos;
    [Header("Variables Instancia")]
    [SerializeField] private Transform origenInstancia; 
    [Header("Instancia Platos")]
    public bool basuraLlena = false; //pendiente por cambio¿?
    public bool areaActivada = false;
    [SerializeField] private GameObject platosPrefab;
    [SerializeField] private Transform origenInstanciaPlatos;
    public List<GameObject> platosInstanciados = new List<GameObject>();
    [Header("Forma Instanciar Comida")]    
    private float currentTimer;
    [Header("Referencias")]
    [SerializeField] private CortePlatos cortePlatos;
    [SerializeField] private CaracteristicasComida caracteristicasComida; //Escripteable

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cortePlatos = GameObject.Find("Controlador_Corte").GetComponent<CortePlatos>();
        currentTimer = gameManager.timerInstanciaComida;
    }

    void Update()
    {        
        if (Input.GetKeyDown(teclaPruebaComida) && test)
        {
            Debug.Log("[Controlador_Instancias] Presionando " + teclaPruebaComida + " en " + this.name);
            InstanciarComida();
        }

        if (Input.GetKeyDown(teclaPruebaPlatos) && test)
        {
            Debug.Log("[Controlador_Instancias] Presionando " + teclaPruebaPlatos);
            DestruirPlatos();
        }

        if (platosInstanciados.Count >= 9 && !areaActivada)
        {
            cortePlatos.ActivarAreaCorte();
            areaActivada = true;
        }
        else if (areaActivada && platosInstanciados.Count < 9)
        {
            cortePlatos.ActivarAreaCorte();
            areaActivada = false;
        }
                
        platosInstanciados.RemoveAll(item => item == null);

        // TIMER
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0f)
        {
            InstanciarComida();
            currentTimer = gameManager.timerInstanciaComida;
        }
    }

    public void InstanciarComida()
    {

        float probabilidad = Random.Range(0f, 100f);
        int idComida;

        switch (gameManager.fase)
        {
            case 1: // 80% buena, 20% mala
                if (probabilidad < 85f)
                    idComida = 1;
                else
                    idComida = 2;
                break;
            case 2:// 70% buena, 30% mala
                if (probabilidad < 70f)
                    idComida = 1;
                else
                    idComida = 2;
                break;
            case 3:// 60% buena, 35% mala, 5% boost
                if (probabilidad < 60f)
                    idComida = 1;
                else if (probabilidad < 95f) // 60 + 35 = 95
                    idComida = 2;
                else
                    idComida = 3;
                break;
            default:
                if (probabilidad < 80f)
                    idComida = 1;
                else
                    idComida = 2;
                break;
        }

        List<ClickObjetosPuntos> listaPrefabs = null;

        switch (idComida)
        {
            case 1:
                listaPrefabs = caracteristicasComida.prefabsComidaBuena;
                break;
            case 2:
                listaPrefabs = caracteristicasComida.prefabsComidaMala;
                break;
            case 3:
                listaPrefabs = caracteristicasComida.prefabsComidaBoost;
                break;
        }

        if (listaPrefabs == null || listaPrefabs.Count == 0)
        {
            Debug.LogWarning("La lista de prefabs para el ID " + idComida + " está vacía.");
            return;
        }

        int indiceAleatorio = Random.Range(0, listaPrefabs.Count);
        ClickObjetosPuntos prefabSeleccionado = listaPrefabs[indiceAleatorio];

        GameObject nuevaComida = Instantiate(prefabSeleccionado.gameObject, origenInstancia.position, Quaternion.identity);
        nuevaComida.GetComponent<ClickObjetosPuntos>().ID = idComida;
    }

    public void InstanciarPlatos()
    {
        Vector3 posicionInstancia = origenInstanciaPlatos.position;
        posicionInstancia.y += 0.5f * platosInstanciados.Count;

        GameObject nuevaComida = Instantiate(platosPrefab, posicionInstancia, Quaternion.identity);
        platosInstanciados.Add(nuevaComida);
    }

    public void DestruirPlatos()
    {
        foreach (GameObject obj in platosInstanciados)
        {
            Destroy(obj);
        }
    }
}
