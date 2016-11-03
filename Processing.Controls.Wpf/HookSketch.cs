using Processing.Core;
using System;

namespace Processing.Controls.Wpf
{
    internal class HookSketch : Canvas
    {
        internal HookSketch()
        {
        }

        public Action<Canvas> OnDraw { get; set; }
        protected override void Draw()
        {
            OnDraw?.Invoke(this);
        }

        public Action<Canvas> OnMouseMoved { get; set; }
        protected override void MouseMoved()
        {
            OnMouseMoved?.Invoke(this);
        }

        public Action<Canvas, MouseButton> OnMousePressed { get; set; }
        protected override void MousePressed(MouseButton button)
        {
            OnMousePressed?.Invoke(this, button);
        }

        public Action<Canvas, MouseButton> OnMouseReleased { get; set; }
        protected override void MouseReleased(MouseButton button)
        {
            OnMouseReleased?.Invoke(this, button);
        }
    }
}
