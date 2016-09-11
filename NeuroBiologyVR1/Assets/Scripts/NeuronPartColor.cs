using UnityEngine;
using System.Collections;

public class NeuronPartColor : MonoBehaviour {

    public Renderer NeuronPartA;
    public Renderer NeuronPartB;
    public float Interval;
    public float delay;
    public float wait;
    public int number;
    private static Color color;

    public float fadeTime;
    // Use this for initialization
    void Start()
    {
        color = NeuronPartA.material.color;
    }
    void Awake()
    {
        //fade to clear
        
        StartCoroutine(FadetoClear());
    }
    private IEnumerator FadetoClear()
    {
        int count = 0;
        yield return new WaitForSeconds(delay);
        while (count<number)
        {
            NeuronPartA.material.color = color;
            NeuronPartB.material.color = color;
            //Color red = new Color(225, 0, 0, .25f);
            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < .5f)
            {
                NeuronPartA.material.color = Color.Lerp(color, Color.red, progress);
                NeuronPartB.material.color = Color.Lerp(color, Color.red, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            rate = 1.0f / fadeTime;

            progress = 0.0f;

            yield return new WaitForSeconds(Interval);

            while (progress < .5f)
            {
                NeuronPartA.material.color = Color.Lerp(Color.red, color, progress);
                NeuronPartB.material.color = Color.Lerp(Color.red, color, progress);
                progress += rate * Time.deltaTime;
                NeuronPartA.material.color = color;
                NeuronPartB.material.color = color;
                yield return null;
            }
            yield return new WaitForSeconds(wait);
            
            count++;
        }
    }
}
