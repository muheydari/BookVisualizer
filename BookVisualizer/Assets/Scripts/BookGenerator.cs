using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BookGenerator : MonoBehaviour
{
    public int GridSize;
    public TextAsset mainText;
    private readonly List<string> allCharacterArray = new List<string>();
    private readonly List<string> allAlphabet;

    private Texture2D finalImage;

    public BookGenerator()
    {
        allAlphabet = new List<string>()
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"
        };
    }

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var t in mainText.text)
        {
            allCharacterArray.Add(t.ToString().ToLower());
        }

        finalImage = new Texture2D(GridSize, GridSize, TextureFormat.RGB24, false);

        var c = 0;
        var yPixel = 0;
        var color = new Color(0f, 0f, 0f, 1f);
        foreach (var t1 in allCharacterArray)
        {
            c++;
            if (c > GridSize)
            {
                yPixel++;
                c = 0;
            }

            if (allAlphabet.Contains(t1.ToLower()))
            {
                var t = Array.FindIndex(allCharacterArray.ToArray(), s => s.Equals(t1.ToLower()));
                color = new Color(t, 0f, 0f, 1f);
            }
            finalImage.SetPixel(c, yPixel, color);
        }
        finalImage.Apply();
        finalImage.filterMode = FilterMode.Point;
        finalImage.wrapMode = TextureWrapMode.Clamp;
        byte[] bytes = finalImage.EncodeToPNG();
        File.WriteAllBytes("Textulize.png", bytes);

        MeshRenderer m = GetComponent<MeshRenderer>();
        m.material.mainTexture = finalImage;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}

public static class WordRGB
{
    public static Color GetWordRGB()
    {
        var _color = Color.white;
        return _color;
    }
    
}
