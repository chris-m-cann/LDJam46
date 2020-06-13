using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class AfterDelay : MonoBehaviour
    {
        public UnityEvent afterDelay;
        [SerializeField] private float delay;
        [SerializeField] private bool repeating;
        [SerializeField] private bool startDelayOnStart = true;

        private void Start()
        {
            if (startDelayOnStart) StartDelay();
        }

        public void StartDelay()
        {
            StartCoroutine(CallEvent());
        }

        private IEnumerator CallEvent()
        {
            do
            {
                yield return new WaitForSeconds(delay);

                afterDelay.Invoke();
            } while (repeating);
        }
    }
}