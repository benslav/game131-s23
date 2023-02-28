using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fighter))]
public class FighterEditor : Editor
{
    void OnEnable()
    {

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

        Fighter fighterObject = target as Fighter;

        CharacterUtility.DrawClassLevelLabel(fighterObject as Character);


        // ************ Endrendering custom content ************* //

        // Apply modified properties to the connecting object
        serializedObject.ApplyModifiedProperties();
    }
}
