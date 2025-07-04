using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controlador_PuntajeFinal : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Varios")]
    [SerializeField] private TextMeshProUGUI puntajeNum;
    [SerializeField] private TextMeshProUGUI puntajeAltoNumber;

    [Header("Variables")]
    public int puntajeAlto;

    [Header("Referencias")]
    public Controlador_Puntos controladorPuntos;
    #endregion

    void Start()
    {
        controladorPuntos = GameObject.Find("Controlador_Puntaje").GetComponent<Controlador_Puntos>();

        if (PlayerPrefs.HasKey("HighScore"))
        {
            puntajeAlto = PlayerPrefs.GetInt("HighScore");
        }
    }

    void Update()
    {
        if (controladorPuntos.puntaje > puntajeAlto)
        {
            puntajeAlto = controladorPuntos.puntaje;
            PlayerPrefs.SetInt("HighScore", puntajeAlto);
        }
    }

    public void MostrarPuntajesGO()
    {
        if (puntajeNum != null)
            puntajeNum.text = controladorPuntos.puntaje.ToString("D2");

        if (puntajeAltoNumber != null)
            puntajeAltoNumber.text = puntajeAlto.ToString("D2");
    }
}