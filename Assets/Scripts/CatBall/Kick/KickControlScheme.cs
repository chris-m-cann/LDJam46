using UnityEngine;
using Util;
using Util.Control;

namespace CatBall
{
    public abstract class KickControlScheme: ScriptableObject
    {
        public abstract void UpdateControls(GameObject caller);
        public abstract bool KickButtonDown();
        public abstract void KickButtonHandled();
        public abstract bool CancelButtonDown();
        public abstract void CancelButtonHandled();

        public abstract Vector2 GetDirection();
    }
}