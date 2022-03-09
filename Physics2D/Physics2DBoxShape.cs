using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;
using OpenTK;
using System;

namespace L2D.Physics2D
{
    public class Physics2DBoxShape : Physics2DShape, IDisposable
    {
        public PolygonShape B2DShape = new PolygonShape();

        public Physics2DBoxShape(Vector2 position, Vector2 size, float angle, float mass) : base(mass, position, angle)
        {
            B2DShape.SetAsBox(size.X, size.Y, new Vec2(position.X, position.Y), angle);
        }

        public void Dispose()
        {
            B2DShape.Dispose();
            B2DBody.Dispose();
        }
    }
}
