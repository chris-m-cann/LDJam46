using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerVolumeBehaviour : MonoBehaviour
    {
        public UnityEvent onTrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTrigger.Invoke();
        }
    }
}