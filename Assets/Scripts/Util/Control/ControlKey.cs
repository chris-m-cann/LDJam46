using UnityEngine;

namespace Util.Control
{
    [CreateAssetMenu(menuName = "Control/Binary/Key")]
    public class ControlKey : ControlBinary
    {
        [SerializeField] private KeyCode keyId;

        private bool _wasPressed;
        private bool _wasReleased;

        public void Init(KeyCode key) => keyId = key;

        public override bool IsDown() => Input.GetKey(keyId);
        public override bool WasPressed() => Input.GetKeyDown(keyId);
        public override bool WasReleased() => Input.GetKeyUp(keyId);

        public override void Handled()
        {
            _wasPressed = false;
            _wasReleased = false;
        }

        public override void UpdateControl(GameObject caller)
        {
            _wasPressed = Input.GetKeyDown(keyId);
            _wasReleased = Input.GetKeyUp(keyId);
        }

        public static ControlKey NewInstance(KeyCode code)
        {
            var i = CreateInstance<ControlKey>();
            i.Init(code);
            return i;
        }
    }
}