using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Processing.Core;

namespace ExampleUserCode
{
    public class MySketch : Canvas
    {
        IList<Spring> springs;

        private int numSprings = 3;

        public MySketch()
        {
            Size(840, 660);
            Fill = Color.FromArgb(126, 255, 255, 255);
            // Inputs: x, y, mass, gravity
            springs = new List<Spring>();
            for (int i = 0; i < numSprings; i++)
            {
                springs.Add(new Spring(this, Width/2, Height / 2));
            }
        }

        public override void Draw()
        {
            Background(Color.Black);

            Spring last = springs.First();
            last.Update(MouseX, MouseY);
            last.Display(MouseX, MouseY);
            foreach (Spring spring in springs.Skip(1))
            {
                spring.Update(last.x, last.y);
                spring.Display(last.x, last.y);
                last = spring;
            }

        }

        public override void MousePressed(MouseButton button)
        {
            if (button == MouseButton.Right)
            {
                if (springs.Count > 2)
                {
                    springs.Remove(springs.Last());
                }
            }
            if (button == MouseButton.Left)
            {
                springs.Add(new Spring(this, springs.Last().x, springs.Last().y));
            }

        }

        public class Spring
        {
            public float vx, vy; // The x- and y-axis velocities
            public float x, y; // The x- and y-coordinates
            public float gravity = 6.0f;
            public float mass = 4f;
            public float radius = 15f;
            public float stiffness = 0.3f;
            public float damping = 0.7f;
            Canvas _canvas;

            public Spring(Canvas canvas, float xpos, float ypos)
            {
                _canvas = canvas;
                x = xpos;
                y = ypos;
            }

            public void Update(float targetX, float targetY)
            {
                float forceX = (targetX - x) * stiffness;
                float ax = forceX / mass;
                vx = damping * (vx + ax);
                x += vx;
                float forceY = (targetY - y) * stiffness;
                forceY += gravity;
                float ay = forceY / mass;
                vy = damping * (vy + ay);
                y += vy;
            }

            public void Display(float newX, float newY)
            {
                _canvas.Stroke = Color.Transparent;
                _canvas.Ellipse(x - radius, y - radius, radius * 2, radius * 2);
                _canvas.Stroke = Color.White;
                _canvas.Line(x, y, newX, newY);
            }
        }
    }
}
