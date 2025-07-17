using UnityEngine;

public class GameOver_Controller : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = false;

    [Header("Bools")]
    public bool esGameOver;
    public bool sumandoStrike = false;

    [Header("Strikes")]
    public int strikes;

    [Header("Flags")]
    public bool flagBasura;
    public bool flagNoComer;
    public bool flagLlenura;
    public bool flagHambre;
    public bool flagEnfermo;

    [Header("Condicion Basura")]
    public float timerCondicion1;
    public float timerCondicion1_i; // Basura llena    
    public float valorStrikeNoComer;

    [Header("Condicion Hambre")]
    public float timerHambre;
    public float timerHambre_i; // Limite Hambre Strike
    public float timerHambre_GO; // Limite Hambre GameOver

    [Header("Referencias")]
    public GameManager gameManager;
    public MenuGameOver menuGameOver;
    public InstanciarBasura instanciarBasura;
    public Controlador_Puntos controladorPuntos;
    public Controlador_Fases controladorFases;
    public MovimientoStrike movimientoStrike;
    public Controlador_Combos controladorCombos;
    #endregion

    void Start()
    {
        controladorFases = GameObject.Find("Controlador_Fases").GetComponent<Controlador_Fases>();
        timerCondicion1 = timerCondicion1_i;
        timerHambre = timerHambre_i;
    }

    void Update()
    {
        if (controladorFases.enPausa)
            return;

        if (menuGameOver.esGameOver)
            return;

        CondicionBasura();
        CondicionStrikes();
        CondicionHambre();
        StrikeLlenura();
        StrikeEnfermo();
        StrikeBasura();

        if (strikes >= 4)
        {
            Controlador_EmotesT.Instance.ReproducirEmoji("GameOver");
        }
    }

    public void StrikeBasura()
    {
        if (instanciarBasura.contStrikeNoComer == valorStrikeNoComer)
        {
            if (!flagNoComer)
            {
                if (depuracion)
                    Debug.Log($"{gameObject.name}: StrikeBasura");
                SumarStrikes();
                instanciarBasura.contStrikeNoComer = 0;
                flagNoComer = true;
            }
        }
        else
        {
            flagNoComer = false;
        }
    }

    public void StrikeEnfermo()
    {
        if (controladorPuntos.estaEnfermo)
        {
            if (!flagEnfermo)
            {
                SumarStrikes();
                flagEnfermo = true;
            }
        }
        else
        {
            flagEnfermo = false;
        }
    }

    public void StrikeLlenura()
    {
        if (controladorPuntos.estaLleno)
        {
            if (!flagLlenura)
            {
                SumarStrikes();                
                flagLlenura = true;
            }
        }
        else
        {
            flagLlenura = false;
        }
    }

    public void CondicionHambre()
    {
        if (controladorPuntos.tieneHambre)
        {
            timerHambre -= Time.deltaTime;

            if (timerHambre <= 0 && !flagHambre)
            {
                SumarStrikes();
                flagHambre = true;
            }
        }
        else
        {
            timerHambre = timerHambre_i;
            flagHambre = false;
        }

        if (timerHambre < timerHambre_GO)
        {
            menuGameOver.MostrarGameOver();
        }
    }

    public void CondicionBasura()
    {
        if (instanciarBasura.basuraLlena)
        {
            timerCondicion1 -= Time.deltaTime;

            if (!flagBasura)
            {
                SumarStrikes();
                flagBasura = true;
            }

            if (timerCondicion1 <= 0)
            {
                menuGameOver.MostrarGameOver();
            }
        }
        else
        {
            timerCondicion1 = timerCondicion1_i;
            flagBasura = false;
        }
    }

    public void CondicionStrikes()
    {
        if (strikes >= gameManager.strikesMax && !menuGameOver.esGameOver)
        {
            menuGameOver.MostrarGameOver();
        }
    }

    public void SumarStrikes()
    {
        controladorPuntos.combo = 0;
        controladorCombos.CortarCombo();
        movimientoStrike.AplicarFuerzaAleatoria();
        AudioImp.Instance.Reproducir("gatoStrikes");
        Controlador_EmotesT.Instance.ReproducirEmoji("Bravo");
        strikes++;
    }
}