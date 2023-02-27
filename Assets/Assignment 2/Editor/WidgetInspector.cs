// WidgetInspector.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WidgetInspector : Editor
{
    SerializedProperty widgetName, count;
    GUIStyle buttonOffStyle, buttonOnStyle;

    void OnEnable() 
    {
        widgetName = serializedObject.FindProperty("widgetName");
    }

    private void OnSceneGUI()
    {
        // HINT: Use this to initialize GUIStyles
    }

    public override void OnInspectorGUI() 
    {
        // Update the connecting object
        serializedObject.Update();

        // ************ Start rendering custom content ************* //


        // A sample string property, same as the examples, for the [widgetName] property of Widget (see Widget.cs)
        widgetName.stringValue = EditorGUILayout.TextField("Name", widgetName.stringValue);

        // A sample int property, same as the examples, for the [count] property of Widget (see Widget.cs)
        count.intValue = EditorGUILayout.IntField("Count", count.intValue);


        // TODO: Create an int control for the [number] property of Widget (see Widget.cs)
        // TODO: Clamp this control's value between 0 and 100

        // TODO: Draw a green rectangle 20 pixels high and half the current width of the control,
        // horizontally centered (HINT: EditorGUIUtility.currentViewWidth)

        // TODO: Draw a button labled "Button" that causes "button clicked" to appear
        // in the Console (HINT: You can use GUILayout as well as EditorGUILayout here)


        // ************ Endrendering custom content ************* //

        // Apply modified properties to the connecting object
        serializedObject.ApplyModifiedProperties();
    }


}
