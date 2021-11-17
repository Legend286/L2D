using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2D
{
    public static class CameraManager
    {
        private static Camera currentCamera;
        public static Camera GetCurrentCamera()
        {
            return currentCamera;
        }

        public static void SetCurrentCamera(Camera current)
        {
            currentCamera = current;
        }

    }
}
