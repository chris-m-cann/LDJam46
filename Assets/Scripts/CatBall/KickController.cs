using System;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace CatBall
{
    // todo(chris) disabled collision between the player and the ball for now as couldnt make it feel good.
    // come back to if you have time as double jumping by having it boost you up sound like fun
    // or maybe even just giving yourself some velocity in the reverse direction of your kick
    public class KickController : MonoBehaviour
    {
        [SerializeField] private Transform bootzoneDetectionPos;
        [SerializeField] private float bootzoneRadius = 1f;
        [SerializeField] private LayerMask bootableLayers;
        [SerializeField] private bool drawGizmos = true;

        [Space]
        [SerializeField] private bool invertDirection = true;

        [Space]
        [SerializeField] private float maxKickedSpeed = 30f;
        [Range(0, 1)]
        [SerializeField] private float reducedTimScale = .05f;

        [Space] [SerializeField] private float minKickVelocity = 10f;
        [Space] [SerializeField] private float maxKickVelocity = 10f;
        [SerializeField] private float maxKickTime = 2f;
        [SerializeField] private float maxTrailLength = 2f;

        [SerializeField] private UnityEvent onKick;


        private bool _canBoot;
        private bool _ballInZone;
        private GameObject _ball;
        private Vector3 _mousePosOnEntry;
        private Vector3 _mousePosOnEntryWorldPos;
        private float _kickButtonDownTime = -1;
        private Vector3 _kickDir = Vector3.zero;
        private float _kickScale = -1f;


        private LineRenderer _lines;
        private Camera cam;

        private void Awake()
        {
            _lines = GetComponent<LineRenderer>();
            cam = Camera.main;
            // in case mid boot-zone
            Time.timeScale = 1;
        }

        private void Start()
        {
            if (!bootzoneDetectionPos) bootzoneDetectionPos = transform;
        }

        private void Update()
        {
            if (!_canBoot) return;

            if (_kickButtonDownTime < 0 && KickButtonIsDown())
            {
                _kickButtonDownTime = Time.unscaledTime;
            }


            var dir = GetInputDirection();

            // get the length of the trail to put on screen
            float timePressed = 0;
            if (_kickButtonDownTime > 0)
            {
                timePressed = Time.unscaledTime - _kickButtonDownTime;
            }

            // in range 0-1: how much of the kick we have charged
            // todo(chris) consider using a different algorithm here to get y for diferent ramp up curves
            // this is y = mx where m = 1/maxKick, could be y = mx^2 etc
            var timeScaler = timePressed / maxKickTime;


            _lines.SetPosition(0, transform.position);
            var finalPos = transform.position + (timeScaler * maxTrailLength * dir);
            _lines.SetPosition(1, finalPos);


            if (KickButtonReleased())
            {
                // this will trigger the kick in Fixed Update
                _kickDir = dir;
                _kickScale = timeScaler;
            }
        }


        private void FixedUpdate()
        {
            DetectBall();

            if (_canBoot && _kickScale > 0)
            {
                BootIt(_kickDir, _kickScale);

                _kickScale = -1f;
                _kickDir = Vector3.zero;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Ball")) return;
            //
            // var kickTime = minKickTime;
            // // if (_kickButtonDownTime > 0)
            // // {
            // //     kickTime = Time.unscaledTime - _kickButtonDownTime;
            // // }
            //
            // var kickScale = kickTime / maxKickTime;
            //
            // // todo(chris) why do i need to do this?
            // _ball = other.gameObject;
            //
            // BootIt(GetInputDirection(), kickScale);
        }

        private void OnDrawGizmosSelected()
        {
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

        private void BootIt(Vector3 dir, float velocityScale)
        {
            // var dir = Input.mousePosition - _mousePosOnEntry;
            // dir.z = 0;

            // var dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), transform.position.z);
            // if (invertDirection) dir *= -1;

            onKick.Invoke();

            Debug.DrawRay(transform.position, -dir.normalized * 2, Color.red, 1f);

            var vToAdd = Mathf.Lerp(minKickVelocity, maxKickVelocity, velocityScale);

            Vector2 scalers = -dir.normalized * vToAdd;

            _ball.GetComponent<Rigidbody2D>().velocity = scalers;

            // todo(chris) grab this when we grab the ball
            // _ball.GetComponent<Rigidbody2D>().velocity = new Vector2(scalers.x + vel.x, scalers.y + vel.y);
            CantBoot();
        }

        private void CanBoot(Collider2D other)
        {
            Time.timeScale = reducedTimScale;

            _mousePosOnEntry = Input.mousePosition;
            _mousePosOnEntryWorldPos = cam.ScreenToWorldPoint(_mousePosOnEntry);
            _mousePosOnEntryWorldPos.z = transform.position.z;

            _ball = other.gameObject;
            _lines.enabled = true;

            if (KickButtonIsDown())
            {
                _kickButtonDownTime = Time.unscaledTime;
            }

            _canBoot = true;
        }


        private void CantBoot()
        {
            _canBoot = false;

            _ball = null;
            Time.timeScale = 1f;
            _lines.enabled = false;
            _kickButtonDownTime = -1f;
        }

        private bool KickButtonIsDown()
        {
            return Input.GetButton("Fire1");
        }

        private bool KickButtonReleased()
        {
            return Input.GetButtonUp("Fire1");
        }
        private Vector3 GetInputDirection()
        {
            Vector3 dir;

            if (InputEx.IsControllerConnected())
            {
                dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), transform.position.z).normalized;
            }
            else
            {
                // var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                // pos.z = transform.position.z;
                // dir = pos - _mousePosOnEntryWorldPos;
                // dir.Normalize();

                var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                pos.z = transform.position.z;
                dir = pos - transform.position;
                dir.Normalize();
            }


            if (invertDirection) dir *= -1;
            return dir;
        }
    }
}