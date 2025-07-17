using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controlador_Instancias : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Boost")]
    public bool boostFlag = true;
    public float boostTimer;
    public float boostTimer_i = 5f;

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
    public GameObject panelRecuadro;    
    public List<GameObject> platosInstanciados = new List<GameObject>();

    [Header("Forma Instanciar Comida")]
    public float currentTimer;

    [Header("Referencias")]
    public GameManager gameManager;
    public Controlador_Fases controladorFases;
    [SerializeField] private CortePlatos cortePlatos;
    [SerializeField] private CaracteristicasComida caracteristicasComida;    
    #endregion

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cortePlatos = GameObject.Find("Controlador_Corte").GetComponent<CortePlatos>();
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        currentTimer = controladorFases.intervaloInstancias;
        probBoostFase3 = probGoodFase3 + probBadFase3;
        boostTimer = boostTimer_i;

        if (depuracion)
            Debug.Log($"{gameObject.name}: Inicialización de referencias y timers.");
    }

    void Update()
    {
        #region Area Limpiar Platos

        if (platosInstanciados.Count >= gameManager.maxPlatosLimpiar && !areaActivada)
        {
            cortePlatos.ActivarAreaCorte();
            panelCorte.GetComponent<Image>().raycastTarget = true;
            areaActivada = true;
            panelRecuadro.SetActive(true);
            if (depuracion)
                Debug.Log($"{gameObject.name}: Área de corte activada.");
        }
        else if (areaActivada && platosInstanciados.Count < gameManager.maxPlatosLimpiar)
        {
            cortePlatos.ActivarAreaCorte();
            panelCorte.GetComponent<Image>().raycastTarget = false;
            areaActivada = false;
            panelRecuadro.SetActive(false);
            if (depuracion)
                Debug.Log($"{gameObject.name}: Área de corte desactivada.");
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

        if (!boostFlag)
        {
            BoostTimer();
        }
    }

    public void InstanciarComida()
    {
        float probabilidad = Random.Range(0f, 100f);
        int idComida;

        switch (gameManager.fase)
        {
            case 1:
                idComida = (probabilidad < probGoodFase1) ? 1 : 2;
                break;
            case 2:
                idComida = (probabilidad < probGoodFase2) ? 1 : 2;
                break;
            case 3:
                if (probabilidad < probGoodFase3)
                    idComida = 1;
                else if (probabilidad < probBoostFase3)
                    idComida = 2;
                else
                    idComida = 3;
                break;
            default:
                idComida = (probabilidad < 80f) ? 1 : 2;
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
                if (boostFlag) listaPrefabs = caracteristicasComida.prefabsComidaBoost;
                else
                {
                    listaPrefabs = caracteristicasComida.prefabsComidaMala;
                    idComida = 2;
                    if (depuracion)
                        Debug.LogWarning($"{gameObject.name}: Boost no activo, instanciando comida mala en su lugar.");
                }
                break;
        }

        if (listaPrefabs == null || listaPrefabs.Count == 0)
        {
            if (depuracion)
                Debug.LogWarning($"{gameObject.name}: La lista de prefabs para el ID {idComida} está vacía.");
            return;
        }

        int indiceAleatorio = Random.Range(0, listaPrefabs.Count);
        ClickObjetosPuntos prefabSeleccionado = listaPrefabs[indiceAleatorio];

        GameObject nuevaComida = Instantiate(prefabSeleccionado.gameObject, origenInstancia.position, Quaternion.identity);
        nuevaComida.GetComponent<ClickObjetosPuntos>().ID = idComida;

        if (depuracion)
            Debug.Log($"{gameObject.name}: Instanciada comida con ID {idComida}.");
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

        if (depuracion)
            Debug.Log($"{gameObject.name}: Instanciado plato con ID {iD}.");
    }

    public void DestruirPlatos()
    {
        foreach (GameObject obj in platosInstanciados)
        {
            Destroy(obj);
        }
        if (depuracion)
            Debug.Log($"{gameObject.name}: Todos los platos han sido destruidos.");
    }

    public void BoostTimer()
    {
        boostTimer -= Time.deltaTime;

        if (boostTimer <= 0f)
        {
            boostFlag = true;
            boostTimer = boostTimer_i;
            if (depuracion)
                Debug.Log($"{gameObject.name}: Boost timer ha terminado.");
        }
    }
}