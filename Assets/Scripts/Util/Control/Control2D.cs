using UnityEngine;

namespace Util.Control
{
    public abstract class Control2D : Control
    {
        public abstract Vector2 GetDirection(Transform from);
    }
}