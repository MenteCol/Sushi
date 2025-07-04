using UnityEngine;

public class ActivarBotonesMenu : MonoBehaviour
{
    #region Variables
    [Header("Botones")]
    public GameObject botonStart;
    public GameObject botonOpciones;
    public GameObject botonRules;
    public GameObject botonCreditos;
    public GameObject botonSalir;
    #endregion

    void Start()
    {

    }

    void Update()
    {

    }

    public void ActivarBotones()
    {
        botonStart.SetActive(true);
        botonOpciones.SetActive(true);
        botonRules.SetActive(true);
        botonCreditos.SetActive(true);
        botonSalir.SetActive(true);
    }
}