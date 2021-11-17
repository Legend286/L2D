using System;
using System.Collections.Generic;
using OpenTK;

namespace L2D
{
    class ShaderManager
    {
        private static Dictionary<Tuple<ShaderFlags, ShaderTypeL2D>, Shader> Shaders = new Dictionary<Tuple<ShaderFlags, ShaderTypeL2D>, Shader>();
        public static Shader currentShader;

        public static int GetShaderCount() { return Shaders.Count; }

        public enum ShaderTypeL2D
        {
            Null = -1,
            ImGui = 2,
            Sprite = 4,
            Light = 8,
            PostProcess = 16,
        };

        [Flags]
        public enum ShaderFlags : long
        {
            NULL = -1,
            SPRITE = 2,
            INSTANCING = 4,
            LIGHT_SPOT = 8,
            LIGHT_POINT = 16,
        };


        public static void put(ShaderTypeL2D type, ShaderFlags flags, Shader shader)
        {
            Shaders[Tuple.Create(flags, type)] = shader;
        }

        public static Shader get(ShaderTypeL2D type, ShaderFlags flags = 0)
        {
            if (Shaders.ContainsKey(Tuple.Create(flags, type)))
            {
                //Return existing shader
                return Shaders[Tuple.Create(flags, type)];
            }
            else
            {
                //Create shader
                Shader shader = new Shader(type, flags);
                Shaders.Add(Tuple.Create(flags, type), shader);
                return shader;
            }
        }

        public static void SetCurrentShader(Shader shader)
        {
            currentShader = shader;
        }

        public static Shader GetCurrentShader()
        {
            return currentShader;
        }

        public static void UpdateCurrentShader()
        {
            currentShader.BindMatrix4("viewprojection", CameraManager.GetCurrentCamera().GetViewAndProjectionMatrix());

        }

        public static void BindModelMatrix(Matrix4 modelMatrix)
        {
            currentShader.BindMatrix4("model", modelMatrix);
        }
        public static void UseShader()
        {
            currentShader.UseShader();
        }
    }
}