using System;
using UnityEngine;

namespace Util
{
    public class ScreenShake : MonoBehaviour
    {
        private ScreenShaker _shaker;

        private void Awake()
        {
            _shaker = FindObjectOfType < ScreenShaker >();
        }

        public void Shake(ScreenShakeParameters ps) => _shaker?.StartShake(ps);
    }
}