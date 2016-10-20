using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class PlayMovie : MonoBehaviour {
    public int delay;
    public MovieTexture movie;
    //private AudioSource audio;
	// Use this for initialization
	void Start () {
        StartCoroutine(Pause());

    }

    // Update is called once per frame
    public IEnumerator Pause()
    { 
        GetComponent<RawImage>().texture = movie as MovieTexture;
        //audio = GetComponent<AudioSource>();
        //audio.clip = movie.audioClip;
        movie.loop = true;
        yield return new WaitForSeconds(delay);
        movie.Play();
        //audio.Play();
        movie.loop = true;
    }

}
