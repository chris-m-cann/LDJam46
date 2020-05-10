using UnityEngine;
using Util;
using Util.Control;
using Util.UI;

namespace CatBall
{
    public class CatBallControlSchemeGenerator : ControlSchemeGenerator
    {
        public SchemeMappings mappings;

        public override void SetSchemeMappings(SchemeMappings mappings)
        {
            this.mappings = mappings;
        }

        public override ControlScheme GenerateScheme()
        {
            var defaultRestartBinding = new InputBindingsManager.ControlBinding
            {
                key = KeyCode.R
            };
            var bindings = mappings.CreateControlBindings();

            var jump = CreateBinaryControl(bindings[PlayerAction.Jump]);
            var kick = CreateBinaryControl(bindings[PlayerAction.Kick]);
            var kickCancel = CreateBinaryControl(bindings[PlayerAction.KickCancel]);
            var aim = CreateControl2D(bindings[PlayerAction.KickAim].axis);
            var restart = CreateBinaryControl(bindings.GetValue(PlayerAction.Restart, defaultRestartBinding));

            Control1D move;
            if (bindings.ContainsKey(PlayerAction.Move))
            {
                move = Create1DControl(bindings[PlayerAction.Move].axis);
            }
            else
            {
                var left = CreateBinaryControl(bindings[PlayerAction.Left]);
                var right = CreateBinaryControl(bindings[PlayerAction.Right]);
                move = TwoButtonControl1D.NewInstance(left, right);
            }

            var controls = ScriptableObject.CreateInstance<ControlScheme>();
            controls.jump = jump;
            controls.kick = kick;
            controls.kickCancel = kickCancel;
            controls.move = move;
            controls.kickAim = SnapToAngleControl2D.NewInstance(aim);
            controls.restart = restart;

            return controls;
        }
    }
}