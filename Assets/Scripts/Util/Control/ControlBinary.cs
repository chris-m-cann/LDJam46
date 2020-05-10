using UnityEngine;

namespace Util.Control
{

    public abstract class ControlBinary : Control
    {
        public abstract bool IsDown();
        public abstract bool WasPressed();
        public abstract bool WasReleased();

        public abstract void Handled();
    }
}