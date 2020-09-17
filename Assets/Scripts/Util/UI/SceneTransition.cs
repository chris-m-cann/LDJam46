using System;
using UnityEngine;

namespace Util.UI
{
    [Serializable]
    public struct SceneTransition
    {
        public Texture2D texture;
        public float duration;
        public float smoothing;
        public float rotation;
        public int weight;
    }
}