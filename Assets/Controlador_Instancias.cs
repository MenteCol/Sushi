using UnityEngine;

public class Controlador_Instancias : MonoBehaviour
{
    [Header("Depuracion")]
    public bool test;
    public KeyCode teclaPrueba;
    //[Space]
    [Header("Variables Instancia")]
    [SerializeField] private GameObject prefabBuena;
    [SerializeField] private GameObject prefabMala;
    [SerializeField] private GameObject prefabBoost;
    [SerializeField] private Transform origenInstancia;
        
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(teclaPrueba) && test)
        {
            if (test) Debug.Log("[Controlador_Instancias] Presionando " + teclaPrueba + " en " + this.name);
            InstanciarComida();
        }
    }

    public void InstanciarComida()
    {       
        int index = Random.Range(0, 3);

        GameObject prefabInstance = null; //Necesita tener un valor.

        switch (index)
        {
            case 0:
                prefabInstance = prefabBuena;
                break;

            case 1:
                prefabInstance = prefabMala;
                break;

            case 2:
                prefabInstance = prefabBoost;
                break;

                default:
                return;
        }

        // ESTRUCTURA DE LA INSTANCIA

        GameObject nuevaComida = Instantiate(prefabInstance, origenInstancia.position, Quaternion.identity);        
    }
}
