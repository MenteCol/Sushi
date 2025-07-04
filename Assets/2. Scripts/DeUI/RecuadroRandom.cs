using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecuadroRandom : MonoBehaviour
{
    #region Variables
    [Header("Depuracion")]
    public bool depuracion = true;

    [Header("Objetos")]
    public List<GameObject> imagenRecuadro = new List<GameObject>();
    #endregion

    private void OnEnable()
    {
        if (imagenRecuadro == null || imagenRecuadro.Count == 0)
        {
            return;
        }

        int index = Random.Range(0, imagenRecuadro.Count);
        imagenRecuadro[index].SetActive(true);

        if (depuracion)
            Debug.Log($"{gameObject.name}: Se mostro la imagen {imagenRecuadro[index].GetComponent<Image>().name}");

    }

    private void OnDisable()
    {
        foreach (GameObject obj in imagenRecuadro)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}