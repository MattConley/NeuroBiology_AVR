using UnityEngine;
using System.Collections;

public class NegativeIonTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider pIEnter)
    {
        switch (pIEnter.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(-1);
                ScoreTracker.UpdateOutsideDistribution(1);
                //ChargedCloud.UpdateColorCloud(-1);
                break;
            case "PermeableTrig":
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,-10);
                //ScoreTracker.UpdateOutsideDistribution(100);
                break;
        }
    }
    void OnTriggerExit(Collider pIExit)
    {
        switch (pIExit.tag)
        {
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(1);
                ScoreTracker.UpdateOutsideDistribution(-1);
                //ChargedCloud.UpdateColorCloud(1);
                break;
            case "Boundary":
                Destroy(gameObject);
                break;
        }

    }
}
