using System;
using UnityEngine;

namespace CatBall
{
    public class HurtThings : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{gameObject.name} hit spikes");
            var hurtable = other.GetComponent<IHurtable>();
            if (hurtable != null)
            {
                Debug.Log($"{gameObject.name} gets hurt");
                hurtable.Hurt();
            }
        }
    }
}