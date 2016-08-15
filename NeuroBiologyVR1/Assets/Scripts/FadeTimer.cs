using UnityEngine;
using System.Collections;

public class FadeTimer : MonoBehaviour {
    public GUITexture Top;
    public GUITexture Bottom;
    public GUIText Title;
    public GUIText Text1;
    public GUIText Text2;
    public GUIText Text3;
    public int Interval;

    public float fadeTime;
    // Use this for initialization
    void Awake()
    {
        //fade to clear
        StartCoroutine(FadetoClear());
    }
    private IEnumerator FadetoClear()
    {
        Top.gameObject.SetActive(true);
        Bottom.color = Color.black;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;

        yield return new WaitForSeconds(Interval);

        while (progress < 1.0f)
        {
            Top.color = Color.Lerp(Color.black, Color.clear, progress);
            Bottom.color = Color.Lerp(Color.black, Color.clear, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Top.color = Color.clear;
        Bottom.color = Color.clear;
        Top.gameObject.SetActive(false);
        Bottom.gameObject.SetActive(false);
    }
}
