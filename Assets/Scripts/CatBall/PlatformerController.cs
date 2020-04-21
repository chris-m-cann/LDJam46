﻿using UnityEngine;
using UnityEngine.Events;
using Util;

namespace CatBall
{
    public class PlatformerController : MonoBehaviour
    {
        public bool IsGrounded
        {
            get => _isGrounded;
        }

        public bool IsOnAWall
        {
            get
            {
                if (_isGrounded) return false;
                var h = Input.GetAxis("Horizontal");
                return (h < 0 && _isOnLeftWall) || (h > 0 && _isOnRightWall);
            }
        }

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

        // 0 - from ground
        // 1 - from wall
        // can you tell im running out of time :)
        [SerializeField] private IntUnityEvent onJump;

        private float _gravityScaleUp = 1f;
        private float _gravityScaleDown = 1f;
        private float _vyzero;
        private float _wallJumpGravityScale = 1f;
        private Vector2 _wallJumpVzero;

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
        private bool _wasOnAWall;
        private float _lastPress;
        private float _lastGrounded;
        private float _lastOnAWall;

        private float _groundedAcceleration;
        private float _groundedDeceleration;
        private float _inAirAcceleration;
        private float _inAirDeceleration;
        private float _offTheWallAcceleration;
        private float _offTheWallDeceleration;


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

            var wallJumpVyzero = 2 * param.wallJumpHeight / param.wallJumpTimeToPeak;
            var wallJumpTimeToPeak = 2 * param.wallJumpHeight / wallJumpVyzero;
            var gwall = - 2 * param.wallJumpHeight / (wallJumpTimeToPeak * wallJumpTimeToPeak);
            _wallJumpGravityScale = gwall / Physics2D.gravity.y;
            var wallJumpxzero = param.wallJumpMinLength / param.timeToBackFromMaxSpeedOffTheWall;
            _wallJumpVzero = new Vector2(wallJumpxzero, wallJumpVyzero);

            _lastGrounded = float.MinValue;
            _lastPress = float.MinValue;
            _lastGrounded = float.MinValue;

            _maxSpeed = param.maxJumpWidth / (param.timeToPeak + param.timeBackDown);

            _groundedAcceleration = _maxSpeed / param.timeToMaxSpeed;
            _groundedDeceleration = _maxSpeed / param.timeBackFromMaxToRest;

            _inAirAcceleration = _maxSpeed / param.timeToMaxSpeedInAir;
            _inAirDeceleration = _maxSpeed / param.timeToBackFromMaxSpeedInAir;

            _offTheWallAcceleration = _maxSpeed / param.timeToMaxSpeedOffTheWall;
            _offTheWallDeceleration = _maxSpeed / param.timeToBackFromMaxSpeedOffTheWall;
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

            var onAWall = !_isGrounded && (_isOnLeftWall || _isOnRightWall);

            if (onAWall)
            {
                _lastOnAWall = Time.time;
                _wasOnAWall = true;
            }

            if (_isGrounded) _wasOnAWall = false;


            var vx = HorizontalMovement();

            // these stop the player sticking on walls when pressing in to them
            var pressingIntoWall = false;
            if (_isOnRightWall && vx > 0)
            {
                vx = _rb.velocity.x;
                pressingIntoWall = true;
            }

            if (_isOnLeftWall && vx < 0)
            {
                vx = _rb.velocity.x;
                pressingIntoWall = true;
            }

            var newVelocity =  HandleJumpPress(vx);




            newVelocity.y = Mathf.Max(newVelocity.y, -param.maxFallSpeed);

            _rb.velocity = newVelocity;
            HandleJumpButtonRelease();


            if (_rb.velocity.y < 0)
            {
                if (!(onAWall && pressingIntoWall))
                {
                    _rb.gravityScale = _gravityScaleDown;
                    _wasOnAWall = false;
                }
                else // wall sliding
                {
                    _rb.gravityScale = 0;
                    _rb.velocity = new Vector2(_rb.velocity.x, -param.wallSlideSpeed);
                }
            }



        }

        private Vector2 HandleJumpPress(float xvel)
        {
            var yvel = _rb.velocity.y;


            var inGraceTime = (Time.time - _lastPress) < param.graceTime;
            var inGroundedGraceTime = _isGrounded && inGraceTime;
            var inCoyoteTime = _jumpPressed && (Time.time - _lastGrounded) < param.coyoteTime;
            var inWallJumpCoyoteTime = _jumpPressed && (Time.time - _lastOnAWall) < param.wallJumpCoyoteTime;


            if (!_isGrounded && _isOnLeftWall && (inGraceTime || inWallJumpCoyoteTime))
            {
                _rb.gravityScale = _wallJumpGravityScale;
                _jumpStartTime = Time.time;
                _lastPress = 0;
                yvel = _wallJumpVzero.y;
                xvel = _wallJumpVzero.x;

                onJump.Invoke(1);
            }
            else if (!_isGrounded && _isOnRightWall && (inGraceTime || inWallJumpCoyoteTime))
            {
                _rb.gravityScale = _wallJumpGravityScale;
                _jumpStartTime = Time.time;
                _lastPress = 0;
                yvel = _wallJumpVzero.y;
                xvel = -_wallJumpVzero.x;

                onJump.Invoke(1);
            }
            else
            {
                if (inGroundedGraceTime || inCoyoteTime)
                {
                    //Debug.Log($"jumping because inGraceTime={inGraceTime}, inCoyoteTime={inCoyoteTime}");
                    _rb.gravityScale = _gravityScaleUp;
                    _jumpStartTime = Time.time;
                    _lastPress = 0;
                    yvel = _vyzero;

                    onJump.Invoke(0);
                }
            }

            _jumpPressed = false;

            return new Vector2(xvel, yvel);
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
            else if (_wasOnAWall)
            {
                acceleration = _offTheWallAcceleration;
                deceleration = _offTheWallDeceleration;
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
