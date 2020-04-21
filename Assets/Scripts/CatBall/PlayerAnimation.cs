using System;
using UnityEngine;

namespace CatBall
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rb;
        private SpriteRenderer _sprite;
        private PlatformerController _controller;

        private Vector2 _lastV = Vector2.zero;

        private void Awake()
        {
            _controller = GetComponent<PlatformerController>();
            _sprite = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            var vx = _rb.velocity.x;
            var vy = _rb.velocity.y;

            _animator.SetFloat("Horizontal", Mathf.Abs(vx));
            _animator.SetFloat("Vertical", vy);
            _animator.SetBool("WallSliding", _controller.IsOnAWall);

            // if (_lastV.y < -0.05 && Mathf.Approximately(vy, 0f) && _controller.IsGrounded)
            // {
            //     _animator.SetTrigger("Land");
            // }
            _sprite.flipX = (vx < 0);

            _lastV = _rb.velocity;
        }

        public void Jump()
        {
            _animator.SetTrigger("Jump");
        }

        public void Kick()
        {
            _animator.SetTrigger("Kick");
        }
    }
}