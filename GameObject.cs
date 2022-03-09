using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2D
{
    public class GameObject
    {
        private Vector2 position = new Vector2(0.0f, 0.0f);
        private float rotation = 0.0f;
        private float scale = 1.0f;
        public Guid UniqueIdentifier;

        public Vector2 Position { get { return position; } set { position = value; UpdateTransform(); } }

        public float Rotation { get { return rotation; } set { rotation = value; UpdateTransform(); } }

        public float Scale { get { return scale; } set { scale = value; UpdateTransform(); } }


        public GameObject(Vector2 pos)
        {
            Position = pos;
            Rotation = 0.0f;
            Scale = 1.0f;
            InitialiseGameObjectUniqueID();
        }

        public virtual void UpdateTransform()
        {
            // Update Transform
        }

        public GameObject(Vector2 pos, float rot)
        {
            Position = pos;
            Rotation = rot;
            Scale = 1.0f;
            InitialiseGameObjectUniqueID();
        }

        public GameObject(Vector2 pos, float rot, float sz)
        {
            Position = pos;
            Rotation = rot;
            Scale = sz;
            InitialiseGameObjectUniqueID();
        }

        void InitialiseGameObjectUniqueID()
        {
            Guid ID = Guid.NewGuid();
            UniqueIdentifier = ID;
        }
    }
}
