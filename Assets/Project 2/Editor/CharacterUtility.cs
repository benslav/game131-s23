using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterUtility
{

    public static void DrawClassLevelLabel(Character character)
    {
        EditorGUILayout.LabelField("Class/Level", string.Format("{0} {1}", character.characterClass, character.level)) ;
    }

}
