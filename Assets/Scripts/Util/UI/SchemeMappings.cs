using System;
using System.Collections.Generic;
using CatBall;
using UnityEngine;
using Util.Control;

namespace Util.UI
{
    [CreateAssetMenu(menuName = "Custom/Input/SchemeMappings")]
    public class SchemeMappings : ScriptableObject
    {
        [Serializable]
        public struct ActionBinding
        {
            public PlayerAction action;
            public KeyCode key;
            public Axis axis;
        }

        [SerializeField] private List<ActionBinding> bindings;

        private Dictionary<PlayerAction, InputBindingsManager.ControlBinding> _bindingsCache;
        public Dictionary<PlayerAction, InputBindingsManager.ControlBinding> CreateControlBindings()
        {
            if (_bindingsCache != null) return _bindingsCache;

            _bindingsCache = new Dictionary<PlayerAction, InputBindingsManager.ControlBinding>();

            foreach (var binding in bindings)
            {
                var control = new InputBindingsManager.ControlBinding
                {
                    key = binding.key,
                    axis = binding.axis
                };

                if (binding.key == KeyCode.None) control.key = null;
                _bindingsCache[binding.action] = control;
            }

            return _bindingsCache;
        }

        public void Save(Dictionary<PlayerAction, InputBindingsManager.ControlBinding> controlBindings)
        {
            bindings.Clear();
            foreach (var pair in controlBindings)
            {
                bindings.Add(new ActionBinding
                {
                    action = pair.Key,
                    key = pair.Value.key ?? KeyCode.None,
                    axis = pair.Value.axis
                });
            }

            _bindingsCache = null;
        }
    }
}