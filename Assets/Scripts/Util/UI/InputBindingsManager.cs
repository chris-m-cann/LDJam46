using System;
using System.Collections.Generic;
using CatBall;
using UnityEngine;
using UnityEngine.Events;
using Util.Control;

namespace Util.UI
{
    public class InputBindingsManager : MonoBehaviour
    {
        public struct ControlBinding
        {
            public KeyCode? key;
            public Axis axis;
        }

        [SerializeField] private SchemeMappings _scheme;

        [SerializeField] private InputDisplayMappings displayMappings;


        [SerializeField] private StringUnityEvent onBindAction;
        [SerializeField] private StringUnityEvent onActionChosen;

        private Dictionary<PlayerAction, StringUnityEvent> actionDisplayNameEvents = new Dictionary<PlayerAction, StringUnityEvent>();
        private Dictionary<PlayerAction, SpriteUnityEvent> actionDisplaySpriteEvents = new Dictionary<PlayerAction, SpriteUnityEvent>();

        private Dictionary<PlayerAction, ControlBinding> _controlBindings;

        private void OnEnable()
        {
            LoadMappings();
        }

        private void OnDisable()
        {
            SaveMappings();
        }

        public void RegisterAction(
            PlayerAction action,
            UnityAction<String> onDisplayStringChanged,
            UnityAction<Sprite> onDisplaySpriteChanged
            )
        {
            if (onDisplayStringChanged != null)
            {
                StringUnityEvent evnt = actionDisplayNameEvents.ContainsKey(action) ? actionDisplayNameEvents[action] : new StringUnityEvent();
                evnt.AddListener(onDisplayStringChanged);
                actionDisplayNameEvents[action] = evnt;
            }

            if (onDisplaySpriteChanged != null)
            {
                SpriteUnityEvent evnt = actionDisplaySpriteEvents.ContainsKey(action)
                    ? actionDisplaySpriteEvents[action]
                    : new SpriteUnityEvent();
                evnt.AddListener(onDisplaySpriteChanged);
                actionDisplaySpriteEvents[action] = evnt;
            }
        }

        public void BindAction(PlayerAction action, KeyCode code)
        {
            MapDisplay(action, code);

            _controlBindings[action] = new ControlBinding{ key = code};

            onActionChosen.Invoke(action.ToString());

        }

        public void BindAction(PlayerAction action, Axis axis)
        {
            MapDisplay(action, axis);

            _controlBindings[action] = new ControlBinding{ axis = axis };

            onActionChosen.Invoke(action.ToString());
        }

        private void MapDisplay(PlayerAction action, KeyCode code)
        {
            var mapped = displayMappings.GetKeyCodeDisplay(code);

            mapped.Match(
                it => actionDisplayNameEvents.GetValue(action)?.Invoke(it),
                it => actionDisplaySpriteEvents.GetValue(action)?.Invoke(it)
            );
        }

        private void MapDisplay(PlayerAction action, Axis axis)
        {
            var mapped = displayMappings.GetAxisDisplay(axis);

            mapped.Match(
                it => actionDisplayNameEvents.GetValue(action)?.Invoke(it),
                it => actionDisplaySpriteEvents.GetValue(action)?.Invoke(it)
            );
        }


        public void StartRebinding(PlayerAction action)
        {
            onBindAction.Invoke(action.ToString());
        }

        public void LoadMappings()
        {
            // basically have a circular reference here. The CreateControlBindings needs the actions to be registered and the register action for the axis needs the bindings in place. not really sure how this worked before
            _controlBindings = _scheme.CreateControlBindings();

            var actions = GetComponentsInChildren<IRegisteredAction>();
            foreach (var action in actions)
            {
                action.RegisterAction(this);
            }


            foreach (var binding in _controlBindings)
            {
                if (binding.Value.key.HasValue)
                {
                    MapDisplay(binding.Key, binding.Value.key.Value);
                }
                else
                {
                    MapDisplay(binding.Key, binding.Value.axis);
                }
            }
        }

        public void SaveMappings()
        {
            _scheme.Save(_controlBindings);
        }

        public ControlBinding? GerCurrentBinding(PlayerAction action)
        {
            if (!_controlBindings.ContainsKey(action)) return null;
            return _controlBindings[action];
        }

        public void SwitchScheme(SchemeMappings newScheme)
        {
            SaveMappings();
            _scheme = newScheme;
            LoadMappings();
        }
    }
}