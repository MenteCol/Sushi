using UnityEngine;

public class MenuGameOver : MonoBehaviour
{
    [Header("Depuracion")]
    public bool test;        
    [Header("Variables")]
    public bool esGameOver = false;
    public bool flagPuntaje = false;
    [Header("Objetos")]
    public GameObject panelGameOver;
    [Header("Referencias")]
    public Controlador_Fases controladorFases;
    public Controlador_PuntajeFinal puntajeFinal;

    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        puntajeFinal = GameObject.Find("Controlador_Puntaje").GetComponent<Controlador_PuntajeFinal>();

        if (!test && panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void MostrarGameOver()
    {
        if (!esGameOver)
        {
            AlternarPanelGameOver();
            controladorFases.enPausa = true;

            if (!flagPuntaje)
            { 
                puntajeFinal.MostrarPuntajesGO();
                Debug.Log("Mostrando Puntaje Final");
                flagPuntaje  = true;            
            }
        }        
    }
        
    private void AlternarPanelGameOver()
    {        
        if (panelGameOver != null)
        {            
            panelGameOver.SetActive(!panelGameOver.activeSelf);
            esGameOver = true;
        }
        else
        {
            Debug.LogError("El GameObject panelGameOver no está asignado en el Inspector.");
        }
    }
    
}
