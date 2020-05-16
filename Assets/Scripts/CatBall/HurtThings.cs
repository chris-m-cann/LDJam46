using System;
using UnityEngine;

namespace CatBall
{
    public class HurtThings : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var hurtable = other.GetComponent<IHurtable>();
            if (hurtable != null)
            {
                hurtable.Hurt();
            }
        }
    }
}