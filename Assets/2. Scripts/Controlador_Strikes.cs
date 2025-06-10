using System.Collections.Generic;
using UnityEngine;

public class Controlador_Strikes : MonoBehaviour
{
    public RectTransform padreMarcas;
    public GameObject prefabMarca;
    public GameManager gameManager;

    private List<GameObject> marcasList;
    private int strikesPrevio = -1; // Valor inicial que no coincida con strikes real

    void Awake()
    {
        marcasList = new List<GameObject>();
    }

    private void Start()
    {
        InstanciasMarcas();
        ActualizarMarcas(gameManager.strikes); // Inicializar marcas correctamente
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
