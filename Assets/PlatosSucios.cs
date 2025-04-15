using Unity.Cinemachine;
using UnityEngine;

public class PlatosSucios : MonoBehaviour
{
    public Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piso") || collision.gameObject.CompareTag("Plato"))
        {             
            rigidBody.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piso") || collision.gameObject.CompareTag("Plato"))
        {
            rigidBody.isKinematic = false;
        }
    }

}
