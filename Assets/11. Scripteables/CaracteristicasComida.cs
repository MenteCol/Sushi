using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CaracteristicasComida", menuName = "Comida/Caracteristicas", order = 0)]
[System.Serializable]
public class CaracteristicasComida : ScriptableObject
{
    [Header("Prefabs Comidas")]
    public List<ClickObjetosPuntos> prefabsComidaBuena = new List<ClickObjetosPuntos>();
    public List<ClickObjetosPuntos> prefabsComidaMala = new List<ClickObjetosPuntos>();
    public List<ClickObjetosPuntos> prefabsComidaBoost = new List<ClickObjetosPuntos>();
}
