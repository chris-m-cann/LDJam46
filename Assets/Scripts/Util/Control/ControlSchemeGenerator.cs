using System;
using CatBall;
using UnityEngine;
using Util.UI;

namespace Util.Control
{
    public abstract class ControlSchemeGenerator : MonoBehaviour
    {
        public abstract void SetSchemeMappings(SchemeMappings mappings);
        public abstract ControlScheme GenerateScheme();

        protected ControlBinary CreateBinaryControl(InputBindingsManager.ControlBinding binding)
        {
            ControlBinary binary;
            if (binding.key.HasValue)
            {
                binary= ControlKey.NewInstance(binding.key.Value);
            }
            else
            {
                binary = AxisControlBinary.NewInstance(binding.axis);
            }

            return binary;
        }

        // is vertical is just used in the occasion when a 2D axis is supplied, need to decide to take the vertical or horizontal
        protected Control1D Create1DControl(Axis axis, bool isVertical = false)
        {
            switch (axis)
            {
                case Axis.None:
                    // doh!
                    throw new ArgumentException("None supplied for input axis");
                case Axis.MouseDirection:
                    throw new ArgumentException("no 1d axis to be made out of a mouse direction");
                case Axis.ArrowKeys:
                    var negative = isVertical ? KeyCode.DownArrow : KeyCode.LeftArrow;
                    var positive = isVertical ? KeyCode.UpArrow : KeyCode.RightArrow;
                    return TwoButtonControl1D.NewInstance(ControlKey.NewInstance(negative),
                        ControlKey.NewInstance(positive));
                case Axis.RightStick:
                    return AxisControl1D.NewInstance(isVertical ? Axis.RightStickY : Axis.RightStickX);
                case Axis.LeftStick:
                    return AxisControl1D.NewInstance(isVertical ? Axis.LeftStickY : Axis.LeftStickX);
                default:
                    return AxisControl1D.NewInstance(axis);
            }
        }

        protected Control2D CreateControl2D(Axis axis)
        {
            switch (axis)
            {
                case Axis.ArrowKeys:
                    return CompositeControl2D.NewInstance(
                        TwoButtonControl1D.NewInstance(ControlKey.NewInstance(KeyCode.LeftArrow),
                            ControlKey.NewInstance(KeyCode.RightArrow)),
                        TwoButtonControl1D.NewInstance(ControlKey.NewInstance(KeyCode.DownArrow),
                            ControlKey.NewInstance(KeyCode.UpArrow))
                    );
                case Axis.WASD:
                    return CompositeControl2D.NewInstance(
                        TwoButtonControl1D.NewInstance(ControlKey.NewInstance(KeyCode.A),
                            ControlKey.NewInstance(KeyCode.D)),
                        TwoButtonControl1D.NewInstance(ControlKey.NewInstance(KeyCode.S),
                            ControlKey.NewInstance(KeyCode.W))
                    );
                case Axis.RightStick:
                    return AxisDirection.NewInstance(Axis.RightStickX, Axis.RightStickY);
                case Axis.LeftStick:
                    return AxisDirection.NewInstance(Axis.LeftStickX, Axis.LeftStickY);
                case Axis.MouseDirection :
                    return MouseDirection.NewInstance();
                default:
                    throw new ArgumentException($"Axis.{axis} isnt a valid 2D axis");
            }
        }
    }
}