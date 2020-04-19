using System;
using UnityEngine;
using Util;

namespace CatBall
{
    public class DoorTriggerVolume : MonoBehaviour
    {
        [SerializeField] private Door door;
        [SerializeField] private LayerMask triggers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggers.Contains(other.gameObject.layer))
            {
                door.Activate();
            }
        }
    }
}