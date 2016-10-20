using UnityEngine;
using System.Collections;
using System;

public class LoadOnClick : MonoBehaviour
{

    public GameObject loadingImage;
    public GameObject Canvas;
 
    public GUITexture overlay;
    public float fadeTime;
    void Awake()
    {
        overlay.pixelInset = new Rect(0, 0, Screen.width, Screen.height);

        //fade to clear
        StartCoroutine(FadetoClear());
    }
    public void LoadScene(int level)
    {
        StartCoroutine(FadetoBlack(() => Application.LoadLevel(level)));
        
    }
    private IEnumerator FadetoClear()
    {
        overlay.gameObject.SetActive(true);
        overlay.color = Color.black;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;
        //
        while (progress < 1.0f)
        {
            overlay.color = Color.Lerp(Color.black, Color.clear, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
        Canvas.SetActive(true);
        overlay.color = Color.clear;
        overlay.gameObject.SetActive(false);
    }
    private IEnumerator FadetoBlack(Action levelMethod)
    {
        overlay.color = Color.clear;
        overlay.gameObject.SetActive(true);

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;

        while (progress < 1.0f)
        {
            overlay.color = Color.Lerp(Color.clear, Color.black, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
        overlay.color = Color.black;
        levelMethod();
    }
}