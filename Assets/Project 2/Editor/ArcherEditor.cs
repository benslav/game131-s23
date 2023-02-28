using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Archer))]
public class ArcherEditor : Editor
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

        Archer archerObject = target as Archer;

        CharacterUtility.DrawClassLevelLabel(archerObject);


        // ************ Endrendering custom content ************* //

        // Apply modified properties to the connecting object
        serializedObject.ApplyModifiedProperties();
    }
}
