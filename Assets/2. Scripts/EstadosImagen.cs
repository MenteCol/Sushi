using UnityEngine;
using UnityEngine.UI;
using System;

public class EstadosImagen : MonoBehaviour
{
    [Serializable]
    public struct Estado
    {
        public string nombre;
        public float posicionX;
        public float posicionY;
        public float ancho;
        public float alto;
        public Sprite sprite;
    }

    public Estado[] estados = new Estado[4];
    public int estadoActual = 0;

    private RectTransform rectTransform;
    private Image image;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        AplicarEstado(estadoActual);
    }
    
    public void CambiarEstado(int nuevoEstado)
    {
        if (nuevoEstado >= 0 && nuevoEstado < estados.Length)
        {
            estadoActual = nuevoEstado;
            AplicarEstado(estadoActual);
        }
    }

    private void AplicarEstado(int indice)
    {
        Estado estado = estados[indice];
        rectTransform.anchoredPosition = new Vector2(estado.posicionX, estado.posicionY);
        rectTransform.sizeDelta = new Vector2(estado.ancho, estado.alto);
        image.sprite = estado.sprite;
    }
    
    public void ActualizarEstadoActual()
    {
        AplicarEstado(estadoActual);
    }
}
