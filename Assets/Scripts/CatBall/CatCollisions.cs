using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCollisions : MonoBehaviour
{
    [SerializeField] private GameObject dustCloud;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.DrawRay(other.contacts[0].point, other.contacts[0].normal, Color.red, .5f);
        var cloud = Instantiate(dustCloud, other.contacts[0].point, Quaternion.identity);
         var angle = Vector2.SignedAngle(Vector2.up, other.contacts[0].normal);
         cloud.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        //cloud.transform.rotation = Quaternion.FromToRotation(transform.up, other.contacts[0].normal);
    }
}
