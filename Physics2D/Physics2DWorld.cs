using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using OpenTK;
using System.Collections.Generic;

namespace L2D
{
    public static class Physics2DWorld
    {
        private static Vec2 GravityVector;
        private static AABB WorldBounds;
        public static World B2DWorld;
        public static List<Physics2DBody> WorldBodies;

        public static void InitialiseWorld(Vector2 Gravity, Vector2 MinExtent, Vector2 MaxExtent)
        {
            GravityVector = new Vec2(Gravity.X, Gravity.Y);
            WorldBounds = new AABB();
            WorldBounds.LowerBound = new Vec2(MinExtent.X, MinExtent.Y);
            WorldBounds.UpperBound = new Vec2(MaxExtent.X, MaxExtent.Y);
            B2DWorld = new World(WorldBounds, GravityVector, false);
        }

        public static World GetWorld()
        {
            return B2DWorld;
        }

        public static void UpdatePhysicsWorld(float dt)
        {
            B2DWorld.Step(dt, 10, 10);
        }

        public static Body AddBodyToWorld(Physics2DBody body)
        {
            return B2DWorld.CreateBody(body.B2DBodyDef);            
        }

        public static void RemoveBodyFromWorld(Physics2DBody body)
        {
            B2DWorld.DestroyBody(body.B2DBody);
            WorldBodies.Remove(body);
        }
    }
}
