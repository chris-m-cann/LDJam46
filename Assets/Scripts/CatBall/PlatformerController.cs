using UnityEngine;

namespace CatBall
{
    public class PlatformerController : MonoBehaviour
    {
        [SerializeField] private PlatformerControlParameters param;

        [SerializeField] private Transform groundedPos;
        [SerializeField] private float groundedWidth = .2f;
        [SerializeField] private float groundedHeight = .2f;


        [SerializeField] private Transform rightWallCheckPos;
        [SerializeField] private float rightWallCheckWidth = .2f;
        [SerializeField] private float rightWallCheckHeight = .2f;

        [SerializeField] private Transform leftWallCheckPos;
        [SerializeField] private float leftWallCheckWidth = .2f;
        [SerializeField] private float leftWallCheckHeight = .2f;

        private float _gravityScaleUp = 1f;
        private float _gravityScaleDown = 1f;
        private float _vyzero;

        private float _lastHorizontal;
        private float _horizontal;
        private float _leftPressTime = -1f;
        private float _rightPressTime = -1f;
        private float _horzontalReleasedTime = -1f;
        private float _fromV;
        private float _maxSpeed;
        private float tweenTime;


        private bool _jumpPressed;
        private bool _jumpReleased;
        private float _jumpStartTime;
        private bool _isGrounded;
        private bool _isOnLeftWall;
        private bool _isOnRightWall;
        private float _lastPress;
        private float _lastGrounded;
        private float _groundedAcceleration;
        private float _groundedDeceleration;
        private float _inAirAcceleration;
        private float _inAirDeceleration;


        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            param.onUpdate = Setup;
            Setup();

            _rb.gravityScale = _gravityScaleUp;
        }

        private void OnValidate()
        {
            if (param == null) return;
            Setup();
        }

        private void Setup()
        {
            // here is where we will need to do all the maths to figure out our V0 and g for our jump, also the damping and acceleration factors
            _vyzero = 2 * param.maxJumpHeight / param.timeToPeak;

            var gup = - 2 * param.maxJumpHeight / (param.timeToPeak * param.timeToPeak);
            _gravityScaleUp = gup / Physics2D.gravity.y;


            var gdown = - 2 * param.maxJumpHeight / (param.timeBackDown * param.timeBackDown);
            _gravityScaleDown = gdown / Physics2D.gravity.y;

            _lastGrounded = float.MinValue;
            _lastPress = float.MinValue;

            _maxSpeed = param.maxJumpWidth / (param.timeToPeak + param.timeBackDown);

            _groundedAcceleration = _maxSpeed / param.timeToMaxSpeed;
            _groundedDeceleration = _maxSpeed / param.timeBackFromMaxToRest;

            _inAirAcceleration = _maxSpeed / param.timeToMaxSpeedInAir;
            _inAirDeceleration = _maxSpeed / param.timeToBackFromMaxSpeedInAir;

        }

        private void Update()
        {
            var lastHorizontal = _horizontal;
            var wasZero = Mathf.Approximately(lastHorizontal, 0f);

            _horizontal = Input.GetAxisRaw("Horizontal");

            if (!wasZero && Mathf.Approximately(_horizontal, 0f))
            {
                _leftPressTime = -1f;
                _rightPressTime = -1f;
                _horzontalReleasedTime = Time.time;
                _fromV = _rb.velocity.x;
            } else if (_horizontal > float.Epsilon)
            {
                if (wasZero || lastHorizontal < -float.Epsilon)
                {
                    _leftPressTime = -1f;
                    _rightPressTime = Time.time;
                    _horzontalReleasedTime = -1f;
                    _fromV = _rb.velocity.x;
                }
            } else if (_horizontal < -float.Epsilon)
            {
                if (wasZero || lastHorizontal > float.Epsilon)
                {
                    _leftPressTime = Time.time;
                    _rightPressTime = -1f;
                    _horzontalReleasedTime = -1f;
                    _fromV = _rb.velocity.x;
                }
            }


            if (Input.GetButtonDown("Jump"))
            {
                _jumpPressed = true;
                _lastPress = Time.time;
            }
            if (Input.GetButtonUp("Jump")) _jumpReleased = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (groundedPos != null)
                Gizmos.DrawWireCube(groundedPos.position, new Vector3(groundedWidth, groundedHeight, .1f));
            if (rightWallCheckPos != null)
                Gizmos.DrawWireCube(rightWallCheckPos.position, new Vector3(rightWallCheckWidth, rightWallCheckHeight, .1f));
            if (leftWallCheckPos != null)
                Gizmos.DrawWireCube(leftWallCheckPos.position, new Vector3(leftWallCheckWidth, leftWallCheckHeight, .1f));
        }

