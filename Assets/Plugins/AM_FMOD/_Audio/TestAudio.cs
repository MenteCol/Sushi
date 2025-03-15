using UnityEngine;
using System.Collections.Generic;

public class TestAudio : MonoBehaviour
{
    [System.Serializable]
    public class PruebaAudio
    {
        public string nombreEvento; // Nombre del evento o prueba
        public KeyCode teclaPrueba; // Tecla asociada a la prueba
    }

    [Header("Pruebas de Audio")]
    [Tooltip("Para Usar el audio te llevas esta linea de codigo AudioImp.Instance.Reproducir(nombreEvento);")]
    public List<PruebaAudio> pruebasAudio = new List<PruebaAudio>(); // Lista de pruebas de audio

    [Header("Evento Inicial")]
    public string nombreReproduccionInicial; // Reproducción inicial opcional

    private void Start()
    {
        if (!string.IsNullOrEmpty(nombreReproduccionInicial))
        {
            ReproducirEvento(nombreReproduccionInicial);
        }
    }

    private void Update()
    {
        // Recorremos la lista de pruebas y verificamos si se presiona alguna tecla
        foreach (var prueba in pruebasAudio)
        {
            if (Input.GetKeyDown(prueba.teclaPrueba))
            {
                ReproducirEvento(prueba.nombreEvento);
                Debug.Log($"Reproduciendo evento: Para Usar el audio te llevas esta linea de codigo 'AudioImp.Instance.Reproducir(comillas "+prueba.nombreEvento+" comillas);'");
            }
        }
    }

    public void ReproducirEvento(string nombreEvento)
    {
        // Lógica para reproducir audio desde AudioImp.Instance
        if (AudioImp.Instance != null)
        {
            AudioImp.Instance.Reproducir(nombreEvento);
        }
        else
        {
            Debug.LogWarning($"AudioImp.Instance no está configurado. No se puede reproducir: {nombreEvento}");
        }
    }

    public void AgregarPrueba(string nombreEvento, KeyCode tecla)
    {
        // Agrega dinámicamente una nueva prueba a la lista
        PruebaAudio nuevaPrueba = new PruebaAudio
        {
            nombreEvento = nombreEvento,
            teclaPrueba = tecla
        };
        pruebasAudio.Add(nuevaPrueba);
        Debug.Log($"Prueba agregada: {nombreEvento} con tecla: {tecla}");
    }

    public void EliminarPrueba(string nombreEvento)
    {
        // Elimina una prueba de la lista según el nombre del evento
        PruebaAudio pruebaAEliminar = pruebasAudio.Find(p => p.nombreEvento == nombreEvento);
        if (pruebaAEliminar != null)
        {
            pruebasAudio.Remove(pruebaAEliminar);
            Debug.Log($"Prueba eliminada: {nombreEvento}");
        }
        else
        {
            Debug.LogWarning($"No se encontró la prueba con el nombre: {nombreEvento}");
        }
    }
}
