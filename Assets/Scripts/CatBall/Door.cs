using UnityEngine;

namespace CatBall
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private bool isOpen = false;

        private Animator _animator;
        private static readonly int CloseDoor = Animator.StringToHash("CloseDoor");
        private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Activate()
        {
            if (isOpen)
            {
                Debug.Log("Setting CloseDoor Trigger");
                _animator.SetTrigger(CloseDoor);
            }
            else
            {
                Debug.Log("Setting OpenDoor Trigger");
                _animator.SetTrigger(OpenDoor);
            }
        }

    }
}