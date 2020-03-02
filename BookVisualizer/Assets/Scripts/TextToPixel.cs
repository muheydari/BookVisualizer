using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextToPixel : MonoBehaviour
{
    public int GridSize;
    public TextAsset mainText;
    List<string> allCharacterArray = new List<string>();
    List<string> allAlphabet = new List<string>()
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" 
        
    };

    Texture2D finalImage;

    public string S;

    // Start is called before the first frame update
    void Start()
    {
        for( var i = 32; i <= 126; i++ )
        {
         //   S += String.fromCharCode( i );
        }
        for (int i = 0; i < mainText.text.Length; i++)
        {
            allCharacterArray.Add(mainText.text[i].ToString().ToLower());
        }

        finalImage = new Texture2D(GridSize, GridSize, TextureFormat.RGB24,false);

        int c = 0 ;
        int yPixel = 0;
        for (int i = 0; i < allCharacterArray.Count; i++)
        {
            c++;
            if (c > GridSize)
            {
                yPixel++;
                c = 0;
            }

            Color color = new Color(0f, 0f, 0f, 1f);
            if (allAlphabet.Contains(allCharacterArray[i].ToLower()))
            {
                int t = Array.FindIndex(allCharacterArray.ToArray(), s => s.Equals(allCharacterArray[i].ToLower()));
                float colorV = (float)t / (float)allAlphabet.Count;
                color = new Color(colorV, 0f, 0f, 1f);
            }
            finalImage.SetPixel(c, yPixel, color);
        }
        finalImage.Apply();
        finalImage.filterMode = FilterMode.Point;
        finalImage.wrapMode = TextureWrapMode.Clamp;
        byte[] bytes = finalImage.EncodeToPNG();
        File.WriteAllBytes("Textulize_" + mainText.name + "_"+ DateTime.UtcNow + ".png", bytes);
        
        MeshRenderer m = GetComponent<MeshRenderer>();
        m.material.mainTexture = finalImage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
