using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class OnKeyPress : MonoBehaviour
    {
        [SerializeField] private KeyCode key = KeyCode.Escape;
        [SerializeField] private UnityEvent onKeyPressed;


        void Update()
        {
            if (Input.GetKeyDown(key)) onKeyPressed.Invoke();
        }
    }
}