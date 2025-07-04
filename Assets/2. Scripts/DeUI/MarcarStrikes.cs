using UnityEngine;
public class MarcarStrikes : MonoBehaviour
{
    public GameObject imagenStrikeCheck;
    public bool esCheck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (esCheck)
        {         
            imagenStrikeCheck.SetActive(true);
        }
    }

}
