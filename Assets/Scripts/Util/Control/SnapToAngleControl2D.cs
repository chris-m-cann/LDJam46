using UnityEngine;

namespace Util.Control
{
    public class SnapToAngleControl2D : Control2D
    {
        [SerializeField] private Control2D delegateControl;
        [SerializeField] private float snapDegrees = 10f;

        public void Init(Control2D delegateCntrl)
        {
            delegateControl = delegateCntrl;
        }

        public override void UpdateControl(GameObject caller)
        {
            delegateControl.UpdateControl(caller);
        }

        public override Vector2 GetDirection(Transform from)
        {
            var dir = delegateControl.GetDirection(from);

            var angle = Vector3.Angle(Vector3.up, dir);

            int eighths = (int) (angle / 45);
            float last = angle % 45;

            float finalAngle;
            if (last < snapDegrees)
            {
                finalAngle = eighths * 45;
            }
            else if ((45 - last) < snapDegrees)
            {
                finalAngle = (eighths + 1) * 45;
            }
            else
            {
                finalAngle = eighths * 45 + last;
            }

            if (dir.x > 0) finalAngle *= -1;

            // Debug.Log($"angle={angle}, eighths={eighths}, last={last}, final={finalAngle}");

            return Quaternion.AngleAxis(finalAngle, Vector3.forward) * Vector3.up;
        }

        public static SnapToAngleControl2D NewInstance(Control2D delegateCntrl)
        {
            var i = CreateInstance<SnapToAngleControl2D>();
            i.Init(delegateCntrl);
            return i;
        }
    }
}