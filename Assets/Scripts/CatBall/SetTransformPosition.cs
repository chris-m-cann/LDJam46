using UnityEngine;

namespace CatBall
{
    public class SetTransformPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 pos;

        public void SetPosition()
        {
            SetPosition(pos);
        }

        public void SetPosition(Vector3 newPos)
        {
            transform.position = newPos;
        }
    }
}