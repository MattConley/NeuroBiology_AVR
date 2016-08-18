using UnityEngine;
using System.Collections;

public class PostiveIonTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider pIEnter)
    {
        switch (pIEnter.tag)
        {
            //THIS CASE IS FOR THE INSIDE THE MEMBRANE IN THE CABLE VIEW
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(1);
                ScoreTracker.UpdateOutsideDistribution(-1);
                //ChargedCloud.UpdateColorCloud(1);
                break;
            // THIS CASE IS FOR THE PERMEABLE MEMBRANE IN THE PATCH VIEW
            case "PermeableTrig":
               // pIEnter.enabled = false;
                ScoreTracker.UpdateInsideDistribution(-1);
                ScoreTracker.UpdateOutsideDistribution(1);
                ClearPlus.Opacity(0.25f, 10);
                ClearMinus.Opacity(0.25f, 10);
                break;
        }
    }
    void OnTriggerExit(Collider pIExit)
    {
        switch (pIExit.tag)
        {
            //THIS CASE IS FOR THE INSIDE THE MEMBRANE IN THE CABLE VIEW
            case "InnerMembrane":
                ScoreTracker.UpdateInsideDistribution(-1);
                ScoreTracker.UpdateOutsideDistribution(1);
                //ChargedCloud.UpdateColorCloud(-1);
                break;
            case "Boundary":
                GetComponentInParent<ParticleManager>().minusParticle(this.gameObject.GetComponent<MovingChargedParticle>());
                Destroy(this.gameObject);
                ScoreTracker.UpdateOutsideDistribution(-1);
                ScoreTracker.UpdateInsideDistribution(1);
                //gameObject.SetActive(false);
                //Destroy(gameObject);

                break;
            // THIS CASE IS FOR THE PERMEABLE MEMBRANE IN THE PATCH VIEW
            case "PermeableTrig":
                //pIExit.enabled = false;
                ScoreTracker.UpdateInsideDistribution(1);
                ScoreTracker.UpdateOutsideDistribution(-1);
                ClearPlus.Opacity(-0.25f, -10);
                ClearMinus.Opacity(-0.25f, -10);
                break;
        }
    }
}