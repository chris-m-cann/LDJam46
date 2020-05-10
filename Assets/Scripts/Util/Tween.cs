using UnityEngine;

namespace Util
{
    public static class Tween
    {
        public delegate float Function(float start, float end, float time);
        public static float Lerp(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t;
        }

        #region SmoothStart

        public static float SmoothStart2(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t * t;
        }

        public static float SmoothStart3(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t * t * t;
        }

        public static float SmoothStart4(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t * t * t * t;
        }

        public static float SmoothStart5(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t * t * t * t * t;
        }

        public static float SmoothStart6(float start, float end, float time)
        {
            var t = Mathf.Clamp01(time);
            return start + (end - start) * t * t * t * t * t * t;
        }

        #endregion

        #region SmoothStop

        public static float SmoothStop2(float start, float end, float time)
        {
            var t =  1 - Mathf.Clamp01(time);
            return start + (end - start) * (1 - t * t);
        }

        public static float SmoothStop3(float start, float end, float time)
        {
            var t =  1 - Mathf.Clamp01(time);
            return start + (end - start) * (1 - t * t * t);
        }

        public static float SmoothStop4(float start, float end, float time)
        {
            var t =  1 - Mathf.Clamp01(time);
            return start + (end - start) * (1 - t * t * t * t);
        }

        public static float SmoothStop5(float start, float end, float time)
        {
            var t =  1 - Mathf.Clamp01(time);
            return start + (end - start) * (1 - t * t * t * t * t);
        }

        public static float SmoothStop6(float start, float end, float time)
        {
            var t =  1 - Mathf.Clamp01(time);
            return start + (end - start) * (1 - t * t * t * t * t * t);
        }

        #endregion

        #region SmoothStep

        public static float SmoothStep3(float start, float end, float time)
        {
            var t =  Mathf.Clamp01(time);
            var inT = t * t;
            var outT = 1 - (1 - t) * (1 - t);

            var fade = (1 - t) * inT + t * outT;

            return start + (end - start) * fade;
        }

        public static float SmoothStep4(float start, float end, float time)
        {
            var t =  Mathf.Clamp01(time);
            var inT = t * t * t;
            var outT = 1 - (1 - t) * (1 - t) * (1 - t);

            var fade = (1 - t) * inT + t * outT;

            return start + (end - start) * fade;
        }

        public static float SmoothStep5(float start, float end, float time)
        {
            var t =  Mathf.Clamp01(time);
            var inT = t * t * t * t;
            var outT = 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);

            var fade = (1 - t) * inT + t * outT;

            return start + (end - start) * fade;
        }

        public static float SmoothStep6(float start, float end, float time)
        {
            var t =  Mathf.Clamp01(time);
            var inT = t * t * t * t * t;
            var outT = 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t) * (1 - t);

            var fade = (1 - t) * inT + t * outT;

            return start + (end - start) * fade;
        }

        #endregion
    }
}