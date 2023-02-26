// WidgetInspector.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Widget))]
public class WidgetInspector : Editor
{
    SerializedProperty name;
    SerializedProperty count;

    float renderWidth, controlWidth;

    GUIStyle buttonOffStyle, buttonOnStyle;
    bool buttonOn = false;

    void OnEnable() 
    {
        name = serializedObject.FindProperty("name");
        count = serializedObject.FindProperty("count");

    }

    private void OnSceneGUI()
    {
        if (buttonOffStyle == null)
        {
            buttonOffStyle = new GUIStyle("button");
            buttonOnStyle = new GUIStyle("button");

            buttonOnStyle.normal.background = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            buttonOnStyle.normal.background.SetPixels(new Color[] { Color.green });
            buttonOnStyle.normal.background.Apply();

        }
    }

    public override void OnInspectorGUI() 
    {
        // Update the connecting object
        serializedObject.Update();

        // Render the controls and handle user input
        name.stringValue = EditorGUILayout.TextField("Widget Name", name.stringValue);
        int prevCount = count.intValue;
        count.intValue = EditorGUILayout.IntField("Widget Count", count.intValue);
        count.intValue = Mathf.Clamp(count.intValue, 0, 17);


//        EditorGUILayout.LabelField("");

//        EditorGUI.DrawRect(new Rect(3, 50, 100, 20), Color.green);


        if (GUILayout.Button("um", buttonOn ? buttonOnStyle : buttonOffStyle))
        {
            Debug.Log("plorgh");
            buttonOn = !buttonOn;

        }

        //renderWidth = EditorGUIUtility.currentViewWidth;
        //controlWidth = renderWidth * 0.6f;   // 60%

        //DrawBorderedRect(GetControlRect(50), Color.green, 2, Color.gray);

        GUIContent imgC = new GUIContent(buttonOffStyle.normal.background);

        EditorGUILayout.LabelField(imgC);

        imgC = new GUIContent("Off", buttonOnStyle.normal.background);
        EditorGUILayout.LabelField("On", imgC);

        // Apply modified properties to the connecting object
        serializedObject.ApplyModifiedProperties();
    }

    Rect GetControlRect(int height)
    {
        return new Rect(renderWidth - controlWidth, height, controlWidth - 5, 20);
    }

    void DrawBorderedRect(Rect area, Color interiorColor, int borderThickness, Color borderColor)
    {
        if (borderThickness > 0)
        {
            EditorGUI.DrawRect(area, borderColor);
        }

        Rect interior = new Rect(area);
        interior.x += borderThickness;
        interior.width -= borderThickness * 2;
        interior.y += borderThickness;
        interior.height -= borderThickness * 2;

        EditorGUI.DrawRect(interior, interiorColor);
    }

}
