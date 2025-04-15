using UnityEngine;

public class MenuGameOver : MonoBehaviour
{
    [Header("Depuracion")]
    public bool test;
    public KeyCode toggleGOPanel;
    [Header("Objetos")]
    public GameObject panelGameOver;

    void Start()
    {
        
    }
        
    void Update()
    {
        if (Input.GetKeyDown(toggleGOPanel))
        {

        }
    }

    public void MostrarGameOver()
    { 
    
    
    }
}
