using UnityEngine;

public class ObjetoPosicionSonido : MonoBehaviour
{
    [SerializeField] private Transform objetoSonido;
    [SerializeField] private bool isTest;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            if (mainCamera != null)
            {
                audioManager.jugador = mainCamera;
            }
            else if (mainCamera == null)
            {
                Debug.LogWarning("No objeto con tag 'MainCamera'. Asignando Vector Zero al sonido");
                audioManager.jugador = transform.gameObject;
            }
        }
    }

    void Update()
    {
        if (isTest)
        {
            transform.position = objetoSonido.position;
        }        
    }
}
