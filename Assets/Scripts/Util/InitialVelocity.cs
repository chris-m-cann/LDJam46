using System;
using UnityEngine;

namespace Util
{
    public class InitialVelocity : MonoBehaviour
    {
        [SerializeField] private Vector3 velocity;

        private void Start()
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = velocity;
            }
        }
    }
}