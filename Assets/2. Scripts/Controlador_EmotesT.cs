using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controlador_EmotesT : MonoBehaviour
{
    public static Controlador_EmotesT Instance { get; private set; }

    [SerializeField] private Vector3 posicionInicial;
    [SerializeField] private Vector3 escalaInicial;

    [Header("Configuración Animación")]
    [SerializeField] private Vector3 posicionFinal;
    [SerializeField] private Vector3 escalaFinal;
    [SerializeField] private float velocidadEscala = 0.5f;
    [SerializeField] private float velocidadPosicion = 0.5f;
    [SerializeField] private float tiempoMostrado = 0.5f;

    private Coroutine escalaPosicionCoroutine;

    [Header("Animator")]
    public Animator animatorEmojis;
    public bool reproduciendoEmoji;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        transform.localScale = escalaInicial;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ReproducirEmoji("Boost");
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReproducirEmoji("Hambre");
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ReproducirEmoji("Feliz");
        }
    }

    private IEnumerator CicloAnimacionCompleto(string estadoEmji)
    {
        animatorEmojis.Play(estadoEmji);

        yield return Tweening.SetScaledPosition(
            transform,
            posicionFinal,
            escalaFinal,
            velocidadEscala,
            velocidadPosicion,
            true
        );

        Debug.Log("Mostrando " + estadoEmji);

        // Pausa antes de regresar
        yield return new WaitForSecondsRealtime(tiempoMostrado);

        // Animación de retorno
        yield return Tweening.SetScaledPosition(
            transform,
            posicionInicial,
            escalaInicial,
            velocidadEscala,
            velocidadPosicion,
            true
        );

        reproduciendoEmoji = false;
    }

    public void ReproducirEmoji(string estadoEmoji)
    {
        if (reproduciendoEmoji)
            return;

        if (escalaPosicionCoroutine != null)
        {
            StopCoroutine(escalaPosicionCoroutine);
        }

        if (!reproduciendoEmoji)
        {
            escalaPosicionCoroutine = StartCoroutine(CicloAnimacionCompleto(estadoEmoji));
            reproduciendoEmoji = true;
        }
    }
}
