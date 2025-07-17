using UnityEngine;

public class AdoptandoSushis : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Comida"))
        {            
          collision.transform.SetParent(transform);
          Debug.Log("Sushi adoptado: " + collision.gameObject.name);
        }
    }


}
