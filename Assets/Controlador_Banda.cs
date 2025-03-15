using System.Collections.Generic;
using UnityEngine;

public class Controlador_Banda : MonoBehaviour
{
    [Header("Instancia Banda")]
    [SerializeField] private GameObject prefabBanda;
    [SerializeField] private Transform inicioBanda;

    [Header("Movimiento Banda")]
    public List<GameObject> bandas = new List<GameObject>();

    [SerializeField] private Transform finalBanda;
    [SerializeField] private float velocidadBanda = 5f;

    void Start()
    {
        ActualizarListaBandas();
    }

    void Update()
    {
        ActualizarListaBandas(); // Mantiene la lista sincronizada
        MoverBandas(); // Mueve las bandas en cada frame

        Debug.Log("[Controlador_Banda] Bandas activas: " + bandas.Count);
    }

    public void InstanciaBanda()
    {
        GameObject nuevaBanda = Instantiate(prefabBanda, inicioBanda.position, Quaternion.identity);
        bandas.Add(nuevaBanda); // Agregamos la nueva banda a la lista
    }

    private void MoverBandas()
    {
        for (int i = bandas.Count - 1; i >= 0; i--) // Recorremos de atrás hacia adelante para eliminar sin errores
        {
            if (bandas[i] != null)
            {
                bandas[i].transform.position = Vector3.MoveTowards(
                    bandas[i].transform.position,
                    finalBanda.position,
                    velocidadBanda * Time.deltaTime
                );

                // Si la banda llegó al destino, se elimina
                if (Vector3.Distance(bandas[i].transform.position, finalBanda.position) < 0.1f)
                {
                    Destroy(bandas[i]);
                    InstanciaBanda();
                    bandas.RemoveAt(i);
                }
            }
        }
    }

    private void ActualizarListaBandas()
    {
        // Buscar todas las bandas activas en la escena
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("Banda");

        // Limpiar la lista eliminando objetos destruidos
        bandas.RemoveAll(b => b == null);

        // Agregar nuevas bandas si aún no están en la lista
        foreach (GameObject obj in objetosConTag)
        {
            if (!bandas.Contains(obj))
            {
                bandas.Add(obj);
            }
        }
    }
}
