using UnityEngine;
using System.Collections;
using System;

public class AutomaticLoad : MonoBehaviour {
    public float Timer;
    public int level;
    public GUITexture overlay;
    public float fadeTime;
    
    void Awake()
    {
        //BeginTimer
        LoadScene(level);
    }
    public void LoadScene(int level)
    {
        StartCoroutine(FadetoBlack(() => Application.LoadLevel(level)));

    }

    private IEnumerator FadetoBlack(Action levelMethod)
    {
        yield return new WaitForSeconds(Timer);
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
