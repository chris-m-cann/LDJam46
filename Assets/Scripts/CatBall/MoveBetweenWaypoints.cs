using UnityEngine;
using Util;

namespace CatBall
{
    public class MoveBetweenWaypoints : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed;
        [SerializeField] private bool runOnAwake = true;


        private readonly InSequenceSelector _selector = new InSequenceSelector();
        private Vector3 _lastTargetPos;
        private Transform _target;
        private float _leaveTime;
        private float _timeToTarget;
        private bool _running = false;

        private void Awake()
        {
            _lastTargetPos = transform.position;
        }

        private void Start()
        {
            if (waypoints.Length == 0)
            {
                return;
            }
            _target = SelectNextWaypoint();
            _lastTargetPos = transform.position;

            var dist = Vector3.Distance(_target.position, _lastTargetPos);

            _timeToTarget = dist / speed;
            _leaveTime = Time.time;
            _running = runOnAwake;
        }

        // Despite not using physics I still use FixedUpdate so as to sync up with the players physics based movement
        // without this I found the players lateral movement was slow and janky
        // I didnt use RigidBody2D physics for the platform as then i found the player came away from the platform if it moved to fast,
        // this may be desirable for dropping away platforms
        private void FixedUpdate()
        {
            if (waypoints.Length == 0 || !_running)
            {
                return;
            }

            if (Vector3.Distance(transform.position, _target.position) < Vector3.kEpsilon)
            {
                _lastTargetPos = transform.position;
                _target = SelectNextWaypoint();
                var dist = Vector3.Distance(_target.position, _lastTargetPos);

                _timeToTarget = dist / speed;
                _leaveTime = Time.time;
            }

            transform.position = (new Vector2(
                TweenPos(_lastTargetPos.x, _target.position.x),
                TweenPos(_lastTargetPos.y, _target.position.y)
            ));
        }

        private float TweenPos(float start, float end)
        {
            return Tween.SmoothStep6(start, end, (Time.time - _leaveTime) / _timeToTarget);
        }

        private Transform SelectNextWaypoint()
        {
            return _selector.Select(waypoints);
        }

        public void Run()
        {
            _running = true;
        }
    }
}