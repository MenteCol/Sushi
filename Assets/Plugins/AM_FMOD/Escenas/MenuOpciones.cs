using UnityEngine;

public class MenuOpciones : MonoBehaviour
{
#region Variables

    [Header("Depuración")]
    public bool mostrarDebug;

    [Header("Paneles de Opciones")]
    public GameObject panelOpciones;
    public GameObject panelControles;
    public GameObject panelSonido;
    public GameObject panelGraficos;

#endregion

    void Start()
    {
        MostrarLog("Menú de opciones cargado.");
    }

    void Update()
    {

    }

    public void MostrarControles()
    {
        panelOpciones.SetActive(false);
        panelControles.SetActive(true);
        panelSonido.SetActive(false);
        panelGraficos.SetActive(false);
        MostrarLog("Se mostró el panel de controles.");
    }

    public void MostrarSonido()
    {
        panelOpciones.SetActive(false);
        panelControles.SetActive(false);
        panelSonido.SetActive(true);
        panelGraficos.SetActive(false);
        MostrarLog("Se mostró el panel de sonido.");
    }

    public void MostrarGraficos()
    {
        panelOpciones.SetActive(false);
        panelControles.SetActive(false);
        panelSonido.SetActive(false);
        panelGraficos.SetActive(true);
        MostrarLog("Se mostró el panel de gráficos.");
    }

    public void Volver()
    {
        panelOpciones.SetActive(true);
        panelControles.SetActive(false);
        panelSonido.SetActive(false);
        panelGraficos.SetActive(false);
    }

    public void Salir()
    {
        MostrarLog("Saliendo del juego...");
        Application.Quit();
    }

    private void MostrarLog(string mensaje)
    {
        if (mostrarDebug)
        {
            Debug.Log($"[MenuOpciones]: {mensaje}");
        }
    }
}
