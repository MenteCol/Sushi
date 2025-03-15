using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Evento
{
    public string NombreElegido;
    public string Metodo;
    public string EventoFMOD;
    public GameObject gameobject;
    public bool start;  // para decidir si es para dar Start
    public string Tipo; // Pa mostrar las listas separadas
}

public class AudioImp : MonoBehaviour
{    
    public static AudioImp Instance { get; private set; }
     
    public List<Evento> Eventos = new List<Evento>();
    public List<Evento> Musica = new List<Evento>();
    public List<Evento> SFX = new List<Evento>();
    public List<Evento> Ambiente = new List<Evento>();

    public static List<string> TiposEvento = new List<string> { "Música", "Ambiente", "Personajes", "NPCS", "Enemigos", "Jefes", "SFX" };

    public bool mostrarLog = false;

    private void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (mostrarLog) Debug.LogWarning("Ya existe una instancia de AudioImp. Destruyendo el objeto duplicado.");
            Destroy(gameObject);
            return;
        }

        // Por si manejamos Escenarios
        // DontDestroyOnLoad(gameObject);
    }

    public void AgregarEvento(Evento nuevoEvento)
    {
        Eventos.Add(nuevoEvento);

        switch (nuevoEvento.Tipo)
        {
            case "Música":
                Musica.Add(nuevoEvento);
                break;
            case "SFX":
                SFX.Add(nuevoEvento);
                break;
            case "Ambiente":
                Ambiente.Add(nuevoEvento);
                break;
        }
    }


    public void Reproducir(string nombreElegido)
    {
        // Buscar el evento por su nombre
        Evento evento = Eventos.Find(e => e.NombreElegido == nombreElegido);

        if (evento == null)
        {
            if (mostrarLog) Debug.LogWarning($"No se encontró un evento con el nombre: {nombreElegido}");
            return;
        }

        string metodoSeleccionado = evento.Metodo;
        string nombreEventoFMOD = evento.EventoFMOD;
        GameObject eventoTransform = evento.gameobject; // Transform configurado (puede ser null)
        bool decisionReprodicir = evento.start;

        if (mostrarLog) Debug.Log($"Reproduciendo '{nombreElegido}' usando el método '{metodoSeleccionado}' con evento '{nombreEventoFMOD}'.");
                
        switch (metodoSeleccionado)
        {
            case "MusicaFondo":
                FMODEvents.instance.EventMusic(nombreEventoFMOD,decisionReprodicir);                
                break;

            case "SonidoAmbiental":
                FMODEvents.instance.EventAmbient(nombreEventoFMOD, decisionReprodicir);
                break;

            case "SonidoAmbiental3D":
                FMODEvents.instance.EventAmbient(nombreEventoFMOD, decisionReprodicir, eventoTransform.transform);
                break;

            case "OneShot":
                FMODEvents.instance.EventPlayOneShot(nombreEventoFMOD);
                break;

            case "OneShot3D":
                FMODEvents.instance.EventPlayOneShot(nombreEventoFMOD, eventoTransform);
                break;

            case "TimeLine":
                FMODEvents.instance.EventTimeline(nombreEventoFMOD, null, decisionReprodicir);
                break;

            case "TimeLine3D":
                FMODEvents.instance.EventTimeline(nombreEventoFMOD, eventoTransform.transform, decisionReprodicir);
                break;

            case "Emitter":
                FMODEvents.instance.EventEmitter(nombreEventoFMOD, eventoTransform);
                break;

            default:
                Debug.LogWarning($"Método '{metodoSeleccionado}' no implementado.");
                break;
        }
    }

    public List<Evento> ObtenerEventosPorTipo(string tipo)
    {
        return Eventos.FindAll(e => e.Tipo == tipo);
    }

}
