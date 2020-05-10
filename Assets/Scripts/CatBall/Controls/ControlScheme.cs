using UnityEngine;
using Util.Control;

namespace CatBall
{

    [CreateAssetMenu(menuName = "Custom/Input/ControlScheme")]
    public class ControlScheme : ScriptableObject
    {
        public Control1D move;
        public ControlBinary jump;
        public ControlBinary kick;
        public ControlBinary kickCancel;
        public Control2D kickAim;
        public ControlBinary restart;

        public void UpdateControls(GameObject gameObject)
        {
            move.UpdateControl(gameObject);
            jump.UpdateControl(gameObject);
            kick.UpdateControl(gameObject);
            kickCancel.UpdateControl(gameObject);
            kickAim.UpdateControl(gameObject);
            restart.UpdateControl(gameObject);
        }

        public void Set(ControlScheme other)
        {
            move = other.move;
            jump = other.jump;
            kick = other.kick;
            kickCancel = other.kickCancel;
            kickAim = other.kickAim;
            restart = other.restart;
        }
    }
}