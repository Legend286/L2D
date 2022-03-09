using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;
using OpenTK;
using System;

namespace L2D.Physics2D
{
    public class Physics2DCircleShape : Physics2DShape, IDisposable
    {
        public CircleShape B2DShape = new CircleShape();

        public Physics2DCircleShape(Vector2 position, Vector2 size, float angle, float mass) : base(mass, position, angle)
        {
            B2DShape.Radius = 1.0f;
        }

        public void Dispose()
        {
            B2DShape.Dispose();
            B2DBody.Dispose();
        }
    }
}