        private void FixedUpdate()
        {
            DetectWhatImTouching();
            if (_isGrounded) _lastGrounded = Time.time;


            var vx = HorizontalMovement();
            var vy =  HandleJumpPress();


            // these stop the player ticking on walls when pressing in to them
            if (_isOnRightWall && vx > 0) vx = _rb.velocity.x;
            if (_isOnLeftWall && vx < 0) vx = _rb.velocity.x;


            vy = Mathf.Max(vy, -param.maxFallSpeed);
            _rb.velocity = new Vector2(vx, vy);

            HandleJumpButtonRelease();


            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _gravityScaleDown;
            }

        }

        private float HandleJumpPress()
        {
            var yvel = _rb.velocity.y;

            var inGraceTime = _isGrounded && (Time.time - _lastPress) < param.graceTime;
            var inCoyoteTime = _jumpPressed && (Time.time - _lastGrounded) < param.coyoteTime;
            if (inGraceTime || inCoyoteTime)
            {
                //Debug.Log($"jumping because inGraceTime={inGraceTime}, inCoyoteTime={inCoyoteTime}");
                _rb.gravityScale = _gravityScaleUp;
                _jumpStartTime = Time.time;
                _lastPress = 0;
                yvel = _vyzero;
            }

            _jumpPressed = false;

            return yvel;
        }

        private void HandleJumpButtonRelease()
        {
            // variable height jumping (I dont think this works with things adding exterior forces like springs and moving platforms)
            // this is basically saying, if we release the jump button while jumping and travelling up
            // then using our initail jump velocity and time since our jump we can work out where we are on the parabola
            if (_jumpReleased && _rb.velocity.y > 0)
            {
                var g = _gravityScaleUp * Physics2D.gravity.y;
                var timeSinceJumpStart = Time.time - _jumpStartTime;
                var jumpedHeight = -.5f * g * timeSinceJumpStart * timeSinceJumpStart + _vyzero * timeSinceJumpStart;
                var heightToMin = param.minJumpHeight - jumpedHeight;

                // someone has let go before weve reached our min so make sure we go to our min
                // by calculating a new parabola using out current speed as v0 and the height needed to
                // get to the minimum jump height as h
                if (heightToMin > 0)
                {
                    var vnow = _rb.velocity.y;
                    var gToGetUsToMin = -(vnow * vnow) / (2 * heightToMin);
                    _rb.gravityScale = gToGetUsToMin / Physics2D.gravity.y;
                }
                // someone has let go between the min height and reaching the peak
                // just increase gravity so we fall earlier
                else
                {
                    _rb.gravityScale = _gravityScaleDown;
                }
            }

            _jumpReleased = false;
        }

        private void DetectWhatImTouching()
        {
            var eulerAnglesZ = transform.eulerAngles.z;

            _isGrounded = Physics2D.OverlapBox(
                groundedPos.position,
                new Vector3(groundedWidth, groundedHeight, .1f),
                eulerAnglesZ,
                param.groundMask
            );

            _isOnRightWall = Physics2D.OverlapBox(
                rightWallCheckPos.position,
                new Vector3(rightWallCheckWidth, rightWallCheckHeight, .1f),
                eulerAnglesZ,
                param.groundMask
            );
            _isOnLeftWall = Physics2D.OverlapBox(
                leftWallCheckPos.position,
                new Vector3(leftWallCheckWidth, leftWallCheckHeight, .1f),
                eulerAnglesZ,
                param.groundMask
            );
        }

        private float HorizontalMovement()
        {
            float acceleration;
            float deceleration;

            if (_isGrounded)
            {
                acceleration = _groundedAcceleration;
                deceleration = _groundedDeceleration;
            }
            else
            {
                acceleration = _inAirAcceleration;
                deceleration = _inAirDeceleration;
            }

            var vx = _rb.velocity.x;
            vx += _horizontal * acceleration * Time.fixedDeltaTime;
            if (Mathf.Approximately(_horizontal, 0))
            {
                if (vx > float.Epsilon)
                {
                    vx -= deceleration * Time.fixedDeltaTime;
                    vx = Mathf.Max(0f, vx);
                }
                else if (vx < float.Epsilon)
                {
                    vx += deceleration * Time.deltaTime;
                    vx = Mathf.Min(0f, vx);
                }
            }

            vx = Mathf.Clamp(vx, -_maxSpeed, _maxSpeed);
            return vx;
            // if pressing _horizontal then ease from v to maxSpeed
            // if not ease from v to 0
            // var vx = _rb.velocity.x;
            //
            // if (_leftPressTime > float.Epsilon)
            // {
            //
            //     tweenTime = Time.time - _leftPressTime;
            //     tweenTime /= param.timeToMaxSpeed;
            //     vx = TweenUp(_fromV, -_maxSpeed, tweenTime);
            // } else if (_rightPressTime > float.Epsilon)
            // {
            //     tweenTime = Time.time - _rightPressTime;
            //     tweenTime /= param.timeToMaxSpeed;
            //     vx = TweenUp(_fromV, _maxSpeed, tweenTime);
            // } else if (_horzontalReleasedTime > float.Epsilon)
            // {
            //     tweenTime = Time.time - _horzontalReleasedTime;
            //     tweenTime /= param.timeBackFromMaxToRest;
            //     vx = TweenDown(_fromV, 0, tweenTime);
            // }
            //
            // return vx;


            // var vx = _rb.velocity.x;
            // vx += _horizontal;
            // // todo(chris) understand this damping functions
            // vx *= Mathf.Pow(1 - param.damping, Time.fixedDeltaTime * 10f);
            // return vx;
        }

        private float TweenUp(float initial, float final, float tweenTime)
        {

            var t = 1 - Mathf.Clamp01(tweenTime);
            var tweenFactor = 1 - t * t * t;
            return initial + (final - initial) * tweenFactor;

            //return Mathf.Lerp(initial, final, tweenTime);
            // var t = Mathf.Clamp01(tweenTime);
            // var tweenFactor = t * t * t;
            // return initial + (final - initial) * tweenFactor;
        }

        private float TweenDown(float initial, float final, float tweenTime)
        {
            // var t = 1 - Mathf.Clamp01(tweenTime);
            // var tweenFactor = 1 - t * t * t;
            // return initial + (final - initial) * tweenFactor;

            var t = Mathf.Clamp01(tweenTime);

            var tweenFactor = t * t * t;
            return initial + (final - initial) * tweenFactor;
        }
    }
}
