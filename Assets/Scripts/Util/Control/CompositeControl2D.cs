using UnityEngine;

namespace Util.Control
{
    public class CompositeControl2D : Control2D
    {
        [SerializeField] private Control1D horizontal;
        [SerializeField] private Control1D vertical;

        public void Init(Control1D x, Control1D y)
        {
            horizontal = x;
            vertical = y;
        }

        public override void UpdateControl(GameObject caller)
        {
            horizontal.UpdateControl(caller);
            vertical.UpdateControl(caller);
        }

        public override Vector2 GetDirection(Transform from)
        {
            return new Vector2(horizontal.GetAxisRaw(), vertical.GetAxisRaw());
        }

        public static CompositeControl2D NewInstance(Control1D x, Control1D y)
        {
            var c = CreateInstance<CompositeControl2D>();
            c.Init(x, y);
            return c;
        }
    }
}