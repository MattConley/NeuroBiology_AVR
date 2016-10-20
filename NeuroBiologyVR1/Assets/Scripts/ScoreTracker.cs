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
    static int currentPercent = 100;
    static int hundred = 100;
    static Text Prob;
    static Text Perc;
    static int currentProb = 0;
    static int currentPerc = 100;
    // Use this for initialization
    void Start()
    {
        insideText = GameObject.FindGameObjectWithTag("InsideDistribution").GetComponent<Text>();
        outsideText = GameObject.FindGameObjectWithTag("OutsideDistribution").GetComponent<Text>();
        chargeDifference = GameObject.FindGameObjectWithTag("DrivingForce").GetComponent<Text>();
        UpdateInsideDistribution(currentinsideDistribution);
        UpdateOutsideDistribution(currentoutsideDistribution);
        UpdateChargeDifference(currentchargeDifference);


        Prob = GameObject.FindGameObjectWithTag("Prob").GetComponent<Text>();
        Perc = GameObject.FindGameObjectWithTag("Perc").GetComponent<Text>();
        UpdateProb(currentProb);
        UpdatePerc(currentPerc);
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
        currentchargeDifference = currentinsideDistribution - currentoutsideDistribution - addedValue;
        chargeDifference.text = "" + currentchargeDifference;
    }
    public static void UpdateProb(int addedValue)
    {
        currentProb = addedValue;
        Prob.text = "" + currentProb;

    }
    public static void UpdatePerc(int addedValue)
    {
        currentPerc = addedValue;
        Perc.text = "" + currentPerc;

    }
    public static int CalcProbability(int percent)
    {
        int Probability = Random.Range(0, hundred);

        if (Probability < currentPercent && Probability > 0)
        {
            currentPercent += percent;
            UpdateProb(Probability);
            UpdatePerc(currentPercent);
            return 1;
        }
        else
        {
            UpdateProb(Probability);
            UpdatePerc(currentPercent);
            return 0;
        }
    }
    public static void IncreasePercent(int percent, int hun)
    {
        hundred += hun;
        currentPercent += percent;
        UpdatePerc(currentPercent);
    }

    public static void Reset(int zero)
    {
        currentinsideDistribution = 0;
        UpdateInsideDistribution(currentinsideDistribution);
        currentchargeDifference = 0;
        UpdateChargeDifference(currentchargeDifference);
        currentoutsideDistribution = 0;
        UpdateOutsideDistribution(currentoutsideDistribution);
        currentPercent = 100;
        hundred = 100;
        currentPerc = 100;
        currentProb = 0;
        UpdateProb(currentProb);
        UpdatePerc(currentPerc);

    }
}
