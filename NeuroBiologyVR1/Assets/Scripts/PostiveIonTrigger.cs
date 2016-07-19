using UnityEngine;
using System.Collections;

public class PostiveIonTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider pIEnter)
    {
        switch (pIEnter.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(1);
                ScoreTracker.UpdateOutsideDistribution(-1);
                ChargedCloud.UpdateColorCloud(1);
                break;
        }
    }
    void OnTriggerExit(Collider pIExit)
    {
        switch (pIExit.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(-1);
                ScoreTracker.UpdateOutsideDistribution(1);
                ChargedCloud.UpdateColorCloud(-1);
                break;
            case "Boundary":
                Destroy(gameObject);
                break;
        }
    }

}