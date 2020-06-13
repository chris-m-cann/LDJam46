using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerVolumeBehaviour : MonoBehaviour
    {
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit.Invoke();
        }
    }
}