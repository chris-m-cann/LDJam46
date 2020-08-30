using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerVolumeBehaviour : MonoBehaviour
    {
        public String targetTag;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;

        public GameObjectUnityEvent onTriggerEnterWithObj;
        public GameObjectUnityEvent onTriggerExitWithObj;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (targetTag.Length != 0 && !other.gameObject.CompareTag(targetTag)) return;

            onTriggerEnter.Invoke();
            onTriggerEnterWithObj.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (targetTag.Length != 0 && !other.gameObject.CompareTag(targetTag)) return;

            onTriggerExit.Invoke();
            onTriggerExitWithObj.Invoke(other.gameObject);
        }
    }
}