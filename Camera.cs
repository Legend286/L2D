using OpenTK;
using System;

namespace L2D
{
    public enum CameraType
    {
        PERSPECTIVE = 2,
        ORTHOGRAPHIC = 4,
    }
    public class Camera
    {
        private CameraType type;
        public CameraType Type { get { return type; } set { type = value; BuildProjectionMatrix(); } }

        private float orthographicWidth;

        public float OrthographicWidth { get { return orthographicWidth; } set { orthographicWidth = value; BuildProjectionMatrix(); } }
        
        private float orthographicHeight;
        public float OrthographicHeight { get { return orthographicHeight; } set { orthographicHeight = value; BuildProjectionMatrix(); } }

        private float fieldOfView;
        public float FieldOfView { get { return fieldOfView; } set { fieldOfView = value; BuildProjectionMatrix(); } }

        private Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; BuildViewMatrix(); } }

        private Vector3 rotation;
        public Vector3 Rotation { get { return rotation; } set { rotation = value; BuildViewMatrix(); } }


        private Matrix4 cameraProjectionMatrix = Matrix4.Identity;
        private Matrix4 cameraViewMatrix = Matrix4.Identity;

        private float nearPlane;
        public float NearPlane { get { return nearPlane; } set { nearPlane = value; BuildProjectionMatrix(); } }
      
        private float farPlane;
        public float FarPlane { get { return farPlane; } set { farPlane = value; BuildProjectionMatrix(); } }

        public Camera(float orthoWidth, float orthoHeight, float orthoNear, float orthoFar)
        {
            OrthographicWidth = orthoWidth;
            OrthographicHeight = orthoHeight;
            NearPlane = orthoNear;
            FarPlane = orthoFar;
            Type = CameraType.ORTHOGRAPHIC;
            CameraManager.SetCurrentCamera(this);
            BuildProjectionMatrix();
            BuildViewMatrix();
        }

        public Camera(float fov, float perspectiveNear, float perspectiveFar)
        {
            NearPlane = perspectiveNear;
            FarPlane = perspectiveFar;
            FieldOfView = fov;
            Type = CameraType.PERSPECTIVE;
            CameraManager.SetCurrentCamera(this);
            BuildProjectionMatrix();
            BuildViewMatrix();
        }
        private void BuildProjectionMatrix()
        {
            switch(type)
            {
                case CameraType.ORTHOGRAPHIC:
                    {
                        // 2d rendering, 0-100 depth range.
                        Matrix4.CreateOrthographic(orthographicWidth, orthographicHeight, nearPlane, farPlane, out cameraProjectionMatrix);
                        return;
                    }
                    case CameraType.PERSPECTIVE:
                    {
                        // not implemented for 3d camera yet!
                        // ew application.application is messy let's figure out a better way of accessing these variables.
                        Matrix4.CreatePerspectiveFieldOfView(fieldOfView, (float)Application.Engine.Width / (float)Application.Engine.Height, nearPlane, farPlane, out cameraProjectionMatrix);
                        return;
                    }
            }
        }

        private void BuildViewMatrix()
        {

            switch (type)
            {
                case CameraType.ORTHOGRAPHIC:
                    {
                        cameraViewMatrix = Matrix4.LookAt(Position,
                        new Vector3(Position.X, Position.Y, -1.0f),
                        new Vector3(0.0f, 1.0f, 0.0f));
                        return;
                    }
                case CameraType.PERSPECTIVE:
                    {
                        cameraViewMatrix = Matrix4.Identity;
                        return;
                    }
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return cameraViewMatrix;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return cameraProjectionMatrix;
        }

        public Matrix4 GetViewAndProjectionMatrix()
        {
            return cameraViewMatrix * cameraProjectionMatrix;
        }
    }
}
