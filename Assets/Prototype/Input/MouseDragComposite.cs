using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Controls;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Custom_Composites
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif

    public class MouseDragComposite : InputBindingComposite<Vector2>
    {
        static MouseDragComposite()
        {
            InputSystem.RegisterBindingComposite<MouseDragComposite>();
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
        }

        public override Vector2 ReadValue(ref InputBindingCompositeContext context)
        {
            var b = context.ReadValueAsButton(Button);
            var x = context.ReadValue<float>(Axis1);
            var y = context.ReadValue<float>(Axis2);
            var v = new Vector2(x, y);

            return b && v.magnitude > 0.0f ? v : default;
        }

        public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
        {
            return ReadValue(ref context).magnitude;
        }

        #region Fields

        [InputControl(layout = "Button")]
        public int Button;

        [InputControl(layout = "Axis")]
        public int Axis1;

        [InputControl(layout = "Axis")]
        public int Axis2;

        #endregion
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class MouseDragInteraction : IInputInteraction
    {
        static MouseDragInteraction()
        {
            InputSystem.RegisterInteraction<MouseDragInteraction>();
        }

        public void Reset()
        {
        }

        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                context.Performed();
                return;
            }

            var phase = context.phase;

            switch (phase)
            {
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    if (context.ControlIsActuated())
                    {
                        context.Started();
                        context.SetTimeout(float.PositiveInfinity);
                    }

                    break;
                case InputActionPhase.Started:
                    context.PerformedAndStayPerformed();
                    break;
                case InputActionPhase.Performed:
                    if (context.ControlIsActuated())
                    {
                        context.PerformedAndStayPerformed();
                    }
                    else if (!((ButtonControl)context.action.controls[0]).isPressed)
                    {
                        context.Canceled();
                    }

                    break;
                case InputActionPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
        }
    }
}

