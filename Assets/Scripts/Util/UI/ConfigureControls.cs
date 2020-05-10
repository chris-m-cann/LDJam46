using UnityEngine;

namespace Util.UI
{
    public class ConfigureControls : MonoBehaviour
    {
        [SerializeField] private GameObject keyboardOptions;
        [SerializeField] private GameObject controllerOptions;
        [SerializeField] private InputBindingsManager bindings;
        [SerializeField] private SchemeMappings keyboardMappings;
        [SerializeField] private SchemeMappings controllerMappings;

        public void SwitchScheme(int which)
        {
            switch (which)
            {
                case 0:
                    controllerOptions.SetActive(false);
                    keyboardOptions.SetActive(true);
                    bindings.SwitchScheme(keyboardMappings);
                    break;
                case 1:
                    controllerOptions.SetActive(true);
                    keyboardOptions.SetActive(false);
                    bindings.SwitchScheme(controllerMappings);
                    break;
            }
        }
    }
}