using System;
using TMPro;
using UnityEngine;

namespace Util.UI
{
    public class OutputAxes : MonoBehaviour
    {
        [Serializable]
        public struct AxisPair
        {
            public TextMeshProUGUI text;
            public string axis;
        }

        [SerializeField] private AxisPair[] axes;

        private void Update()
        {
            foreach (var pair in axes)
            {
                pair.text.text = Input.GetAxis(pair.axis).ToString();
            }
        }
    }
}