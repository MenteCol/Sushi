using System.Collections.Generic;
using UnityEngine;

public class ColorContadorBasura : MonoBehaviour
{
    [Header("Marcadores")]
    public List<GameObject> indicadorMarcador = new List<GameObject>();

    [Header("Referencia contador")]
    public LimpiarBasura limpiarBasura;

    [Header("Colores por estado")]
    public Color goodColor;
    public Color mediumColor;
    public Color badColor;
    public Color offColor;

    private MeshRenderer[] _renderers;

    void Awake()
    {        
        _renderers = new MeshRenderer[indicadorMarcador.Count];
        for (int i = 0; i < indicadorMarcador.Count; i++)
            _renderers[i] = indicadorMarcador[i].GetComponent<MeshRenderer>();
    }

    void Update()
    {
        ActualizarColorMarcadores();
    }

    public void ActualizarColorMarcadores()
    {     
        int count = Mathf.Clamp((int)limpiarBasura.contadorLimpiar, 0, indicadorMarcador.Count);
             
        Color activeColor;

        if (count >= indicadorMarcador.Count)
            activeColor = goodColor;      // Todos activos: Good
        else if (count >= 2)
            activeColor = mediumColor;
        else if (count == 1)
            activeColor = badColor;
        else
            activeColor = offColor;

        for (int i = 0; i < _renderers.Length; i++)
        {
            var renderer = _renderers[i];
            if (renderer == null) continue;

            var mat = renderer.material;

            if (i < count)
            {
                // Marcadores 'encendidos'
                mat.color = activeColor;
            }
            else
            {
                // Marcadores 'apagados'
                mat.color = offColor;
//                 mat.DisableKeyword("_EMISSION");
            }
        }
    }
}
