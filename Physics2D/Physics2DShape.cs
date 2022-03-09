using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;
using OpenTK;

namespace L2D.Physics2D
{
    public class Physics2DShape
    {
        public Physics2DBody B2DBody;
        public Shape B2DShape;
        private BodyDef B2DBodyDef;

        public Physics2DShape(float mass, Vector2 position, float angle)
        {
            MassData md = new MassData();
            md.Mass = mass;
            B2DBodyDef = new BodyDef();
            B2DBodyDef.Position = new Vec2(position.X, position.Y);
            B2DBodyDef.Angle = angle;
            B2DBodyDef.MassData = md;

            B2DBody = new Physics2DBody(B2DBodyDef);
        }    
    }
}
