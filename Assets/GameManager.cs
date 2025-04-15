using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("> Valores Globales")]
    [Space]
    public int puntos;    
    [Header("> Valores Control Puntos")]
    [Space]
    public float fr_llenura1;
    public float fr_llenura2;
    public float fr_llenura3;
    [Space]
    public int valorMalestarMaximo;
    public float timerEnfermo = 3;
    public float timerAcumulacionEnfermo = 6; // Valor para volver a malestar = 0.
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
    public int valorLimiteCollider = 13;
    
    [Header("------------- Referencias -------------")]
    public Controlador_Puntos controladorPuntos;
    public Controlador_Instancias controladorInstancias;   
    public Controlador_Banda controladorBanda;
    public LimpiarBasura limpiarBasura;
    public InstanciarBasura instanciarBasura;
    public CortePlatos cortePlatos;

    void Start()
    {
        
    }
        
    void Update()
    {
        puntos = controladorPuntos.puntaje;
    }
}
