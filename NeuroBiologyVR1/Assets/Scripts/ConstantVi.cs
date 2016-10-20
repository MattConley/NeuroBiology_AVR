using UnityEngine;
using System.Collections;

public class ConstantVi : MonoBehaviour {
    public int x;
    public int y;
    public int z;
    static Rigidbody Ion;
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(x, y, z);
    }
}
