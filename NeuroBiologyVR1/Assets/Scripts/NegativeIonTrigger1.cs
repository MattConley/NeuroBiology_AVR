using UnityEngine;
using System.Collections;

public class NegativeIonTrigger1 : MonoBehaviour {

    void OnTriggerExit(Collider pIExit)
    {
        switch (pIExit.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(1);
                ScoreTracker.UpdateOutsideDistribution(-1);
                ChargedCloud.UpdateColorCloud(1);
                break;
        }

    }
    void OnTriggerEnter(Collider pIEnter)
    {
        switch (pIEnter.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(-1);
                ScoreTracker.UpdateOutsideDistribution(1);
                ChargedCloud.UpdateColorCloud(-1);
                break;
        }
    }
}
