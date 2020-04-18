using System;
using UnityEngine;

namespace CatBall
{
    public class ClampVelocity : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var mag = _rb.velocity.magnitude;
            if (mag == 0f) return;

            var clamped = Mathf.Clamp(mag, -maxSpeed, maxSpeed);

            var scale = clamped / mag;
            _rb.velocity *= scale;
        }
    }
}