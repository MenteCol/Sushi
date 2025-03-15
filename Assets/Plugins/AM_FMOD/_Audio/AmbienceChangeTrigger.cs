using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Parameter Change")]

    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("CambiandoAmbiente");
            AudioManager.instance.SetAmbienceParameter(parameterName, parameterValue);        
        }
    }

}
