using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Damagable")
        Debug.Log("Collision detected! destroying..");
        Destroy(this.gameObject);
    }
}
