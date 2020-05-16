using System;
using UnityEngine;
using UnityEngine.Events;

namespace CatBall.Player
{
    public class PlayerAnimatorParameters : MonoBehaviour
    {

        [SerializeField] private UnityEvent onJump;
        [SerializeField] private UnityEvent onKick;
        [SerializeField] private UnityEvent onLand;

        private Animator _animator;
        private PlatformerController _controller;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _sprite;


        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Kick = Animator.StringToHash("Kick");
        private static readonly int IsOnAWall = Animator.StringToHash("IsOnAWall");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _controller = GetComponent<PlatformerController>();
        }

        private void FixedUpdate()
        {
            var vel = _rigidbody.velocity.sqrMagnitude < .1f ? Vector2.zero : _rigidbody.velocity.normalized;
            _animator.SetFloat(Horizontal, vel.x);
            _animator.SetFloat(Vertical, vel.y);

            _sprite.flipX = vel.x < 0;

            _animator.SetBool(IsOnAWall, _controller.IsOnAWall);
            _animator.SetBool(IsGrounded, _controller.IsGrounded);
        }


        // callbacks to wire up setting triggers on the animator controller
        public void OnJump() => _animator.SetTrigger(Jump);
        public void OnKick() => _animator.SetTrigger(Kick);

        // callbacks to be triggered from animations
        public void OnJumpAnimation() => onJump.Invoke();
        public void OnKickAnimation() => onKick.Invoke();
        public void OnLandAnimation() => onLand.Invoke();
    }
}