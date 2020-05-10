using System;
using CatBall;
using UnityEngine;
using Util.Control;

namespace Util.UI
{
    public class RebindAxisFromOptions : MonoBehaviour
    {
        [SerializeField] private PlayerAction action;
        [SerializeField] private Axis[] axes;
        [SerializeField] private MultipleChoiceOptionMenuItem choices;
        private InputBindingsManager _bindings;


        private void Awake()
        {
            _bindings = FindObjectOfType<InputBindingsManager>();
        }

        private void Start()
        {
            if (axes.Length == 0)
            {
                Debug.LogError("need some axes to choose between");
                return;
            }

            string[] options = new string[axes.Length];
            for (int i = 0; i < axes.Length; i++)
            {
                options[i] = axes[i].ToString();
            }

            var binding = _bindings.GerCurrentBinding(action) ?? new InputBindingsManager.ControlBinding
            {
                axis = axes[0]
            };

            var initialOption = Mathf.Max(Array.IndexOf(axes, binding.axis), 0);

            choices.SetOptions(options, initialOption);
        }

        public void OnChoiceSelected(int idx)
        {
            if (idx < 0 || idx >= axes.Length)
            {
                Debug.LogError($"Cannot select axis {idx} as only {axes.Length} in list");
                return;
            }

            _bindings.BindAction(action, axes[idx]);
        }
    }
}