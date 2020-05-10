using System;
using System.Collections;
using CatBall;
using UnityEngine;
using UnityEngine.Events;
using Util.UI;

namespace Util.Control
{
    public class InputEx : MonoBehaviour
    {
        [SerializeField] private ControlSchemeGenerator kbSchemeMappings;
        [SerializeField] private ControlScheme controls;

        [SerializeField] private float controllerDetectionWait = .5f;
        [SerializeField] private UnityEvent onControllerDetected;
        [SerializeField] private UnityEvent onControllerRemoved;

        private bool _hasController;

        public static bool IsControllerConnected()
        {
            var joysticks = Input.GetJoystickNames();

            foreach (var name in joysticks)
            {
                // when controller is disconnected an empty string can remain in the list in its place
                if (string.IsNullOrEmpty(name)) continue;

                return true;
            }

            return false;
        }

        public void GenerateControls()
        {
            controls.Set(kbSchemeMappings.GenerateScheme());
        }


        private void Start()
        {
            GenerateControls();
            StartCoroutine(PollForController());
        }

        private void Update()
        {
            controls.UpdateControls(gameObject);
        }

        private IEnumerator PollForController()
        {
            while (isActiveAndEnabled)
            {
                var controllerPresent = IsControllerConnected();
                if (!_hasController && controllerPresent)
                {
                    _hasController = true;
                    onControllerDetected.Invoke();
                } else if (_hasController && !controllerPresent)
                {
                    onControllerRemoved.Invoke();
                }

                yield return new WaitForSecondsRealtime(controllerDetectionWait);
            }
        }
    }
}