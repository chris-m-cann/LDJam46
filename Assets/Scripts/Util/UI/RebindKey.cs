using System;
using CatBall;
using UnityEngine;
using Util.Control;

namespace Util.UI
{
    public class RebindKey : MonoBehaviour, IRegisteredAction
    {
        [SerializeField] private PlayerAction action;
        [SerializeField] private bool allowKeyboardKeys;
        [SerializeField] private bool allowMouseButtons;
        [Tooltip("number of joysticks to register input from")]
        [Range(0,8)]
        [SerializeField] private int allowJoysticksButtons;

        [SerializeField] private bool allowTriggersAsButtons;

        [SerializeField] private StringUnityEvent onRebindActionString;
        [SerializeField] private SpriteUnityEvent onRebindActionSprite;


        private bool _scanning = false;
        private KeyCode[] _codes;
        private InputBindingsManager _bindings;

        public void RegisterAction(InputBindingsManager bindingsManager)
        {
            _bindings = bindingsManager;
            _bindings.RegisterAction(
                action,
                str => onRebindActionString.Invoke(str),
                spr => onRebindActionSprite.Invoke(spr)
            );
            ConfigureAllowedCodes();
        }

        private void OnValidate()
        {
            ConfigureAllowedCodes();
        }

        private void ConfigureAllowedCodes()
        {
            var allCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
            _codes = FilterCodes(allCodes);
        }

        private KeyCode[] FilterCodes(KeyCode[] codes)
        {
            var cpy = new KeyCode[codes.Length];
            var cpyIdx = 0;


            // start at 1 as we dont want KeyCode.None
            for (int i = 1; i < codes.Length ; i++)
            {
                var v = (int) codes[i];
                // enum value 323 is where keyboard codes end and mouse codes begin
                if (!allowKeyboardKeys && InHalfOpenRange(v, 0, 323)) continue;
                // enum value 330 is where mouse buttons end and joysitcks begin
                if (!allowMouseButtons && InHalfOpenRange(v, 323, 330)) continue;
                // joysitcks contains 20 entries starting at 330, ending at 510
                if (InHalfOpenRange(v, 330 + (20 * allowJoysticksButtons), 510)) continue;

                cpy[cpyIdx] = codes[i];
                ++cpyIdx;
            }

            Array.Resize(ref cpy, cpyIdx);

            return cpy;
        }

        private bool InHalfOpenRange(int v, int fromInclusive, int toExclusive)
        {
            return (v >= fromInclusive && v < toExclusive);
        }

        private void Update()
        {
            if (!_scanning) return;


            // KeyCode actuall includes mouse buttons 0-4 minimum so no need to specific checks!!
            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in _codes)
                {
                    if (Input.GetKeyDown(code))
                    {
                        RebindKeyCode(code);
                        return;
                    }
                }
            }

            if (allowTriggersAsButtons)
            {
                if (Input.GetAxisRaw(Axis.LeftTrigger.ToString()) > .9f)
                {
                    RebindAxis(Axis.LeftTrigger);
                    return;
                }


                if (Input.GetAxis(Axis.RightTrigger.ToString()) > .9f)
                {
                    RebindAxis(Axis.RightTrigger);
                    return;
                }
            }
        }

        private void RebindAxis(Axis name)
        {
            _scanning = false;
            _bindings.BindAction(action, name);
        }

        private void RebindKeyCode(KeyCode name)
        {
            _scanning = false;
            _bindings.BindAction(action, name);
        }

        public void StartRebind()
        {
            _bindings.StartRebinding(action);
            _scanning = true;
        }
    }
}