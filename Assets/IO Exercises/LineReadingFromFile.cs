using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineReadingFromFile : MonoBehaviour
{
    public TextAsset textFile;

    // Start is called before the first frame update
    void Start()
    {
        string textContents = textFile.text;
        string[] lines = textContents.Split(new char[] { '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++) 
        {
            print(string.Format("Line {0} is {1} characters long: {2}\n", i, lines[i].Length, lines[i]));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
