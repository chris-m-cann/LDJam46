using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/ScreenShake")]
    public class ScreenShakeParameters : ScriptableObject
    {
        [Range(0, 1)] public float magnitude;
        public float duration;
    }
}