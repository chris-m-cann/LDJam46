using System;
using UnityEngine;

namespace CatBall
{
    public class NeutralZone : MonoBehaviour
    {
        private GameObject _ball;
        private Rigidbody2D _ballRb;
        [SerializeField] private float snapDistance;
        [SerializeField] private float floatVelocity;
        [SerializeField] private float snapVelocity;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{other.gameObject.name} entered the neutral zone");
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                _ballRb = _ball.GetComponent<Rigidbody2D>();
                _ballRb.velocity = (transform.position - _ball.transform.position).normalized * floatVelocity;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"{other.gameObject.name} left the neutral zone");
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = null;

            }
        }

        private void FixedUpdate()
        {
            if (!_ball) return;

            var distance = Vector3.Distance(transform.position, _ball.transform.position);
            var withinRange = Vector3.Distance(transform.position, _ball.transform.position) < snapDistance;
            var slowEnough = _ballRb.velocity.sqrMagnitude < (snapVelocity * snapVelocity);

            Debug.Log($"in range = {withinRange}, slow enough = {slowEnough} distance = {distance}");
            if (withinRange && slowEnough)
            {
                _ballRb.velocity = Vector2.zero;
                _ball.transform.position = transform.position;
            }
        }
    }
}