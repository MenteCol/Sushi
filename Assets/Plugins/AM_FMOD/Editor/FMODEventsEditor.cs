using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FMODEvents))]
public class FMODEventsEditor : Editor
{
    private GUIStyle titleStyle;    
    private GUIStyle musicaBoxStyle;
    private GUIStyle sfxBoxStyle;
    private GUIStyle expandedButtonStyle;
    private GUIStyle regularButtonStyle;
    private GUIStyle deleteButtonStyle;

    private GUIStyle ambienteBoxStyle;
    private GUIStyle boxAmbienteContainer;
    private GUIStyle boxMusicaContainer;
    private GUIStyle boxSFXContainer;

    private int selectedCategory = 0; // Índice del dropdown
    private readonly string[] categories = { "Música", "SFX", "Ambiente" };

    private bool[] foldoutStatesMusica;
    private bool[] foldoutStatesSFX;

    private void InitializeStyles()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14
            };
        }

        if (musicaBoxStyle == null)
        {
            musicaBoxStyle = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.4f, 0.3f, 0.6f, 1f), new Color(0.3f, 0.2f, 0.5f, 1f)) } // Tono púrpura
            };
        }

        if (sfxBoxStyle == null)
        {
            sfxBoxStyle = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.2f, 0.4f, 0.2f, 1f), new Color(0.1f, 0.3f, 0.1f, 1f)) } // Tono verde
            };
        }

        if (expandedButtonStyle == null)
        {
            expandedButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = MakeTex(2, 2, new Color(0.6f, 0.2f, 0.3f, 1f)) }, // Fondo verde suave para expandido
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };
        }

        if (regularButtonStyle == null)
        {
            regularButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal =
        {
            background = MakeTex(2, 2, new Color(0.2f, 0.8f, 0.2f, 1f)), // Fondo verde suave
            textColor = Color.black // Cambiar el color de la letra a negro
        },
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter
            };
        }


        if (boxMusicaContainer == null)
        {
            boxMusicaContainer = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.2f, 0.15f, 0.45f, 1f), new Color(0.3f, 0.2f, 0.5f, 1f)) } // Púrpura oscuro
            };
        }

        if (boxSFXContainer == null)
        {
            boxSFXContainer = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.1f, 0.2f, 0.25f, 1f), new Color(0.1f, 0.3f, 0.1f, 1f)) } // Verde oscuro
            };
        }

        if (deleteButtonStyle == null)
        {
            deleteButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = GUI.skin.button.normal, // Mantener el fondo normal
                hover = new GUIStyleState
                {
                    background = MakeTex(2, 2, new Color(1f, 0.4f, 0.4f, 1f)), // Fondo rojizo
                    textColor = GUI.skin.button.hover.textColor // Mantener el color de texto estándar
                },
                fontStyle = GUI.skin.button.fontStyle,
                alignment = GUI.skin.button.alignment
            };
        }

        if (ambienteBoxStyle == null)
        {
            ambienteBoxStyle = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.2f, 0.3f, 0.5f, 1f), new Color(0.1f, 0.2f, 0.4f, 1f)) } // Azul suave
            };
        }

        if (boxAmbienteContainer == null)
        {
            boxAmbienteContainer = new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(1, 1, 1, 1),
                normal = { background = MakeTexWithBorder(2, 2, new Color(0.15f, 0.25f, 0.4f, 1f), new Color(0.1f, 0.2f, 0.3f, 1f)) } // Azul más oscuro
            };
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = col;
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    private Texture2D MakeTexWithBorder(int width, int height, Color backgroundColor, Color borderColor)
    {
        int borderThickness = 1;
        Texture2D tex = new Texture2D(width + 2 * borderThickness, height + 2 * borderThickness)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        Color[] pixels = new Color[tex.width * tex.height];
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                pixels[x + y * tex.width] = (x < borderThickness || x >= tex.width - borderThickness || y < borderThickness || y >= tex.height - borderThickness)
                    ? borderColor
                    : backgroundColor;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    public override void OnInspectorGUI()
    {
        InitializeStyles();
        serializedObject.Update();

        // Recuperar el índice guardado al iniciar
        if (selectedCategory == 0 && EditorPrefs.HasKey("SelectedCategory"))
        {
            selectedCategory = EditorPrefs.GetInt("SelectedCategory");
        }

        // Dropdown para seleccionar categoría
        int newSelectedCategory = EditorGUILayout.Popup("Categoria Eventos", selectedCategory, categories);

        // Guardar el índice seleccionado si cambia
        if (newSelectedCategory != selectedCategory)
        {
            selectedCategory = newSelectedCategory;
            EditorPrefs.SetInt("SelectedCategory", selectedCategory);
        }

        // Renderizar la sección correspondiente
        switch (selectedCategory)
        {
            case 0:
                RenderMusicaSection();
                break;
            case 1:
                RenderSFXSection();
                break;
            case 2:
                RenderAmbienteSection();
                break;
        }

        EditorGUILayout.Space();

        // Agregar el "By: Qpa85" al final
        GUIStyle footerStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Italic,
            fontSize = 10,
            alignment = TextAnchor.MiddleRight,
            normal = { textColor = Color.gray }
        };
        EditorGUILayout.LabelField("By: Qpa85", footerStyle);

        serializedObject.ApplyModifiedProperties();
    }

    private void RenderMusicaSection()
    {
        SerializedProperty musicaProperty = serializedObject.FindProperty("musica");
        InitializeFoldoutStates(musicaProperty, ref foldoutStatesMusica);
        RenderEventList(musicaProperty, musicaBoxStyle, ref foldoutStatesMusica, 1, boxMusicaContainer);
    }

    private void RenderSFXSection()
    {
        SerializedProperty sfxProperty = serializedObject.FindProperty("sfx");
        InitializeFoldoutStates(sfxProperty, ref foldoutStatesSFX);
        RenderEventList(sfxProperty, sfxBoxStyle, ref foldoutStatesSFX, 1, boxSFXContainer);
    }

    private void RenderAmbienteSection()
    {
        SerializedProperty ambienteProperty = serializedObject.FindProperty("ambiente");
        InitializeFoldoutStates(ambienteProperty, ref foldoutStatesMusica);
        RenderEventList(ambienteProperty, ambienteBoxStyle, ref foldoutStatesMusica, 1, boxAmbienteContainer);
    }


    private void RenderEventList(SerializedProperty listProperty, GUIStyle boxStyle, ref bool[] foldoutStates, int indentLevel, GUIStyle boxAfuera)
    {
        AdjustFoldoutStates(listProperty, ref foldoutStates);

        EditorGUILayout.BeginVertical(boxAfuera);

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            SerializedProperty nombreEvento = element.FindPropertyRelative("nombreEvento");
            SerializedProperty eventReference = element.FindPropertyRelative("eventReference");

            string foldoutKey = $"FMODEvents_{listProperty.name}_{i}";
            foldoutStates[i] = GetFoldoutState(foldoutKey, foldoutStates[i]);

            // Determinar el estilo según el estado del objeto
            GUIStyle buttonStyle = foldoutStates[i] ? expandedButtonStyle : GUI.skin.button;
            GUIContent buttonContent = new GUIContent(string.IsNullOrEmpty(nombreEvento.stringValue) ? $"Evento {i + 1}" : nombreEvento.stringValue);

            // Botón alineado al lado derecho
            if (GUILayout.Button(buttonContent, buttonStyle, GUILayout.ExpandWidth(true)))
            {
                foldoutStates[i] = !foldoutStates[i];
                SetFoldoutState(foldoutKey, foldoutStates[i]);
            }
            GUILayout.Space(0f);

            // Mostrar detalles si está expandido
            if (foldoutStates[i])
            {
                GUILayout.Space(5); // Espacio específico para contenido expandido
                EditorGUILayout.BeginVertical(boxStyle);

                EditorGUILayout.BeginHorizontal();
                nombreEvento.stringValue = EditorGUILayout.TextField("", nombreEvento.stringValue);
                if (GUILayout.Button("Eliminar", deleteButtonStyle, GUILayout.Width(75)))
                {
                    EditorPrefs.DeleteKey(foldoutKey);
                    listProperty.DeleteArrayElementAtIndex(i);
                    AdjustFoldoutStates(listProperty, ref foldoutStates);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(eventReference);

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }
        }

        if (GUILayout.Button("+", GUILayout.Width(40)))
        {
            listProperty.arraySize++;
            AdjustFoldoutStates(listProperty, ref foldoutStates);
        }

        EditorGUILayout.EndVertical();
    }



    private void AdjustFoldoutStates(SerializedProperty listProperty, ref bool[] foldoutStates)
    {
        int newSize = listProperty.arraySize;
        if (foldoutStates == null || foldoutStates.Length != newSize)
        {
            bool[] newFoldoutStates = new bool[newSize];
            for (int i = 0; i < Mathf.Min(foldoutStates?.Length ?? 0, newSize); i++)
            {
                newFoldoutStates[i] = foldoutStates[i];
            }

            if (newSize > foldoutStates?.Length)
            {
                newFoldoutStates[newSize - 1] = true;
            }

            foldoutStates = newFoldoutStates;
        }
    }

    private void InitializeFoldoutStates(SerializedProperty listProperty, ref bool[] foldoutStates)
    {
        if (foldoutStates == null || foldoutStates.Length != listProperty.arraySize)
        {
            foldoutStates = new bool[listProperty.arraySize];
            for (int i = 0; i < foldoutStates.Length; i++)
            {
                foldoutStates[i] = GetFoldoutState($"FMODEvents_{listProperty.name}_{i}");
            }
        }
    }

    private bool GetFoldoutState(string key, bool defaultValue = true)
    {
        return EditorPrefs.GetBool(key, defaultValue);
    }

    private void SetFoldoutState(string key, bool state)
    {
        EditorPrefs.SetBool(key, state);
    }
}
