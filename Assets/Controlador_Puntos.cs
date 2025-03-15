using System.Collections.Generic;
using UnityEngine;

public class Controlador_Puntos : MonoBehaviour
{
    public List<ClickObjetosPuntos> objetosComida = new List<ClickObjetosPuntos>();

    void Start()
    {        
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("Comida");
             
        foreach (GameObject obj in objetosConTag)
        {
            ClickObjetosPuntos componente = obj.GetComponent<ClickObjetosPuntos>();

            if (componente != null)
            {
                objetosComida.Add(componente);
            }
            else
            {
                Debug.LogWarning("El objeto " + obj.name + " con tag 'Comida' no tiene el componente ClickObjetosPuntos.");
            }




        }

        // Mostrar cuántos objetos se encontraron
        Debug.Log("Se encontraron " + objetosComida.Count + " objetos con el tag 'Comida'.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
