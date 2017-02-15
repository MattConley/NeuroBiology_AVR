using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClearPlus : MonoBehaviour {
    static TextMesh text;
    static float progress = 0.0f;
    static int fontSize = 100;
    // Use this for initialization
    public void Start()
    {
        text = GameObject.FindGameObjectWithTag("Plus").GetComponent<TextMesh>();
        Opacity(progress, fontSize);
    }
    public static void Opacity(float amount, int Size)
    {
        text.color = Color.clear;
        progress += amount;
        
        if (progress < 1.0f || progress == 1.0f)
        {
            text.color = Color.Lerp(Color.clear, Color.red, progress);
            text.fontSize += Size;
        }
        else if (progress < 0.0f)
        {
            progress = 0.0f;
            text.fontSize = 200;
        }
        else
        {
            progress = 1.0f;
            text.fontSize += Size;
        }    
    }
}
