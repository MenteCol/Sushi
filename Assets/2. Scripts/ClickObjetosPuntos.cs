using UnityEngine;

public class ClickObjetosPuntos : MonoBehaviour
{
    [Header("Identificador Asignado")]
    [SerializeField] public int ID;
    [Header("Valor Instancia")]
    public float puntosLlenuraDif;
    [Header("Valores Generales")]
    [SerializeField] public int puntosComida;
    [SerializeField] public float puntosLlenura;
    [SerializeField] public int puntosMalestar;
    [Header("Audio")]
    [SerializeField] private string audioComer;
    [SerializeField] private string audioNoPuede;
    [SerializeField] private Controlador_Puntos controladorPuntos;
    [SerializeField] private Controlador_Instancias controladorInstancias;

    void Start()
    {
        controladorPuntos = GameObject.Find("Controlador_Puntaje").GetComponent<Controlador_Puntos>();
        controladorInstancias = GameObject.Find("Controlador_Instancias").GetComponent<Controlador_Instancias>();
    }

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!controladorPuntos.estaEnfermo && !controladorPuntos.estaLleno && !controladorInstancias.basuraLlena)
        {
            switch (ID)
            {
                case 1:
                    puntosComida = 1;
                    puntosLlenura = 0.1f;                    
                    break;
                case 2:
                    puntosComida = 1;
                    puntosLlenura = 0.2f;
                    puntosMalestar = 1;
                    break;
                case 3:
                    puntosComida = 1;
                    puntosLlenura = 0.05f;
                    break;
                default:
                    return;
            }
            
            controladorPuntos.SumarPuntos(puntosComida, puntosLlenura * puntosLlenuraDif, puntosMalestar);

            AudioImp.Instance.Reproducir(audioComer);
            Destroy(gameObject);
        }

        if (controladorPuntos.estaEnfermo)
        {
            AudioImp.Instance.Reproducir(audioNoPuede);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banda"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Banda"))
        {
            transform.SetParent(null);
        }
    }

    public void AsignarID(int nuevoID)
    {
        ID = nuevoID;
    }

}
