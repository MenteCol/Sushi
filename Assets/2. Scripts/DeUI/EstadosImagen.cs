using UnityEngine;
using UnityEngine.UI;
using System;

public class EstadosImagen : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = true;

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

    [Header("Referencias")]
    private RectTransform rectTransform;
    private Image image;
    #endregion

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        AplicarEstado(estadoActual);
    }

    public void CambiarEstado(int nuevoEstado)
    {
        if (depuracion)
            Debug.Log($"{gameObject.name}: Cambiando a estado {nuevoEstado}");

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
}