using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class GenerationUtils : MonoBehaviour
    {
        [SerializeField] private Bounds[] candiateVolumes;
        [SerializeField] private Vector3UnityEvent onPositionGenerated;
        [SerializeField] private Vector3UnityEvent onDirectionGenerated;

        private void OnDrawGizmosSelected()
        {
            foreach (var bounds in candiateVolumes)
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        public Vector3 GeneratePositionInVolumes()
        {
            if (candiateVolumes.Length == 0) return Vector3.zero;

            var volume = candiateVolumes[Random.Range(0, candiateVolumes.Length)];

            var p = new Vector3(
                    Random.Range(volume.min.x, volume.max.x),
                    Random.Range(volume.min.y, volume.max.y),
                    0
                );

            Debug.Log($"volume center = {volume.center}, extents = {volume.extents}, p = {p}");

            return p;
        }

        public void GeneratePositionEvent()
        {
            onPositionGenerated.Invoke(GeneratePositionInVolumes());
        }

        public Vector3 GenerateNormalisedDirection()
        {
            return Random.insideUnitCircle.normalized;
        }

        public void GenerateDirectionEvent()
        {
            onDirectionGenerated.Invoke(GenerateNormalisedDirection());
        }

        public void GenerateDirectionEvent(float magnitude)
        {
            var v = GenerateNormalisedDirection() * magnitude;
            Debug.Log($"Generated velocity = {v}");
            onDirectionGenerated.Invoke(v);
        }

        // void f()
        // {
        //     disable game object;
        //     set random velocity of set magnitude;
        //     set random position;
        //     enable game object;
        // }
    }
}