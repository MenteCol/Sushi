using UnityEngine;

public class ClickObjetosPuntos : MonoBehaviour
{
    [SerializeField] private int ID;

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Dando click en: " + this.name);
    }
}
