using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    static Text insideText;
    static Text outsideText;
    static Text chargeDifference;
    static int currentinsideDistribution = 0;
    static int currentoutsideDistribution = 0;
    static int currentchargeDifference = currentoutsideDistribution - currentinsideDistribution;
    // Use this for initialization
    void Start()
    {
        insideText = GameObject.FindGameObjectWithTag("InsideDistribution").GetComponent<Text>();
        outsideText = GameObject.FindGameObjectWithTag("OutsideDistribution").GetComponent<Text>();
        chargeDifference = GameObject.FindGameObjectWithTag("DrivingForce").GetComponent<Text>();
        UpdateInsideDistribution(currentinsideDistribution);
        UpdateOutsideDistribution(currentoutsideDistribution);
        UpdateChargeDifference(currentchargeDifference);
    }

    // Update is called once per frame
    public static void UpdateInsideDistribution(int addedValue)
    {
        currentinsideDistribution += addedValue;
        insideText.text = "" + currentinsideDistribution;
        UpdateChargeDifference(0);
    }
    public static void UpdateOutsideDistribution(int addedValue)
    {
        currentoutsideDistribution += addedValue;
        outsideText.text = "" + currentoutsideDistribution;
        UpdateChargeDifference(0);
    }
    public static void UpdateChargeDifference(int addedValue)
    {
        currentchargeDifference = currentoutsideDistribution - currentinsideDistribution - addedValue;
        chargeDifference.text = "" + currentchargeDifference;
    }
}
