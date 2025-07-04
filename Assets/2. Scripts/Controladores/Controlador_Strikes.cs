using System.Collections.Generic;
using UnityEngine;

public class Controlador_Strikes : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Varios")]
    public RectTransform padreMarcas;
    public GameObject prefabMarca;

    [Header("Referencias")]
    public GameManager gameManager;

    // Privadas
    private List<GameObject> marcasList;
    private int strikesPrevio = -1;
    #endregion

    void Awake()
    {
        marcasList = new List<GameObject>();
    }

    private void Start()
    {
        InstanciasMarcas();
        ActualizarMarcas(gameManager.strikes);
        strikesPrevio = gameManager.strikes;
    }

    private void Update()
    {
        int current = gameManager.strikes;

        if (current != strikesPrevio)
        {
            ActualizarMarcas(current);
            strikesPrevio = current;
        }
    }

    public void InstanciasMarcas()
    {
        for (int i = 0; i < gameManager.strikesMax; i++)
        {
            GameObject nuevaMarca = Instantiate(prefabMarca, padreMarcas);
            marcasList.Add(nuevaMarca);
        }
    }

    private void ActualizarMarcas(int currentStrikes)
    {
        for (int i = 0; i < marcasList.Count; i++)
        {
            var marcado = marcasList[i].GetComponent<MarcarStrikes>();
            marcado.esCheck = (i < currentStrikes);
        }
    }
}