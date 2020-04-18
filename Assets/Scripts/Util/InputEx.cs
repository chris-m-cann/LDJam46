using UnityEngine;

namespace Util
{
    public static class InputEx
    {
        public static bool IsControllerConnected()
        {
            return Input.GetJoystickNames().Length > 0;
        }
    }
}