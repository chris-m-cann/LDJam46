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
            var sqrMagnitude = _rb.velocity.sqrMagnitude;

            if (sqrMagnitude > (maxSpeed * maxSpeed))
            {
                _rb.velocity = _rb.velocity.normalized * maxSpeed;
            }

            var v = _rb.velocity;
            if (_rb.velocity.x < 0.001 && _rb.velocity.x > -0.001) v.x = 0;
            if (_rb.velocity.y < 0.001 && _rb.velocity.y > -0.001) v.y = 0;

            _rb.velocity = v;
        }
    }
}