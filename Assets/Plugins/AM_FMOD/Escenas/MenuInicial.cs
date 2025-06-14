using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
#region Variables

    [Header("Depuración")]
    public bool mostrarDebug;

    [Header("Objetos Menu Principal")]
    public GameObject menuInicial;
    public GameObject menuOpciones;
    public GameObject menuCreditos;
    public GameObject menuRules;

    [Header("Objetos Opciones")]
    public GameObject menuControles;
    public GameObject menuSonido;
    public GameObject menuGraficos;

    [Header("Prov - Load Sliders")]
    [SerializeField] private AudioManager volumeLoad;

    #endregion

    void Start()
    {
        MostrarLog("Menu inicial cargado.");
        volumeLoad.masterVolume = PlayerPrefs.GetFloat("volumenMaster");
        volumeLoad.musicVolume = PlayerPrefs.GetFloat("volumenMusica");
        volumeLoad.SFXVolume = PlayerPrefs.GetFloat("volumenSFX");
    }

    public void Jugar(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
        MostrarLog("Entrando al juego...");
    }

    public void AbrirMenuOpciones()
    {
        menuInicial.SetActive(false);
        menuOpciones.SetActive(true);
        menuCreditos.SetActive(false);
        menuRules.SetActive(false);
        MostrarLog("Se abrió el menú de opciones.");
    }

    public void AbrirMenuCreditos()
    {
        menuInicial.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(true);
        menuRules.SetActive(false);
        MostrarLog("Se abrió el menú de créditos.");
    }

    public void AbrirMenuDaRules()
    {
        menuInicial.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuRules.SetActive(true);
        MostrarLog("Se abrió el menú de Las Rules.");
    }

    public void AbrirMenuInicial() // Volver
    {
        menuInicial.SetActive(true);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuRules.SetActive(false);
        //        
        MostrarLog("Se abrió el menú inicial.");
    }

    public void Salir()
    {
        MostrarLog("Saliendo del juego...");
        Application.Quit();
    }

    private void MostrarLog(string mensaje)
    {
        if (mostrarDebug)
        {
            Debug.Log($"[MenuInicial]: {mensaje}");
        }
    }
}
