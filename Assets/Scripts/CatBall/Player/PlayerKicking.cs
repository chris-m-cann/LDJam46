using UnityEngine;

namespace CatBall.Player
{
    public class PlayerKicking : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.SetBool("DoneKicking", false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            animator.SetBool("DoneKicking", true);
        }

    }
}