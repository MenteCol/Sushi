using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("> Valores Globales")]
    [Space]
    public int puntos;
    public float valorReduccionTiempo;
    [Header("> Valores GameOver")]
    public int strikes;
    public int strikesMax;
    [Header("> Valores Platos")]
    public int maxPlatosLimpiar;
    //public float tiempoHambre_i;
    //public float tiempoHambre;
    [Header("* Valores Instancias Comidas")]
    [Space]
    public float tiempoMinInstancias;
    public float reduccionTiempoInstancia;
    [Header("* Valores Velocidad Banda")]
    public float velocidadMaxBanda;
    public float aumentoVelocidadBanda;
    [Header("> Valores Control Puntos")]
    [Space]
    public float fr_llenura1;
    public float fr_llenura2;
    public float fr_llenura3;
    [Header("> Valores Enfermo")]    
    [Space]
    public int malestar;
    public int valorMalestarMaximo;
    public float timerEnfermo = 3;
    public float timerAcumulacionEnfermo = 6; // Valor para volver a malestar = 0.
    [Header(">> Penalizacion Lleno")]
    [Space]
    public float valorReduccion;
    public float valorVelReduccion;
    [Header("> Valores Control Instancias")]
    [Space]
    public float timerInstanciaComida;
    public int fase = 1;
    [Header("> Valores Limpiar Basura")]
    [Space]
    public float tiempoLimpiarBasura;
    [Header("> Valores Instanciar Basura")]
    [Space]
    public int valorLimiteLlena = 10;    
    
    [Header("------------- Referencias -------------")]
    public Controlador_Puntos controladorPuntos;
    public Controlador_Instancias controladorInstancias;   
    public Controlador_Banda controladorBanda;
    public LimpiarBasura limpiarBasura;
    public InstanciarBasura instanciarBasura;
    public CortePlatos cortePlatos;
    public GameOver_Controller gameOverController;

    void Start()
    {
        
    }
        
    void Update()
    {
        puntos = controladorPuntos.puntaje;
        strikes = gameOverController.strikes;

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ReducirVelocidad();
        //}

        //if (Input.GetKeyUp(KeyCode.N))
        //{
        //    //ReestablecerVelocidad();
        //}
    }

    public void ReducirVelocidad()
    {
        float valorEntradaLlennura = controladorPuntos.llenura;

        Debug.Log("[GameManager] Metodo ReducirVelcidad");        

        Time.timeScale = valorReduccionTiempo;
        controladorPuntos.malestar = 0;
        controladorPuntos.llenura =valorEntradaLlennura * 0.3f;
        instanciarBasura.BorrarBasura();

        Invoke(nameof(ReestablecerVelocidad), 0.6f);
    }
    public void ReestablecerVelocidad()
    {
        Debug.Log("[GameManager] Metodo ReestablecerVelocidad");

        Time.timeScale = 1f;
    }
}
