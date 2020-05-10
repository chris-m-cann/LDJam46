using System;
using UnityEngine;

namespace Util.Control
{
    [CreateAssetMenu(menuName = "Control/2D/MouseDirection")]
    public class MouseDirection : Control2D
    {
        [NonSerialized] private Camera _cam;
        [NonSerialized] private Vector2 _dir = Vector2.zero;
        [NonSerialized] private Transform _target;

        [SerializeField] private float snapDegrees = 5f;
        public override Vector2 GetDirection(Transform from)
        {
            _target = from;
            CaclulateDir();
            return _dir;
        }

        public override void UpdateControl(GameObject caller)
        {
            if (!_cam) _cam = Camera.main;
        }

        private void CaclulateDir()
        {
            if (!_cam) return;

            var position = _target.position;

            var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = position.z;

            _dir = (pos - position).normalized;
        }

        public static MouseDirection NewInstance()
        {
            return CreateInstance<MouseDirection>();
        }
    }
}