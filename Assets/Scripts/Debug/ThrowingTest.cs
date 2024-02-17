using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingTest : MonoBehaviour
{
    public Vector3 Force;
    public Vector3 RotateForce;
    public void Awake()
    {
        StartCoroutine(Throw());
    }
    public IEnumerator Throw()
    {
        yield return new WaitForSeconds(3);
        GetComponent<Rigidbody>().AddForce(Force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(RotateForce, ForceMode.Impulse);
    }
}
