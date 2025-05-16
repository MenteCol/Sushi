using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controlador_Instancias : MonoBehaviour
{
    #region Variables

    public GameManager gameManager;
    [Header("Depuración")]
    public bool test;
    [Header("Variables Instancia")]
    [SerializeField] private Transform origenInstancia;
    [Space]
    [SerializeField] private float probGoodFase1;
    [Space]
    [SerializeField] private float probGoodFase2;
    [Space]
    [SerializeField] private float probGoodFase3;
    [SerializeField] private float probBadFase3;
    [SerializeField] private float probBoostFase3;
    [Header("Instancia Platos")]    
    public bool areaActivada = false;
    [SerializeField] private GameObject platosPrefab;
    [SerializeField] private Transform origenInstanciaPlatos;
    public GameObject panelCorte;
    public List<GameObject> platosInstanciados = new List<GameObject>();    
    [Header("Forma Instanciar Comida")]    
    public float currentTimer;
    [Header("Referencias")]
    [SerializeField] private CortePlatos cortePlatos;
    [SerializeField] private CaracteristicasComida caracteristicasComida; //Escripteable
    public Controlador_Fases controladorFases;

    #endregion

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cortePlatos = GameObject.Find("Controlador_Corte").GetComponent<CortePlatos>();
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent <Controlador_Fases>();
        currentTimer = controladorFases.intervaloInstancias;
        probBoostFase3 = probGoodFase3 + probBadFase3;
    }

    void Update()
    {
        #region Area Limpiar Platos

        if (platosInstanciados.Count >= gameManager.maxPlatosLimpiar && !areaActivada)
        {
            cortePlatos.ActivarAreaCorte();
            panelCorte.GetComponent<Image>().raycastTarget = true;
            areaActivada = true;
        }
        else if (areaActivada && platosInstanciados.Count < gameManager.maxPlatosLimpiar)
        {
            cortePlatos.ActivarAreaCorte();
            panelCorte.GetComponent<Image>().raycastTarget = false;
            areaActivada = false;
        }
                
        platosInstanciados.RemoveAll(item => item == null);

        #endregion

        if (!controladorFases.enPausa)
        {
            currentTimer -= Time.deltaTime;
        }

        if (currentTimer <= 0f)
        {
            InstanciarComida();
            currentTimer = controladorFases.intervaloInstancias;
        }
    }

    public void InstanciarComida()
    {
        float probabilidad = Random.Range(0f, 100f);
        int idComida;

        switch (gameManager.fase)
        {
            case 1: // 80% buena, 20% mala
                if (probabilidad < probGoodFase1)
                    idComida = 1;
                else
                    idComida = 2;
                break;
            case 2:// 70% buena, 30% mala
                if (probabilidad < probGoodFase2)
                    idComida = 1;
                else
                    idComida = 2;
                break;
            case 3:// 60% buena, 35% mala, 5% boost
                if (probabilidad < probGoodFase3)
                    idComida = 1;
                else if (probabilidad < probBoostFase3) // 60 + 35 = 95
                    idComida = 2;
                else
                    idComida = 3;
                break;

        //// DEFAULT
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

    public void InstanciarPlatos(int iD)
    {
        Vector3 posicionInstancia = origenInstanciaPlatos.position;
        posicionInstancia.y += 0.5f * platosInstanciados.Count;

        switch (iD)
        {
            case 1:
                platosPrefab = caracteristicasComida.prefabsPlatos[0];
                break;
            case 2:
                platosPrefab = caracteristicasComida.prefabsPlatos[1];
                break;
            case 3:
                platosPrefab = caracteristicasComida.prefabsPlatos[2];
                break;
        }

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
