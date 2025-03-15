using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcionesGraficas : MonoBehaviour
{
#region Variables

    [Header("Depuración")]
    public bool mostrarDebug;
    [Header("Objetos Opciones")]
    public TMP_Dropdown dropdownGraficos;
    public TMP_Dropdown dropdownResolucion;
    public Toggle togglePantallaCompleta;
    
    private List<Resolution> resolucionesPersonalizadas = new List<Resolution>
    {
        new Resolution { width = 800, height = 600 },
        new Resolution { width = 1024, height = 800 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1920, height = 1080 }
    };

    private int estaPantallaCompleta;

#endregion

    void Start()
    {
        #region Resolución de Ventana

        dropdownResolucion.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        // Rellenar opciones de resolución
        for (int i = 0; i < resolucionesPersonalizadas.Count; i++)
        {
            string option = resolucionesPersonalizadas[i].width + " X " + resolucionesPersonalizadas[i].height;
            options.Add(option);
        }

        dropdownResolucion.AddOptions(options);
        dropdownResolucion.value = currentResolutionIndex;
        dropdownResolucion.RefreshShownValue();

        #endregion

        if (PlayerPrefs.HasKey("valorGraficos") || PlayerPrefs.HasKey("valorPantallaCompleta") || PlayerPrefs.HasKey("valorResolucion"))
        {
            LoadGraficos();
        }
        else
        {
            CambiarCalidadGraficos();
        }

        MostrarLog("Valor inicial de resolución: " + dropdownResolucion.value);

        for (int i = 0; i < resolucionesPersonalizadas.Count; i++)
        {
            if (dropdownResolucion.value == i)
            {
                dropdownResolucion.value = i;
                Screen.SetResolution(resolucionesPersonalizadas[i].width, resolucionesPersonalizadas[i].height, Screen.fullScreen);
            }
        }

        #region Pantalla Completa

        if (estaPantallaCompleta == 1)
        {
            togglePantallaCompleta.isOn = true;
            Screen.fullScreen = true;
        }
        if (estaPantallaCompleta == 0)
        {
            togglePantallaCompleta.isOn = false;
            Screen.fullScreen = false;
        }

        #endregion
    }

    public void CambiarCalidadGraficos()
    {
        QualitySettings.SetQualityLevel(dropdownGraficos.value);
        PlayerPrefs.SetInt("valorGraficos", dropdownGraficos.value);
        MostrarLog("Los gráficos cambiaron a: " + dropdownGraficos.value);
    }

    public void cambiarPantallaCompleta(bool esPantallaCompleta)
    {
        Screen.fullScreen = esPantallaCompleta;

        if (esPantallaCompleta)
        {
            estaPantallaCompleta = 1;
        }
        else
        {
            estaPantallaCompleta = 0;
        }

        PlayerPrefs.SetInt("valorPantallaCompleta", estaPantallaCompleta);

        MostrarLog("Cambiando Pantalla Completa a: " + esPantallaCompleta + " y guardo el valor: " + estaPantallaCompleta);
    }

    public void CambiarResolucion(int resolucionIndex)
    {
        Resolution resolution = resolucionesPersonalizadas[resolucionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("valorResolucion", dropdownResolucion.value);

        MostrarLog("Cambiando resolución de ventana a: " + resolution.width + " X " + resolution.height);
        MostrarLog("Guardando valor del índice: " + dropdownResolucion.value);
    }

    private void LoadGraficos()
    {
        dropdownGraficos.value = PlayerPrefs.GetInt("valorGraficos");
        estaPantallaCompleta = PlayerPrefs.GetInt("valorPantallaCompleta");

        dropdownResolucion.value = PlayerPrefs.GetInt("valorResolucion");

        MostrarLog("Cargando Valor: " + dropdownResolucion.value);

        CambiarCalidadGraficos();
    }

    private void MostrarLog(string mensaje)
    {
        if (mostrarDebug)
        {
            Debug.Log($"[OpcionesGraficas]: {mensaje}");
        }
    }
}
