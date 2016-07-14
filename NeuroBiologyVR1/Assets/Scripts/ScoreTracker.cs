using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {
    static Text insideText;
    static Text outsideText;
    static int currentinsideDistribution = 0;
    static int currentoutsideDistribution = 0;

    // Use this for initialization
    void Start () {
        insideText = GameObject.FindGameObjectWithTag("InsideDistribution").GetComponent<Text>();
        outsideText = GameObject.FindGameObjectWithTag("OutsideDistribution").GetComponent<Text>();
        UpdateInsideDistribution(currentinsideDistribution);
        UpdateOutsideDistribution(currentoutsideDistribution);
    }
	
	// Update is called once per frame
	public static void UpdateInsideDistribution(int addedValue)
    {
        currentinsideDistribution += addedValue;
        insideText.text = "" + currentinsideDistribution;
    }
    public static void UpdateOutsideDistribution(int addedValue)
    {
        currentoutsideDistribution += addedValue;
        outsideText.text = "" + currentoutsideDistribution;
    }
}
