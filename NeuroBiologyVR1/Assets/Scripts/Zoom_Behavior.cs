using UnityEngine;
using System.Collections;
using System;
public class Zoom_Behavior : MonoBehaviour {

    public ParticleSystem part_pipette;
    private bool isEnabled;
    public int scene;
    public GUITexture overlay;
    public float fadeTime;
    void Awake()
    {
        overlay.pixelInset = new Rect(0, 0, Screen.width, Screen.height);

        //fade to clear
        StartCoroutine(FadetoClear());
    }

    // Use this for initialization
    void Start () {
        isEnabled = true;
	}

    void OnMouseDown()
    {
        if (isEnabled)
        {
            LoadScene(scene);
        }
    }

    public void LoadScene(int level)
    {
        StartCoroutine(FadetoWhite(() => Application.LoadLevel(level)));
    }

    private IEnumerator FadetoClear()
    {
        overlay.gameObject.SetActive(true);
        overlay.color = Color.black;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;
        
        while (progress < 1.0f)
        {
            overlay.color = Color.Lerp(Color.black, Color.clear, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
        
        overlay.color = Color.clear;
        overlay.gameObject.SetActive(false);
    }
    private IEnumerator FadetoWhite(Action levelMethod)
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
