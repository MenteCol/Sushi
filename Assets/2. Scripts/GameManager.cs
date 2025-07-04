using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("> Valores Globales")]
    public int puntos;
    public float valorReduccionTiempo;

    [Header("> Valores GameOver")]
    public int strikes;
    public int strikesMax;

    [Header("> Valores Platos")]
    public int maxPlatosLimpiar;

    [Header("* Valores Instancias Comidas")]
    public float tiempoMinInstancias;
    public float reduccionTiempoInstancia;

    [Header("* Valores Velocidad Banda")]
    public float velocidadMaxBanda;
    public float aumentoVelocidadBanda;

    [Header("> Valores Enfermo")]
    public int malestar;
    public int valorMalestarMaximo;
    public float timerEnfermo = 3;
    public float timerAcumulacionEnfermo = 6;

    [Header(">> Penalizacion Lleno")]
    public float valorReduccion;
    public float valorVelReduccion;

    [Header("> Valores Control Instancias")]
    public float timerInstanciaComida;
    public int fase = 1;

    [Header("> Valores Limpiar Basura")]
    public float tiempoLimpiarBasura;

    [Header("> Valores Instanciar Basura")]
    public int valorLimiteLlena = 10;

    [Header("Referencias")]
    public Controlador_Puntos controladorPuntos;
    public Controlador_Instancias controladorInstancias;
    public Controlador_Banda controladorBanda;
    public LimpiarBasura limpiarBasura;
    public InstanciarBasura instanciarBasura;
    public CortePlatos cortePlatos;
    public GameOver_Controller gameOverController;
    #endregion

    void Start()
    {

    }

    void Update()
    {
        puntos = controladorPuntos.puntaje;
        strikes = gameOverController.strikes;
    }

    public void ReducirVelocidad()
    {
        float valorEntradaLlennura = controladorPuntos.llenura;
        Controlador_EmotesT.Instance.ReproducirEmoji("Boost");

        if (depuracion)
            Debug.Log($"{gameObject.name}: Metodo ReducirVelcidad");

        Time.timeScale = valorReduccionTiempo;
        controladorPuntos.malestar = 0;
        controladorPuntos.llenura = valorEntradaLlennura * 0.3f;
        instanciarBasura.BorrarBasura();
        controladorInstancias.boostFlag = false;

        Invoke(nameof(ReestablecerVelocidad), 0.6f);
    }

    public void ReestablecerVelocidad()
    {
        if (depuracion)
            Debug.Log($"{gameObject.name}: Metodo ReestablecerVelocidad");

        Time.timeScale = 1f;
    }
}