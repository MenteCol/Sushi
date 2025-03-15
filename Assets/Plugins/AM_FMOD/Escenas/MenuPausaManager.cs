using UnityEngine;

public class MenuPausaManager : MonoBehaviour
{
    #region Variables

    [Header("depuracion")]
    public bool mostrarDebug = true;
    [Space]
    public GameObject canvasPausa;
    private bool canvasPausaOn;
    [Header("Opciones")]
    [SerializeField] private bool ocultarCursor;

    #endregion

    void Start()
    {
        canvasPausaOn = canvasPausa.activeSelf;
        ActualizarEstado();
        if (mostrarDebug) Debug.Log($"[MenuPausaManager] Menu de pausa iniciado con estado: {(canvasPausaOn ? "Activado" : "Desactivado")}.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvasPausaOn = !canvasPausaOn;
            canvasPausa.SetActive(canvasPausaOn);
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
            //ControladorScripts.instance.MovimientoJugador(false);            
            //if (mostrarDebug) Debug.Log("[MenuPausaManager] Cursor desbloqueado, jugador detenido.");
        }
        else
        {
            if (ocultarCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            //ControladorScripts.instance.MovimientoJugador(true);            
            //if (mostrarDebug) Debug.Log("[MenuPausaManager] Cursor bloqueado, jugador habilitado.");
        }
    }

    public void Volver()
    {
        canvasPausaOn = false;
        canvasPausa.SetActive(false);
        ActualizarEstado();
        if (mostrarDebug) Debug.Log("[MenuPausaManager] Menu de pausa desactivado al volver.");
    }
}
