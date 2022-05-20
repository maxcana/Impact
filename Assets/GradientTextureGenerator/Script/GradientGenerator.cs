using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GradientGenerator : MonoBehaviour
{
    public Gradient gradient;
    public string savingPath = "/LKHGames/GradientTextureGenerator/GeneratedTexture/";

    [Tooltip("Width of the gradient texture, 256 by default")]
    public float width = 256;
    [Tooltip("Height of the gradient texture, 64 by default")]
    public float height = 64;

    private Texture2D gradientTexture;
    private Texture2D tempTexture;

    Texture2D GenerateGradientTexture(Gradient grad)
    {
        /*
        if (tempTexture == null)
        {
            tempTexture = new Texture2D((int)width, (int)height);
        }*/
        tempTexture = new Texture2D((int)width, (int)height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = grad.Evaluate(0 + (x / width));
                tempTexture.SetPixel(x, y, color);
            }
        }
        tempTexture.wrapMode = TextureWrapMode.Clamp;
        tempTexture.Apply();
        return tempTexture;
    }

    public void BakeGradientTexture()
    {
        gradientTexture = GenerateGradientTexture(gradient);
        byte[] _bytes = gradientTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + savingPath + "GradientTexture_" + Random.Range(0, 999999).ToString() + ".png", _bytes);
    }
}