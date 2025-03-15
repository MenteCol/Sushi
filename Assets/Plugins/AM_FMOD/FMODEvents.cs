using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FMODEvents : MonoBehaviour
{
    [System.Serializable]
    public class FMODEvent
    {
        public string nombreEvento;
        public EventReference eventReference;
    }

    private bool mostrarDebug;

    [SerializeField] private List<FMODEvent> musica = new List<FMODEvent>();
    [SerializeField] private List<FMODEvent> sfx = new List<FMODEvent>();
    [SerializeField] private List<FMODEvent> ambiente = new List<FMODEvent>();

    public static FMODEvents instance { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if(mostrarDebug) Debug.Log("FMODEvents Test de Iicio en el inspecto.");
        }
        else
        {
            if (mostrarDebug) Debug.LogWarning("FMODEvents inicializado.");
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    public static void BuscarInstanciaEnEditor()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.update += () =>
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<FMODEvents>();
                    if (instance != null)
                    {
                        // Debug.Log("FMODEvents inicializado en el Editor.");
                        instance.RecargarEventosEnEditor();
                    }
                }
            };
        }
    }
#endif

    public void RecargarEventosEnEditor()
    {
        if (musica == null) musica = new List<FMODEvent>();
        if (sfx == null) sfx = new List<FMODEvent>();
        if (ambiente == null) ambiente = new List<FMODEvent>();

        if (mostrarDebug) Debug.Log($"Eventos recargados: {musica.Count} música, {sfx.Count} SFX, {ambiente.Count} ambiente.");
    }

    public static Dictionary<string, List<string>> ObtenerConfiguracionMetodos()
    {
        return new Dictionary<string, List<string>>
        {
            { "MusicaFondo", new List<string> { "startToggle" } },
            { "SonidoAmbiental", new List<string>() { "startToggle" } },
            { "SonidoAmbiental3D", new List<string>() { "startToggle" , "transformField" } },
            { "OneShot", new List<string> { } },
            { "OneShot3D", new List<string> { "transformField" } },
            { "TimeLine", new List<string> { "startToggle" } },
            { "TimeLine3D", new List<string> { "startToggle" , "transformField" } }
            // { "Emitter", new List<string> { "transformField" } }
        };
    }

    public EventReference GetEventReference(string eventName)
    {
        foreach (var musicaEvent in musica)
            if (musicaEvent.nombreEvento == eventName) return musicaEvent.eventReference;

        foreach (var sfxEvent in sfx)
            if (sfxEvent.nombreEvento == eventName) return sfxEvent.eventReference;

        foreach (var ambienceEvent in ambiente)
            if (ambienceEvent.nombreEvento == eventName) return ambienceEvent.eventReference;

        Debug.LogError($"Evento no encontrado: {eventName}");
        return default;
    }

    public List<string> ObtenerTodosLosEventos()
    {
        if (musica == null || sfx == null || ambiente == null)
        {
            Debug.LogWarning("Listas de eventos estaban vacías. Recargando eventos...");
            RecargarEventosEnEditor();
        }

        List<string> eventos = new List<string>();
        eventos.AddRange(musica.ConvertAll(e => e.nombreEvento));
        eventos.AddRange(sfx.ConvertAll(e => e.nombreEvento));
        eventos.AddRange(ambiente.ConvertAll(e => e.nombreEvento));
        return eventos;
    }

    public void EventMusic(string eventReference, bool start = true)
    {
        AudioManager.instance.InitializeMusic(GetEventReference(eventReference), start);
    }
    public void EventAmbient(string eventReference, bool reproducir, Transform transform = null)
    {
        AudioManager.instance.InitializeAmbienceEvent(GetEventReference(eventReference), reproducir, transform);
    }
    public void EventPlayOneShot(string eventReference, GameObject vector3 = null)
    {
        AudioManager.instance.PlayOneShot(GetEventReference(eventReference), vector3);
    }
    public void EventTimeline(string eventReference, Transform transform, bool start, bool fade = true)
    {
        AudioManager.instance.HandleEvent(GetEventReference(eventReference), transform, start, true);
    }
    public void EventEmitter(string eventReference, GameObject emiter)
    {
        AudioManager.instance.InitializeEventEmitter(GetEventReference(eventReference), emiter);
    }
}
