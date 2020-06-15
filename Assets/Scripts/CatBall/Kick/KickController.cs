using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Util.Control;

namespace CatBall
{
    // todo(chris) disabled collision between the player and the ball for now as couldnt make it feel good.
    // come back to if you have time as double jumping by having it boost you up sound like fun
    // or maybe even just giving yourself some velocity in the reverse direction of your kick
    public class KickController : MonoBehaviour
    {
        [SerializeField] private ControlScheme controls;

        [SerializeField] private Transform bootzoneDetectionPos;
        [SerializeField] private float bootzoneRadius = 1f;
        [SerializeField] private LayerMask bootableLayers;
        [SerializeField] private bool drawGizmos = true;

        [Range(0, 1)]
        [SerializeField] private float reducedTimScale = .05f;
        [SerializeField] private float timeToMoveOut = .5f;

        [Space]

        [SerializeField] private float kickSpeed = 30f;
        [SerializeField] private float trailLength = 1.3f;

        [Space] [SerializeField] private GameObject kickEffect;

        [Space]

        [SerializeField] private UnityEvent onKick;
        [SerializeField] private UnityEvent onCanKick;
        [SerializeField] private UnityEvent onCanNotKick;


        private bool _canBoot;
        private bool _ballInZone;
        private GameObject _ball;
        private Vector3 _kickDir = Vector3.zero;
        private bool _kickNextUpdate;


        private LineRenderer _lines;

        private void Awake()
        {
            _lines = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            if (!bootzoneDetectionPos) bootzoneDetectionPos = transform;
        }

        private void Update()
        {
            if (!_canBoot) return;

            if (WasKickCancelPressed()) CantBoot();


            _kickDir = GetInputDirection();


            _lines.SetPosition(0, transform.position);
            var finalPos = transform.position + (trailLength * _kickDir);
            _lines.SetPosition(1, finalPos);


            if (WasKickButtonPressed())
            {
                // this will trigger the kick in Fixed Update
                _kickNextUpdate = true;
            }
        }


        private void FixedUpdate()
        {

            // there is a possibility that ball leaves just as we
            // hit kick (so _canBoot = false but _kickNext_update = true
            // in that case give player benefit of doubt and let them kick
            // Todo(chris) extend this iea to a timer similar to coyote time to give player more leeway
            if (_kickNextUpdate)
            {
                BootIt(_kickDir);

                _kickNextUpdate = false;
                _kickDir = Vector3.zero;
            }

            // needs to be done after the kick in case ball has juuust left the area as we hit kick
            DetectBall();
        }


        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bootzoneDetectionPos.position, bootzoneRadius);
        }

        private void DetectBall()
        {
            // detect bootability
            var bootable = Physics2D.OverlapCircle(bootzoneDetectionPos.position, bootzoneRadius, bootableLayers);

            var bootablePresent = bootable && bootable.CompareTag("Ball");

            if (_ballInZone && !bootablePresent)
            {
                // basically on trigger exit
                // todo(chris) delay this by a grace time
                CantBoot();
                // gameObject.layer = LayerMask.NameToLayer("Default");
                _ballInZone = false;
            } else if (!_ballInZone && bootablePresent)
            {
                // basically on trigger enter
                CanBoot(bootable);
                // gameObject.layer = LayerMask.NameToLayer("NoBallCollisions");
                _ballInZone = true;
            }
        }

        private void BootIt(Vector3 dir)
        {
            onKick.Invoke();

            Vector3 scalers = dir.normalized * kickSpeed;

            _ball.GetComponent<CatController>().Kick(scalers);

            var effectPos = transform.position + (_ball.transform.position - transform.position) / 2;
            Instantiate(kickEffect, effectPos, Quaternion.identity);

            CantBoot();
        }

        private void CanBoot(Collider2D other)
        {
            Time.timeScale = reducedTimScale;

            _ball = other.gameObject;
            _lines.enabled = true;

            _kickDir = GetInitialKickDir();
            _canBoot = true;
            onCanKick.Invoke();
        }


        private void CantBoot()
        {
            _canBoot = false;

            _ball = null;
            _lines.enabled = false;
            controls.kick.Handled();
            onCanNotKick.Invoke();
            StartCoroutine(TweenTimeScaleOut());
        }

        private IEnumerator TweenTimeScaleOut()
        {
            var start = Time.time;
            var end = start + timeToMoveOut;

            while (Time.time < end)
            {
                var t = (Time.time - start) / timeToMoveOut;
                Time.timeScale = Tween.SmoothStop5(reducedTimScale, 1f, t);
                yield return null;
            }

            Time.timeScale = 1f;
        }

        private Vector3 GetInitialKickDir()
        {
            var vec = _ball.transform.position - transform.position;
            vec.Normalize();

            return vec;
        }

        private bool WasKickButtonPressed()
        {
            return controls.kick.WasPressed();
        }

        private Vector3 GetInputDirection()
        {
            var dir = controls.kickAim.GetDirection(transform);

            if (dir == Vector2.zero) return _kickDir;
            else return dir;
        }

        private bool WasKickCancelPressed()
        {
            return controls.kickCancel.WasPressed();
        }
    }
}