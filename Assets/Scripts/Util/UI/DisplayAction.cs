using System;
using CatBall;
using TMPro;
using UnityEngine;
using Util.Control;

namespace Util.UI
{
    public class DisplayAction : MonoBehaviour
    {
        [SerializeField] private PlayerAction action;
        [SerializeField] private SchemeMappings kbMappings;
        [SerializeField] private SchemeMappings controllerMappings;
        [SerializeField] private InputDisplayMappings displayMappings;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private SpriteRenderer sprite;

        private SchemeMappings _activeMappings;
        private InputBindingsManager.ControlBinding? _binding;

        private void Start()
        {
            _activeMappings = InputEx.IsControllerConnected() ? controllerMappings : kbMappings;
            if (_activeMappings.CreateControlBindings().TryGetValue(action, out var binding))
            {
                _binding = binding;
            }
            UpdateText();
        }

        private void Update()
        {
            bool updateNeeded = false;

            var mappings = InputEx.IsControllerConnected() ? controllerMappings : kbMappings;

            if (_activeMappings.CreateControlBindings().TryGetValue(action, out var binding))
            {
                updateNeeded = _binding.HasValue && !Equals(_binding.Value, binding);
                _binding = binding;
            }

            if (mappings != _activeMappings)
            {
                _activeMappings = mappings;
                updateNeeded = true;
            }

            if (updateNeeded)
            {
                UpdateText();
            }
        }

        private void UpdateText()
        {
            Debug.Log("Calling Update text");
            if (_binding.HasValue)
            {
                var binding = _binding.Value;

                if (binding.key != null)
                {
                    displayMappings.GetKeyCodeDisplay(binding.key.Value).Match(
                        s =>
                        {
                            sprite.enabled = false;
                            text.enabled = true;
                            text.text = s;
                        },
                        img =>
                        {
                            text.enabled = false;
                            sprite.enabled = true;
                            sprite.sprite = img;
                        }
                        );
                }
                else
                {
                    displayMappings.GetAxisDisplay(binding.axis).Match(
                        s =>
                        {
                            sprite.enabled = false;
                            text.enabled = true;
                            text.text = s;
                        },
                        img =>
                        {
                            text.enabled = false;
                            sprite.enabled = true;
                            sprite.sprite = img;
                        }
                    );
                }
            }
            else
            {
                text.enabled = false;
                sprite.enabled = false;
            }
        }
    }
}
