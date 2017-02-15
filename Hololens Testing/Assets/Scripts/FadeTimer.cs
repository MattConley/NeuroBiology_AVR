using UnityEngine;
using System.Collections;

public class FadeTimer : MonoBehaviour {

    public TextMesh Title;
    public TextMesh Text1;
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
        Title.gameObject.SetActive(true);
        Text1.gameObject.SetActive(true);
        Title.color = Color.white;
        Text1.color = Color.white;

        float rate = 1.0f / fadeTime;

        float progress = 0.0f;

        

        while (progress < 1.0f)
        {
            Title.color = Color.Lerp(Color.white, Color.clear, progress);
            Text1.color = Color.Lerp(Color.white, Color.clear, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(Interval);

        Title.color = Color.clear;
        Text1.color = Color.clear;
    }
}
