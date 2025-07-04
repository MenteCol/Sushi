using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
public class RaycastPaddingVisualizer : MonoBehaviour
{
    public Vector4 padding = new Vector4(-1, -1, -1, -1);
    public Color paddingColor = new Color(0, 1, 0, 0.25f);

    private GameObject paddingVisual;

    void Start()
    {
        CreatePaddingVisual();
    }

    public void CreatePaddingVisual()
    {
        // Elimina el visualizador anterior si existe
        if (paddingVisual != null)
        {
            DestroyImmediate(paddingVisual);
        }

        // Crea el objeto hijo visualizador
        paddingVisual = new GameObject("RaycastPaddingVisual");
        paddingVisual.transform.SetParent(transform, false);

        Image img = paddingVisual.AddComponent<Image>();
        img.color = paddingColor;
        img.raycastTarget = false;

        RectTransform rt = paddingVisual.GetComponent<RectTransform>();
        RectTransform parentRT = GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = parentRT.pivot;

        rt.offsetMin = new Vector2(padding.x, padding.y);
        rt.offsetMax = new Vector2(-padding.z, -padding.w);

        Outline outline = paddingVisual.AddComponent<Outline>();
        outline.effectColor = Color.green;
        outline.effectDistance = new Vector2(1, 1);
    }

    // Limpia el visualizador al destruir el objeto
    private void OnDestroy()
    {
        if (paddingVisual != null)
        {
            DestroyImmediate(paddingVisual);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RaycastPaddingVisualizer))]
public class RaycastPaddingVisualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RaycastPaddingVisualizer visualizer = (RaycastPaddingVisualizer)target;
        if (GUILayout.Button("Actualizar Visualizador"))
        {
            visualizer.CreatePaddingVisual();
        }
    }
}
#endif
