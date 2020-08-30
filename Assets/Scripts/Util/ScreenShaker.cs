using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class ScreenShaker : MonoBehaviour
    {
        [Range(0, 360)] [SerializeField] private float maxAngleDegrees = 10f;
        [SerializeField] private float maxTranslation = .5f;
        [SerializeField] private float fequency = 2f;
        [Space] [SerializeField] private bool useCinemachineIfPossible = true;
        [SerializeField] private float cinemachineAmplitudeMultiplier = 2;

        private CinemachineBrain _theBrain;
        private CinemachineImpulseSource _impulse;
        private CinemachineImpulseListener _listener;
        private bool checkedForImpluseListener = false;

        private void Awake()
        {
            _theBrain = GetComponent<CinemachineBrain>();
            _impulse = GetComponent<CinemachineImpulseSource>();
        }

        public void OnSceneReloaded()
        {
            checkedForImpluseListener = false;
        }

        public void StartShake(ScreenShakeParameters ps)
        {
            if (!checkedForImpluseListener)
            {
                _listener = _theBrain.ActiveVirtualCamera.VirtualCameraGameObject
                    .GetComponent<CinemachineImpulseListener>();
                checkedForImpluseListener = true;
            }

            if (useCinemachineIfPossible && _listener != null && _listener.isActiveAndEnabled && _impulse != null && _impulse.isActiveAndEnabled)
            {
                StartImpulse(ps.magnitude, ps.duration, _impulse);
            }
            else
            {
                StartShake(ps.magnitude, ps.duration);
            }
        }


        // shake target manually using perlin noise
        private void StartShake(float magnitude, float duration)
        {
            var target = _theBrain.ActiveVirtualCamera?.VirtualCameraGameObject?.transform ?? transform;
            StartCoroutine(Shake(magnitude, duration, target));
        }

        private IEnumerator Shake(float magnitude, float duration, Transform target)
        {
            float start = Time.time;
            float end = start + duration;

            float lastDeltaAngle = 0f;
            Vector3 lastDeltaTranslation = Vector3.zero;

            var seed = Random.Range(1f, 100f);

            while (end > Time.time && target != null)
            {
                if (target == null) yield break;
                
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

            if (target == null) yield break;

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


        // Shake virtual camera via cinemachines impulse feature
        private void StartImpulse(float magnitude, float duration, CinemachineImpulseSource impulse)
        {
            impulse.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = duration;
            impulse.m_ImpulseDefinition.m_AmplitudeGain = magnitude * cinemachineAmplitudeMultiplier;
            impulse.GenerateImpulse();
        }

        // Currently Unused
        // shake virtual camera via Cinemachine BasicPerlinNoise noise component
        public void StartNoiseShake(float magnitude, float duration)
        {
            StartCoroutine(CoNoiseShake(magnitude, duration));
        }

        private IEnumerator CoNoiseShake(float magnitude, float duration)
        {
            var cam = _theBrain.ActiveVirtualCamera;
            if (cam == null)
            {
                Debug.LogError("No Active Virtual camera");
                yield break;
            }

            var camGo = cam.VirtualCameraGameObject;

            if (camGo == null)
            {
                Debug.LogError("camGo null");
                yield break;
            }

            var vcam = camGo.GetComponent<CinemachineVirtualCamera>();

            var perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (perlin == null)
            {
                Debug.LogError($"Couldnt Find Perlin on {camGo.name}");
                yield break;
            }


            perlin.m_AmplitudeGain = magnitude;

            yield return new WaitForSeconds(duration);
            perlin.m_AmplitudeGain = 0;
        }
    }


}