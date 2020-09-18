using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SetRigidbodyVelocity : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector3 v) => _rigidbody.velocity = v;
    }
}