using System;
using UnityEngine;

namespace Util.Control
{
    public class TwoButtonControl1D : Control1D
    {
        [SerializeField] private ControlBinary negative;
        [SerializeField] private ControlBinary positive;

        [NonSerialized]
        private float _cached;

        public void Init(ControlBinary negative, ControlBinary positive)
        {
            this.negative = negative;
            this.positive = positive;
        }

        public override void UpdateControl(GameObject caller)
        {
            var plus = positive.IsDown() ? 1 : 0;
            var minus = negative.IsDown() ? -1 : 0;
            _cached = minus + plus;
        }

        public override float GetAxisRaw()
        {
            return _cached;
        }

        public static TwoButtonControl1D NewInstance(ControlBinary negative, ControlBinary positive)
        {
            var instance = CreateInstance<TwoButtonControl1D>();
            instance.Init(negative, positive);
            return instance;
        }
    }
}