using System;
using UnityEngine;

namespace CatBall
{
    [CreateAssetMenu(menuName = "Control/MouseAndKeyboard")]
    public class MouseAndKeyboardControls : KickControlScheme
    {
        [SerializeField] private float graceTime = .1f;
        [SerializeField] private float snapDegrees = 5f;

        [NonSerialized] private Camera _cam;
        [NonSerialized] private bool _kickButtonDown = false;
        [NonSerialized] private bool _cancelButtonDown = false;
        [NonSerialized] private Vector2 _direction = Vector2.zero;
        [NonSerialized] private float _kickGraceEndTime;

        public override void UpdateControls(GameObject caller)
        {
            if (Input.GetMouseButtonDown(1) && !_kickButtonDown)
            {
                _kickGraceEndTime = Time.unscaledTime + graceTime;
                _kickButtonDown = true;
            }

            if (Time.unscaledTime > _kickGraceEndTime) KickButtonHandled();

            if (!_cam) _cam = Camera.main;

            var position = caller.transform.position;

            var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = position.z;

            var dir = (pos - position).normalized;

            var angle = Vector3.Angle(Vector3.up, dir);

            int eighths = (int) (angle / 45);
            float last = angle % 45;

            float finalAngle;
            if (last < snapDegrees)
            {
                finalAngle = eighths * 45;
            } else if ((45 - last) < snapDegrees)
            {
                finalAngle = (eighths + 1) * 45;
            }
            else
            {
                finalAngle = eighths * 45 + last;
            }

            if (dir.x > 0) finalAngle *= -1;

            // Debug.Log($"angle={angle}, eighths={eighths}, last={last}, final={finalAngle}");

            _direction = Quaternion.AngleAxis(finalAngle, Vector3.forward) * Vector3.up;
        }

        public override bool KickButtonDown()
        {
            return _kickButtonDown;
        }

        public override void KickButtonHandled()
        {
            _kickButtonDown = false;
        }

        public override bool CancelButtonDown()
        {
            return _cancelButtonDown;
        }

        public override void CancelButtonHandled()
        {
            _cancelButtonDown = false;
        }

        public override Vector2 GetDirection() => _direction;
    }
}