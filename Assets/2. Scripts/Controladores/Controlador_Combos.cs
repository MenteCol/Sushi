using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Controlador_Combos : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Objetos UI")]
    public RectTransform panelCombos;
    public TextMeshProUGUI textoCombo;

    [Header("Variables")]
    public float velocidadMostrar = 0.5f;
    public float velocidadOcultar = 0.5f;
    public float timerCombo;
    public float timerCombo_i;
    public int ultimoCombo = 0;

    private Coroutine animacionEscala;
    private Vector3 escalaOriginal = Vector3.one;
    private Vector3 escalaOculta = Vector3.zero;

    // Input System para testeo manual
    private InputAction mostrarComboAction;
    private InputAction esconderComboAction;

    [Header("Referencias")]
    public Controlador_Puntos controladorPuntos;
    #endregion

    private void Awake()
    {
        mostrarComboAction = new InputAction("MostrarCombo", binding: "<Keyboard>/m");
        esconderComboAction = new InputAction("EsconderCombo", binding: "<Keyboard>/n");

        mostrarComboAction.performed += ctx => MostrarCombo();
        esconderComboAction.performed += ctx => EscondiendoCombo();
    }

    private void OnEnable()
    {
        mostrarComboAction.Enable();
        esconderComboAction.Enable();
    }

    private void OnDisable()
    {
        mostrarComboAction.Disable();
        esconderComboAction.Disable();
    }

    void Start()
    {
        escalaOriginal = panelCombos.localScale;
        panelCombos.localScale = escalaOculta;
        timerCombo = timerCombo_i;
    }

    void Update()
    {
        textoCombo.text = controladorPuntos.combo.ToString();

        if (controladorPuntos.combo > 0 && ultimoCombo > 0)
        {
            if (timerCombo >= 0)
            {
                timerCombo -= Time.deltaTime;

                if (timerCombo < 0)
                {
                    EscondiendoCombo();
                }
            }
        }

        ultimoCombo = controladorPuntos.combo;
    }

    public void MostrarCombo()
    {
        if (animacionEscala != null)
            StopCoroutine(animacionEscala);

        timerCombo = timerCombo_i;
        animacionEscala = StartCoroutine(Tweening.SetScale(panelCombos, escalaOriginal, velocidadMostrar));
    }

    public void EscondiendoCombo()
    {
        if (animacionEscala != null)
            StopCoroutine(animacionEscala);

        animacionEscala = StartCoroutine(EsconderYDesactivar());
    }

    private IEnumerator EsconderYDesactivar()
    {
        if (depuracion)
            Debug.Log($"{gameObject.name}: Corrutina Ocultar Twening");
        yield return Tweening.SetScale(panelCombos, escalaOculta, velocidadOcultar);
    }
}