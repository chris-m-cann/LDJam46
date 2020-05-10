using System;
using UnityEngine;
using Util.Control;

namespace Util.UI
{
    [CreateAssetMenu(menuName = "Custom/Input/DisplayMappings")]
    public class InputDisplayMappings : ScriptableObject
    {
        [SerializeField] private AxisStringPair[] axisNames;
        [SerializeField] private KeyCodeStringPair[] codeNames;
        [SerializeField] private AxisSpritePair[] axisSprites;
        [SerializeField] private KeyCodeSpritePair[] codeSprites;

        public Either<string, Sprite> GetKeyCodeDisplay(KeyCode code)
        {
            foreach (var codePair in codeSprites)
            {
                if (codePair.first == code) return new Right<string, Sprite>(codePair.second);
            }

            foreach (var codePair in codeNames)
            {
                if (codePair.first == code) return new Left<string, Sprite>(codePair.second);
            }

            // if we didnt find it in the mappings then just a simple tostring
            return new Left<string, Sprite>(code.ToString());
        }

        public Either<string, Sprite> GetAxisDisplay(Axis axis)
        {
            foreach (var axisPair in axisSprites)
            {
                if (axisPair.first == axis) return new Right<string, Sprite>(axisPair.second);
            }

            foreach (var axisPair in axisNames)
            {
                if (axisPair.first == axis) return new Left<string, Sprite>(axisPair.second);
            }

            // if we didnt find it in the mappings then just a simple tostring
            return new Left<string, Sprite>(axis.ToString());
        }
    }
}