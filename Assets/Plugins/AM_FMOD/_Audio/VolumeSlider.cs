using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,

        MUSIC,

        AMBIENCE,

        SFX,

    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("volumenMusica"))
        {
            LoadVolumen();
        }
        else
        {
            OnSliderValueChanged();
        }
    }


    private void Update()
    {
        switch (volumeType)
        { 
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
               break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.instance.musicVolume;
              break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = AudioManager.instance.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.instance.SFXVolume;
                break;

                default:
                Debug.LogWarning("Volumen no permitido: " + volumeType);
                    break;
        
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("volumenMaster", volumeSlider.value);
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("volumenMusica", volumeSlider.value);
                break;
            case VolumeType.AMBIENCE:
                AudioManager.instance.musicVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("volumenMusica", volumeSlider.value);
                break;
            case VolumeType.SFX:
                AudioManager.instance.SFXVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("volumenSFX", volumeSlider.value);
                break;

            default:
                Debug.LogWarning("Volumen no permitido: " + volumeType);
                    break;
        }
    }

    public void LoadVolumen()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = PlayerPrefs.GetFloat("volumenMaster");
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = PlayerPrefs.GetFloat("volumenMusica");
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = PlayerPrefs.GetFloat("volumenMusica");
                break;
            case VolumeType.SFX:
                volumeSlider.value = PlayerPrefs.GetFloat("volumenSFX");
                break;

            default:
                Debug.LogWarning("Volumen no permitido: " + volumeType);
                break;
        }

        OnSliderValueChanged();


    }


}
