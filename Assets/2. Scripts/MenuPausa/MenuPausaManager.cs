using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausaManager : MonoBehaviour
{
    [Header("depuracion")]
    public bool mostrarDebug = true;
    public GameObject canvasPausa;
    private bool canvasPausaOn;    
    [Header("Opciones")]
    [SerializeField] private bool ocultarCursor;
    [Header("Referencias")]
    public Controlador_Fases controladorFases;

    public void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        canvasPausaOn = canvasPausa.activeSelf;
        ActualizarEstado();
        if (mostrarDebug) Debug.Log($"[MenuPausaManager] Menu de pausa iniciado con estado: {(canvasPausaOn ? "Activado" : "Desactivado")}.");
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            canvasPausaOn = !canvasPausaOn;
            canvasPausa.SetActive(canvasPausaOn);
            controladorFases.enPausa = !controladorFases.enPausa;
            ActualizarEstado();
            if (mostrarDebug) Debug.Log($"[MenuPausaManager] Estado del menu de pausa: {(canvasPausaOn ? "Activado" : "Desactivado")}.");
        }
    }

    private void ActualizarEstado()
    {
        if (canvasPausaOn)
        {
            if (ocultarCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }            
        }
        else
        {
            if (ocultarCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }            
        }
    }

    public void Volver()
    {
        canvasPausaOn = false;
        canvasPausa.SetActive(false);
        controladorFases.enPausa = false;
        ActualizarEstado();
        if (mostrarDebug) Debug.Log("[MenuPausaManager] Menu de pausa desactivado al volver.");
    }
}
