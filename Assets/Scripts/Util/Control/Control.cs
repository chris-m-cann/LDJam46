using UnityEngine;

namespace Util.Control
{
    public abstract class Control : ScriptableObject
    {
        public abstract void UpdateControl(GameObject caller);
    }
}