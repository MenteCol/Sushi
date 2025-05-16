using UnityEngine;
using UnityEngine.UI;

public class Controlador_AnimEstomago : MonoBehaviour
{
    public Image imagenEstomago;
    public Animator animatorImagenEstomago;
    [Header("Referencias")]
    public Controlador_Puntos controladorPuntos;
    public EstadosImagen imagenFondoEstomago;

    private string estadoActual = "";

    void Start()
    {
        controladorPuntos = GameObject.Find("Controlador_Puntaje").GetComponent<Controlador_Puntos>();
        CambiarAnimacion("Neutral");
    }

    void Update()
    {        
        if (controladorPuntos.estaVomitando)
        {
            CambiarAnimacion("Vomitando");
            imagenFondoEstomago.CambiarEstado(2);
        }
        else if (controladorPuntos.malestar > 0 && controladorPuntos.malestar < 3)
        {
            CambiarAnimacion("Aguantando");
            imagenFondoEstomago.CambiarEstado(2);
        }
        else if (controladorPuntos.llenura > 0.8f)
        {
            CambiarAnimacion("Lleno");
            imagenFondoEstomago.CambiarEstado(3);
        }
        else if (controladorPuntos.llenura < 0.1f)
        {
            CambiarAnimacion("Hambre");
            imagenFondoEstomago.CambiarEstado(1);
        }
        else
        {
            CambiarAnimacion("Neutral");
            imagenFondoEstomago.CambiarEstado(0);
        }
    }
    void CambiarAnimacion(string nuevoEstado)
    {
        if (estadoActual == nuevoEstado) return;
        animatorImagenEstomago.Play(nuevoEstado);
        estadoActual = nuevoEstado;
        Debug.Log("[AnimEstomago: Reproducir Animacion " + nuevoEstado + "]");
    }
}
