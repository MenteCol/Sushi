using UnityEngine;
using System.Collections.Generic;

public class ClickObjetosPuntos : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Configuración de Objeto")]
    [SerializeField] public int ID;
    [SerializeField] private float multiplicadorLlenura = 1f;

    [Header("Valores de Interacción")]
    [SerializeField] private int puntosComida = 1;
    [SerializeField] private float puntosLlenura = 0.1f;
    [SerializeField] private int puntosMalestar = 0;

    [Header("Audio")]
    [SerializeField] private string audioComer = "Comer";
    [SerializeField] private string audioNoPuede = "NoPuede";

    [Header("Raycast")]
    [SerializeField] private bool hacerRaycast = true;
    [SerializeField] private float distanciaRaycast = 2f;
        
    private bool mostrarGizmo = false;
    private Transform bandaDetectada = null;

    [Header("Referencias")]
    [SerializeField] private Controlador_Puntos controladorPuntos;
    [SerializeField] private InstanciarBasura instanciarBasura;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Controlador_Fases controladorFases;
    #endregion

    private void Start()
    {
        controladorFases = FindAnyObjectByType<Controlador_Fases>();

        if (controladorPuntos == null)
            controladorPuntos = FindAnyObjectByType<Controlador_Puntos>();

        if (instanciarBasura == null)
            instanciarBasura = FindAnyObjectByType<InstanciarBasura>();

        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        ValidarReferencias();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 origen = transform.position;
        Vector3 direccion = Vector3.down;

        if (Physics.Raycast(origen, direccion, out hit, distanciaRaycast) && hacerRaycast)
        {
            Debug.DrawRay(origen, direccion * hit.distance, Color.cyan);

            if (hit.collider.CompareTag("Banda"))
            {
                if (bandaDetectada != hit.transform)
                {
                    bandaDetectada = hit.transform;
                    transform.SetParent(bandaDetectada);
                    Invoke("QuitarRaycast", 2f);
                }
            }
        }
    }

    private void ValidarReferencias()
    {
        if (controladorFases == null && depuracion)
            Debug.LogWarning($"[{gameObject.name}] No se encontró Controlador_Fases en la escena");

        if (controladorPuntos == null && depuracion)
            Debug.LogWarning($"[{gameObject.name}] No se encontró Controlador_Puntos en la escena");

        if (instanciarBasura == null && depuracion)
            Debug.LogWarning($"[{gameObject.name}] No se encontró InstanciarBasura en la escena");

        if (gameManager == null && depuracion)
            Debug.LogWarning($"[{gameObject.name}] No se encontró GameManager en la escena");
    }

    public void RegistrarToque(int indiceDedo)
    {
        if (!PuedeInteractuar())
        {
            ReproducirAudioNoPuede();
            return;
        }

        ConfigurarValoresPorID();
        EjecutarAccionPrincipal();        
        DestruirObjeto();
    }

    private bool PuedeInteractuar()
    {
        if (controladorPuntos == null || instanciarBasura == null)
            return false;

        return !controladorPuntos.estaEnfermo &&
               !controladorPuntos.estaLleno &&
               !instanciarBasura.basuraLlena;
    }

    private void ConfigurarValoresPorID()
    {
        TomarTransformParticula();

        switch (ID)
        {
            case 1:
                puntosComida = 1;
                puntosLlenura = gameManager.valorBuenaLlenura;
                puntosMalestar = 0;
                break;
            case 2:
                puntosComida = 1;
                puntosLlenura = gameManager.valorMalaLlenura;
                puntosMalestar = 1;
                break;
            case 3:
                puntosComida = 1;
                puntosLlenura = 0.05f;
                gameManager?.ReducirVelocidad();
                puntosMalestar = 0;
                break;
            default:
                if (depuracion)
                    Debug.LogWarning($"[{gameObject.name}] ID {ID} no reconocido");
                break;
        }
    }

    private void EjecutarAccionPrincipal()
    {
        if (controladorPuntos != null)
        {
            controladorPuntos.SumarPuntos(
                puntosComida,
                puntosLlenura * multiplicadorLlenura,
                ID,
                1,
                puntosMalestar
            );
        }

        if (AudioImp.Instance != null)
            AudioImp.Instance.Reproducir(audioComer);

        MostrarFeedbackVisual();
    }

    private void ReproducirAudioNoPuede()
    {
        if (AudioImp.Instance != null)
            AudioImp.Instance.Reproducir(audioNoPuede);
    }

    private void MostrarFeedbackVisual()
    {
        mostrarGizmo = true;
        CancelInvoke(nameof(DesactivarGizmo));
        Invoke(nameof(DesactivarGizmo), 1f);
    }

    private void DestruirObjeto()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void DesactivarGizmo() => mostrarGizmo = false;

    private void OnDrawGizmos()
    {
        if (mostrarGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }

    private void QuitarRaycast()
    {
        hacerRaycast = false;
    }


    private void TomarTransformParticula()
    {
        controladorPuntos.MostrarParticula(this.transform);
    }

    public void AsignarID(int nuevoID) => ID = nuevoID;
}