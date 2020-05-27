using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Util;

namespace CatBall
{
    public class CatController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject onKickEffect;
        [SerializeField] private GameObject regularTrail;
        [SerializeField] private GameObject kickedTrail;
        [SerializeField] private float trailTime;
        [SerializeField] private float trailFadeTime = .5f;

        private Rigidbody2D _rigidbody;
        private CatControllerState _state;
        private Color _kickTrailStartColour;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Kicked = Animator.StringToHash("Kicked");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _state = new IdleState();
            var particles = kickedTrail.GetComponent<ParticleSystem>();
            _kickTrailStartColour = particles.main.startColor.color;
        }


        private void FixedUpdate()
        {
            var v = _rigidbody.velocity.normalized;
            animator.SetFloat(Horizontal, v.x);
            animator.SetFloat(Vertical, v.y);
            _state.OnFixedUpdate(this);
        }

        public void Kick(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
            Instantiate(onKickEffect, transform.position, Quaternion.identity);

            ChangeState(new KickedState());
            animator.SetTrigger("Kick");
        }

        private void ChangeState(CatControllerState newState)
        {
            _state.OnStateExit(this);
            _state = newState;
            _state.OnStateEnter(this);
        }

        private abstract class CatControllerState
        {
            public abstract void OnFixedUpdate(CatController controller);
            public virtual void OnStateEnter(CatController controller){}
            public virtual void OnStateExit(CatController controller){}
        }

        private class IdleState : CatControllerState
        {
            public override void OnFixedUpdate(CatController controller)
            {
                if (controller._rigidbody.velocity.sqrMagnitude > .1f)
                {
                    controller.ChangeState(new MovingState());
                }
            }
        }

        private class KickedState : CatControllerState
        {
            public override void OnStateEnter(CatController controller)
            {
                controller.animator.SetBool(Kicked, true);
                controller.StopAllCoroutines();


                var particles = controller.kickedTrail.GetComponent<ParticleSystem>();
                var main = particles.main;
                var colour = main.startColor;
                colour.color = controller._kickTrailStartColour;
                main.startColor = colour;

                controller.kickedTrail.SetActive(true);
                controller.StartCoroutine(DisableKicked(controller));
            }

            public override void OnFixedUpdate(CatController controller)
            {
                if (controller._rigidbody.velocity.sqrMagnitude < .1f)
                {
                    controller.ChangeState(new IdleState());
                }
            }

            public override void OnStateExit(CatController controller)
            {
                controller.animator.SetBool(Kicked, false);
                controller.StopAllCoroutines();
                controller.StartCoroutine(FadeOutTrail(controller));

            }

            private IEnumerator DisableKicked(CatController controller)
            {
                yield return new WaitForSeconds(controller.trailTime);
                controller.ChangeState(new MovingState());
            }
            private IEnumerator FadeOutTrail(CatController controller)
            {
                var start = Time.time;
                var end = start + controller.trailFadeTime;
                var particles = controller.kickedTrail.GetComponent<ParticleSystem>();
                var main = particles.main;
                var colourGradient = main.startColor;
                var startColour = colourGradient.color;

                while (Time.time < end)
                {
                    var t = (Time.time - start) / controller.trailFadeTime;
                    var v = Tween.SmoothStop3(1, 0, t);
                    var alpha = startColour.a * v;
                    var colour = startColour;
                    colour.a = alpha;
                    colourGradient.color = colour;
                    main.startColor = colourGradient;
                    yield return null;
                }

                controller.kickedTrail.SetActive(false);
                colourGradient.color = startColour;
                main.startColor = colourGradient;
            }

        }

        private class MovingState : CatControllerState
        {
            public override void OnStateEnter(CatController controller)
            {
                controller.regularTrail.SetActive(true);
            }

            public override void OnFixedUpdate(CatController controller)
            {
                if (controller._rigidbody.velocity.sqrMagnitude < .1f)
                {
                    controller.ChangeState(new IdleState());
                }
            }

            public override void OnStateExit(CatController controller)
            {
                controller.regularTrail.SetActive(false);
            }
        }
    }

}

