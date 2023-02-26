using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSettingsFromFile : MonoBehaviour
{

    // Set in the inspector
    public BounceController bounceController;
    public TextAsset file;

    // Start is called before the first frame update
    void Start()
    {
        string textContents = file.text;
        string[] lines = textContents.Split(new char[] { '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) 
        {
            // TODO: Parse the line into values.
            // Assign those values to the appropriate fields in bounceController.
        }

        int t = int.Parse("355");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
