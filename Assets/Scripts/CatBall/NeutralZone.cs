using System;
using UnityEngine;

namespace CatBall
{
    public class NeutralZone : MonoBehaviour
    {
        [SerializeField] private bool disableOnBallExit;
        [SerializeField] private float snapDistance;
        [SerializeField] private float floatVelocity;
        [SerializeField] private float snapVelocity;
        private GameObject _ball;
        private Rigidbody2D _ballRb;

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
                if (disableOnBallExit) gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (!_ball) return;

            var distance = Vector3.Distance(transform.position, _ball.transform.position);
            var withinRange = Vector3.Distance(transform.position, _ball.transform.position) < snapDistance;
            var slowEnough = _ballRb.velocity.sqrMagnitude < (snapVelocity * snapVelocity);

            if (withinRange && slowEnough)
            {
                _ballRb.velocity = Vector2.zero;
                _ball.transform.position = transform.position;
            }
        }
    }
}