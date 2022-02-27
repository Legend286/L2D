using Box2DX.Dynamics;
using System;

namespace L2D
{
    public class Physics2DBody : GameObjectComponent, IDisposable
    {
        public Body B2DBody; // unique to this instance of the Physics2DBody class
        public BodyDef B2DBodyDef;

        public Physics2DBody(BodyDef body) : base()
        {
            B2DBody = Physics2DWorld.AddBodyToWorld(this);
            B2DBodyDef = body;
        }

        public void Dispose()
        {
            Physics2DWorld.RemoveBodyFromWorld(this);
            B2DBody = null;
        }

    }
}
