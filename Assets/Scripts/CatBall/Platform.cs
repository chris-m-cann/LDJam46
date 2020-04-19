using System;
using UnityEngine;

namespace Platform
{
    [RequireComponent(typeof(Collider2D))]
    public class Platform : MonoBehaviour
    {
        private IOnPlayerEnter _onPlayerEnter;
        private IOnPlayerExit _onPlayerExit;

        private void Awake()
        {
            _onPlayerEnter = GetComponent<IOnPlayerEnter>();
            _onPlayerExit = GetComponent<IOnPlayerExit>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            other.gameObject.transform.parent = transform;

            _onPlayerEnter?.OnPlayerEnter(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            other.gameObject.transform.parent = null;

            _onPlayerExit?.OnPlayerExit(other.gameObject);
        }
    }
}
