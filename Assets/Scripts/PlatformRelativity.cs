using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRelativity : MonoBehaviour
{
    public void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(transform, true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
