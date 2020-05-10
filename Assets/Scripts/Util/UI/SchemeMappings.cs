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
        public Dictionary<PlayerAction, InputBindingsManager.ControlBinding> CreateControlBindings()
        {
            var controls = new Dictionary<PlayerAction, InputBindingsManager.ControlBinding>();

            foreach (var binding in bindings)
            {
                var control = new InputBindingsManager.ControlBinding
                {
                    key = binding.key,
                    axis = binding.axis
                };

                if (binding.key == KeyCode.None) control.key = null;
                controls[binding.action] = control;
            }

            return controls;
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
        }
    }
}