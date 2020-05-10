using UnityEngine;

namespace Util.Control
{
    public class AxisControlBinary : ControlBinary
    {
        private Axis _axis;
        private float _min;
        private float _lastV;
        private float _v;
        private bool _wasPressed;
        private bool _wasReleased;

        public void Init(Axis axis, float min = 1f)
        {
            _axis = axis;
            _min = min;
        }

        public override void UpdateControl(GameObject caller)
        {
            _lastV = _v;
            _v = Input.GetAxis(_axis.ToString());

            if (_lastV < _min && _v >= _min) _wasPressed = true;
            if (_lastV >= _min && _v < _min) _wasReleased = true;

        }

        public override bool IsDown() => _v > _min;

        public override bool WasPressed() => _wasPressed;

        public override bool WasReleased() => _wasReleased;

        public override void Handled()
        {
            _wasPressed = false;
            _wasReleased = false;
        }

        public static AxisControlBinary NewInstance(Axis axis)
        {
            var i = CreateInstance<AxisControlBinary>();
            i.Init(axis);
            return i;
        }
    }
}