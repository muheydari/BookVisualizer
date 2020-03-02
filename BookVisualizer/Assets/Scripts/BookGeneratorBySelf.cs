using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using System.IO;


public class BookGeneratorBySelf : MonoBehaviour
{
    public TextAsset OrginalBook = null;
    private string[] AllDatabaseWords;
    private Texture2D finalImage;

    public List<Color> ColorKey = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        //all_words = Regex.Split(OrginalBook.text, @"\s+", RegexOptions.Multiline);
        string[] currentWordList = Regex.Replace(Regex.Replace(OrginalBook.text, "[^a-zA-Z0-9 ]", " "), @"\s+", " ")
            .Split(' ');

        AllDatabaseWords = currentWordList.Distinct().ToArray();
        Array.Sort(AllDatabaseWords);

        int i;
        for (i = 0; i < AllDatabaseWords.Length; i++)
        {
            Color x = Color.black;
            Debug.Log("i - " + i + " - " + ToHex(i));

            ColorUtility.TryParseHtmlString(ToHex(i), out x);
            Debug.Log(ColorUtility.ToHtmlStringRGB((Color.cyan)));

            var r = (i >> 16) & 255;
            var g = (i >> 8) & 255;
            var b = i & 255;

            ColorKey.Add(new Color(r, g, b));
            //    int.Parse((i.ToString()), NumberStyles.HexNumber);
        }

        int sizeOfTexture = Mathf.RoundToInt(Mathf.Pow(currentWordList.Length, 0.5f));
        finalImage = new Texture2D(sizeOfTexture, sizeOfTexture, TextureFormat.RGB24, false);
        int c = 0;
        int yPixel = 0;
        i = 0;
        for (; i < currentWordList.Length - 1; i++)
        {
            c++;
            if (c > sizeOfTexture)
            {
                yPixel++;
                c = 0;
            }

            int colorIndex = 0;
            if (AllDatabaseWords.Contains(currentWordList[i].ToLower()))
            {
                colorIndex = Array.FindIndex(AllDatabaseWords.ToArray(), s => s.Equals(currentWordList[i].ToLower()));
            }

            finalImage.SetPixel(c, yPixel, ColorKey[colorIndex]);
        }

        finalImage.Apply();
        finalImage.filterMode = FilterMode.Point;
        finalImage.wrapMode = TextureWrapMode.Clamp;
        byte[] bytes = finalImage.EncodeToPNG();
        File.WriteAllBytes("Textulize_" + OrginalBook.name + "_" + ".png" , bytes);

        MeshRenderer m = GetComponent<MeshRenderer>();
        if (m)
            m.material.mainTexture = finalImage;
    }

    private string ToHex(int value)
    {
        return String.Format("0x{0:X}", value);
    }

    public int FromHex(string value)
    {
        // strip the leading 0x
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            value = value.Substring(2);
        }

        return Int32.Parse(value, NumberStyles.HexNumber);
    }
}