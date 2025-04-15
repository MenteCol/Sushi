using System.Collections.Generic;
using UnityEngine;

public class InstanciarBasura : MonoBehaviour
{
    public GameManager gameManager;

    public List<Transform> puntoBasura = new List<Transform>();
    public int contadorBasura = 0;
    public Controlador_Instancias controladorInstancias;
    public List<GameObject> objetosBasura = new List<GameObject>();

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();
    }

    void Update()
    {
        if (contadorBasura >= gameManager.valorLimiteLlena)
        {
            controladorInstancias.basuraLlena = true;
        }
        else
        {
            controladorInstancias.basuraLlena = false;
        }

        if (contadorBasura >= gameManager.valorLimiteCollider)
        { 
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
        else
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
        }

    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Comida"))
        {
            int indiceAleatorio = Random.Range(0, puntoBasura.Count);
            contadorBasura++;
            other.transform.localScale = other.transform.localScale * 0.8f;
            // Destroy(other.gameObject);
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
    }
}
