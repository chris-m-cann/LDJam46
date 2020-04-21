using UnityEngine;

namespace CatBall.Player
{
    public class PlayerFalling : StateMachineBehaviour
    {
        [SerializeField] private bool isWallSliding;

        private Rigidbody2D _rb;
        private PlatformerController _controller;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _rb = animator.GetComponent<Rigidbody2D>();
            _controller = animator.GetComponent<PlatformerController>();

            if (isWallSliding) animator.GetComponent<PlatformerSoundController>()?.PlayWallSlide();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if (_controller.IsGrounded) animator.SetTrigger("Land");
            // if (Mathf.Approximately(_rb.velocity.y, 0f)) animator.SetTrigger("Land");
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}