using UnityEngine;

// PLANTILLA BASE PARA SCRIPTS DE UNITY
// -------------------------------------
// Instrucciones y convenciones actualizadas:
// - Si el script implementa un Singleton (ej: public static NombreClase Instance { get; private set; }), debe declararse siempre de primeras y fuera de la región #region Variables.
// - [Header("Depuracion")] y la variable bool depuracion deben ir siempre de primeras en la lista de variables dentro de la región.
// - [Header("Varios")] se usa para strings, TMP, GameObjects, imágenes, etc. si no están ya separados en headers específicos, y debe ir después de "Depuracion".
// - El resto de headers deben describir el grupo de variables (ej: [Header("Botones")], [Header("Audio")], [Header("Raycast")], etc.).
// - [Header("Referencias")] solo se usa para referencias a otros scripts y debe ir siempre de últimas en la lista de variables públicas/serializadas.
// - Dentro de cada header, agrupar primero los bool, luego los int, luego los float, y después el resto de tipos (strings, objetos, etc.). Primero públicas, luego privadas visibles ([SerializeField]), y luego privadas.
// - Todos los Debug.Log, Debug.LogWarning, Debug.LogError, etc. deben estar controlados por la variable depuracion.
// - Todos los mensajes de Debug deben tener el formato: "(Nombre Objeto): el mensaje", usando $"{gameObject.name}: mensaje".
// - Solo agregar Debug.Log si ya existían en el script original, a menos que se indique lo contrario.
// - Usar #region Variables para agrupar las variables públicas y serializadas.
// - Mantener la estructura de métodos: Start, Update, y luego métodos propios.
// - Evitar usar comentarios innecesarios, el código debe o puede ser autoexplicativo.
// - No inicializar referencias a componentes con = null en la declaración, ya que es redundante.
// - Antes de finalizar un script, revisar que todas las variables declaradas estén siendo realmente utilizadas en el propio script o desde otros scripts/escenas. Eliminar o justificar las que no tengan uso.
// - El script no puede tener caracteres como tildes o ñ que puedan causar errores en otros sistemas operativos como IOS.

public class PlantillaScripts : MonoBehaviour
{
    // Ejemplo de Singleton (declarar aquí si aplica)
    // public static PlantillaScripts Instance { get; private set; }

    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Varios")] // Agrupar variables que no tienen un header específico y sin del tipo que esta en este header.
    public string ejemploString;
    public Camera camaraPrincipal;
    public GameObject ejemploGO;
    public UnityEngine.UI.Image ejemploImagen;
    public TMPro.TextMeshProUGUI ejemploTMP;

    [Header("Nombre Ejemplo Grupo Definido")]
    public bool check1;
    public int contador;
    public float tiempoCheck;

    [Header("Variables")] // En caso de no estar asignados a un grupo o header específico
    public bool bool1;
    public int int1;
    public float float1;

    [Header("Audio")]
    public string audioEjemplo = "Audio";

    [Header("Raycast")]
    public bool hacerRaycast = true;
    public int raycastCount;
    public float distanciaRaycast = 2f;

    [Header("Referencias")] // Solo para referencias a otros scripts
    public PlantillaScripts ejemploOtrosScripts;
    #endregion

    void Start()
    {
        if (depuracion)
            Debug.Log($"{gameObject.name}: Mensaje Ejemplo");
    }

    void Update()
    {

    }
}