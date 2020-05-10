using UnityEngine;

namespace Util.Control
{
    [CreateAssetMenu(menuName = "Control/2D/AxisControl1D")]
    public class AxisControl1D : Control1D
    {
        [SerializeField] private Axis axis;

        public void Init(Axis name)
        {
            axis = name;
        }

        public override void UpdateControl(GameObject caller)
        {

        }

        public override float GetAxisRaw()
        {
            return Input.GetAxisRaw(axis.ToString());
        }

        public static AxisControl1D NewInstance(Axis axisName)
        {
            var i = CreateInstance<AxisControl1D>();
            i.Init(axisName);
            return i;
        }
    }
}