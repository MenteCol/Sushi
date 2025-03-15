using FMODUnity; // Asegúrate de tener este namespace
using FMOD.Studio;
using UnityEngine;

public class SnapshotController : MonoBehaviour
{
    // Nombre del parámetro global configurado en FMOD
    private const string GlobalParameterName = "TestAgua"; // Asegúrate de que coincide con tu configuración en FMOD

    void Start()
    {
        // Inicialmente puedes definir un estado, como "afuera"
        // RuntimeManager.StudioSystem.setParameterByName(GlobalParameterName, 0f); // 0 para "afuera"
    }

    void Update()
    {
        // Cambiar al estado "adentro" al presionar "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
           // Debug.Log("Cambiando a adentro");
           // RuntimeManager.StudioSystem.setParameterByName(GlobalParameterName, 1f); // 1 para "adentro"
        }

        // Cambiar al estado "afuera" al presionar "R"
        if (Input.GetKeyDown(KeyCode.R))
        {
           //  Debug.Log("Cambiando a afuera");
           //  RuntimeManager.StudioSystem.setParameterByName(GlobalParameterName, 0f); // 0 para "afuera"
        }
    }
}
