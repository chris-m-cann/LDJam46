using System;
using UnityEngine;

namespace Util
{
    public class Paralax : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float magnitude;

        private Vector3 lastCamPos;

        private Transform _cam;

        private void Start()
        {
            _cam = Camera.main.transform;
            lastCamPos = _cam.position;
        }

        private void LateUpdate()
        {
            var dif = _cam.position - lastCamPos;
            var reduced = dif * magnitude;
            transform.position += reduced;
            lastCamPos = _cam.position;
        }
    }
}