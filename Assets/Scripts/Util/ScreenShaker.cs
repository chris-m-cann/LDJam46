using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class ScreenShaker : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float magnitude = .5f;
        [SerializeField] private float duration = 2f;
        [Range(0, 360)] [SerializeField] private float maxAngleDegrees = 10f;
        [SerializeField] private float maxTranslation = .5f;
        [SerializeField] private float fequency = 2f;

        private CinemachineBrain _theBrain;

        private void Awake()
        {
            _theBrain = GetComponent<CinemachineBrain>();
        }

        public void StartShake(float magnitude, float duration)
        {
            var target = _theBrain.ActiveVirtualCamera?.VirtualCameraGameObject?.transform ?? transform;
            StartCoroutine(Shake(magnitude, duration, target));
        }

        public void StartShake(ScreenShakeParameters ps) => StartShake(ps.magnitude, ps.duration);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartShake(magnitude, duration);
            }
        }


        private IEnumerator Shake(float magnitude, float duration, Transform target)
        {
            float start = Time.time;
            float end = start + duration;

            float lastDeltaAngle = 0f;
            Vector3 lastDeltaTranslation = Vector3.zero;

            var seed = Random.Range(1f, 100f);

            while (end > Time.time)
            {
                var t = (Time.time - start) / duration;
                var m = Tween.SmoothStop3(magnitude, 0, t);


                var deltaAngle = maxAngleDegrees * m * GetFloatOverTime(seed);
                var deltaX = maxTranslation * m * GetFloatOverTime(seed * 10);
                var deltaY = maxTranslation * m * GetFloatOverTime(seed * 100);
                var translation = new Vector3(deltaX, deltaY, 0f);

                var eulers = target.rotation.eulerAngles;
                // return to normal
                eulers.z -= lastDeltaAngle;
                eulers.z += deltaAngle;

                target.rotation = Quaternion.Euler(eulers);
                target.position -= lastDeltaTranslation;
                target.position += translation;

                lastDeltaAngle = deltaAngle;
                lastDeltaTranslation = translation;

                yield return null;
            }

            // return to normal
            var finalEulers = target.rotation.eulerAngles;
            finalEulers.z -= lastDeltaAngle;

            target.rotation = Quaternion.Euler(finalEulers);
            target.position -= lastDeltaTranslation;
        }

        private float GetFloatOverTime(float seed)
        {
            var p = Mathf.Clamp01(Mathf.PerlinNoise(seed, Time.time  * fequency));
            // start of range + p * bredth of range (-1, 1) in this case
            return -1 + (p * 2);
        }
    }
}