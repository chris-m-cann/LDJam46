using System;
using UnityEngine;

namespace CatBall
{
    public class NeutralZone : MonoBehaviour
    {
        [SerializeField] private float snapDistance;
        [SerializeField] private float floatVelocity;
        [SerializeField] private float snapVelocity;
        private GameObject _ball;
        private Rigidbody2D _ballRb;
        // here still as we may want to make some that dont but untill we have the time to make them visually distinct then
        // better to make them all consistant
        private bool _disableOnBallExit = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                _ballRb = _ball.GetComponent<Rigidbody2D>();
                _ballRb.velocity = (transform.position - _ball.transform.position).normalized * floatVelocity;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = null;
                _ballRb = null;
                other.gameObject.transform.parent = null;
                if (_disableOnBallExit) gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (!_ball) return;

            var distance = Vector3.Distance(transform.position, _ball.transform.position);
            var withinRange = distance < snapDistance;
            var sqrMag = _ballRb.velocity.sqrMagnitude;
            var slowEnough = sqrMag < (snapVelocity * snapVelocity);

            if (sqrMag  - (0.001f) <= floatVelocity * floatVelocity)
            {

                if (withinRange && slowEnough)
                {
                    _ballRb.velocity = Vector2.zero;
                    _ball.transform.position = transform.position;
                    _ball.transform.parent = transform;
                }
                else
                {
                    _ballRb.velocity = (transform.position - _ball.transform.position).normalized * floatVelocity;
                }
            }
        }
    }
}