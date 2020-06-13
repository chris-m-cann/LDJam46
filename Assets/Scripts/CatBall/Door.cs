using UnityEngine;

namespace CatBall
{
    [RequireComponent(typeof(Animator))]
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
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Close()
        {
            if (!isOpen) return;
            _animator.SetTrigger(CloseDoor);
            isOpen = false;
        }

        public void Open()
        {
            if (isOpen) return;
            _animator.SetTrigger(OpenDoor);
            isOpen = true;
        }
    }
}