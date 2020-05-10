using UnityEngine;

namespace Util.Control
{
    [CreateAssetMenu(menuName = "Control/2D/AxisDirection")]
    public class AxisDirection : Control2D
    {
        [SerializeField] private Axis horizontalAxis;
        [SerializeField] private Axis verticalAxis;

        public void Init(Axis horizontal, Axis vertical)
        {
            horizontalAxis = horizontal;
            verticalAxis = vertical;
        }

        public override Vector2 GetDirection(Transform from)
        {
            return new Vector2(Input.GetAxis(horizontalAxis.ToString()), Input.GetAxis(verticalAxis.ToString()));
        }

        public override void UpdateControl(GameObject caller)
        {

        }

        public static AxisDirection NewInstance(Axis horizontal, Axis vertical)
        {
            var i = CreateInstance<AxisDirection>();
            i.Init(horizontal, vertical);
            return i;
        }
    }
}