using System.Collections.Generic;
using UnityEngine;

public class Controlador_Banda : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Instancia Banda Collider")]
    [SerializeField] private GameObject prefabBandaCollider;
    [SerializeField] private Transform inicioBanda;
    [SerializeField] private Transform finalBanda;

    [Header("Instancia Banda Asset")]
    [SerializeField] private GameObject prefabBandaAsset;
    [SerializeField] private Transform inicioBandaAsset;
    [SerializeField] private Transform finalBandaAsset;

    [Header("Movimiento Banda")]
    public float velocidadBandas; // ReferenciaExterna para velocidad de bandas
    [Space]
    public List<GameObject> bandasCollider = new List<GameObject>();
    public List<GameObject> bandasAsset = new List<GameObject>();    

    [Header("Referencias")]
    public Controlador_Fases controladorFases;    
    #endregion

    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        ActualizarListaBandasCollider();
    }

    void Update()
    {
        velocidadBandas = controladorFases.velocidadBanda;

        ActualizarListaBandasCollider();
        ActualizarListaBandasAsset();

        MoverBandasCollider();
        MoverBandasAsset();
    }

    public void InstanciaBandaCollider()
    {
        GameObject nuevaBanda = Instantiate(prefabBandaCollider, inicioBanda.position, Quaternion.identity);
        bandasCollider.Add(nuevaBanda);
        if (depuracion)
            Debug.Log("Instanciada nueva BandaCollider");
    }

    public void InstanciaBandaAsset()
    {
        GameObject nuevaBanda = Instantiate(prefabBandaAsset, inicioBandaAsset.position, Quaternion.identity);
        bandasAsset.Add(nuevaBanda);
        if (depuracion)
            Debug.Log("Instanciada nueva BandaAsset");
    }

    private void MoverBandasCollider()
    {
        for (int i = bandasCollider.Count - 1; i >= 0; i--)
        {
            if (bandasCollider[i] != null)
            {
                bandasCollider[i].transform.position = Vector3.MoveTowards(
                    bandasCollider[i].transform.position,
                    finalBanda.position,
                    controladorFases.velocidadBanda * Time.deltaTime
                );

                if (Vector3.Distance(bandasCollider[i].transform.position, finalBanda.position) < 0.0001f)
                {
                    Destroy(bandasCollider[i]);
                    if (depuracion)
                        Debug.Log("BandaCollider destruida y reemplazada");
                    InstanciaBandaCollider();
                    bandasCollider.RemoveAt(i);
                }
            }
        }
    }

    private void MoverBandasAsset()
    {
        for (int i = bandasAsset.Count - 1; i >= 0; i--)
        {
            if (bandasAsset[i] != null)
            {
                bandasAsset[i].transform.position = Vector3.MoveTowards(
                    bandasAsset[i].transform.position,
                    finalBandaAsset.position,
                    controladorFases.velocidadBanda * Time.deltaTime
                );

                if (Vector3.Distance(bandasAsset[i].transform.position, finalBandaAsset.position) < 0.0001f)
                {
                    Destroy(bandasAsset[i]);
                    if (depuracion)
                        Debug.Log("BandaAsset destruida y reemplazada");
                    InstanciaBandaAsset();
                    bandasAsset.RemoveAt(i);
                }
            }
        }
    }

    private void ActualizarListaBandasCollider()
    {
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("Banda");
        bandasCollider.RemoveAll(b => b == null);

        foreach (GameObject obj in objetosConTag)
        {
            if (!bandasCollider.Contains(obj))
            {
                bandasCollider.Add(obj);
                if (depuracion)
                    Debug.Log("BandaCollider añadida a la lista");
            }
        }
    }

    private void ActualizarListaBandasAsset()
    {
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("BandaAsset");
        bandasAsset.RemoveAll(b => b == null);

        foreach (GameObject obj in objetosConTag)
        {
            if (!bandasAsset.Contains(obj))
            {
                bandasAsset.Add(obj);
                if (depuracion)
                    Debug.Log("BandaAsset añadida a la lista");
            }
        }
    }
}