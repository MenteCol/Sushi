using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumenManager : MonoBehaviour
{
    public AudioMixer mixer;

    [Header("Control Volumen")]
    public Slider sliderMaster;
    public Slider sliderMusica;
    public Slider sliderSfx;    

    private void Start()
    {
        if (PlayerPrefs.HasKey("volumenMusica"))
        {
            LoadVolumen();
        }
        else
        {
            CambiarVolumenMaster();
            CambiarVolumenMusica();
            CambiarVolumenSfx();
        }        
    }

    public void CambiarVolumenMaster()
    {
        float volume = sliderMaster.value;
        mixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumenMaster", volume);
    }


    public void CambiarVolumenMusica()
    { 
        float volume = sliderMusica.value;
        mixer.SetFloat("musica", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("volumenMusica", volume);        
    }

    public void CambiarVolumenSfx()
    {
        float volume = sliderSfx.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumenSfx", volume);
    }


    private void LoadVolumen()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("volumenMaster");
        sliderMusica.value = PlayerPrefs.GetFloat("volumenMusica");
        sliderSfx.value = PlayerPrefs.GetFloat("volumenSfx");        

        CambiarVolumenMaster();
        CambiarVolumenMusica();
        CambiarVolumenSfx();
    }

}

#region Referencias
/* Codigo extraido de : https://www.youtube.com/watch?v=G-JUp8AMEx0&t=36s
*/
#endregion
