#define SHADER_STATE_FIX

using System;
using System.Collections.Generic;
using OpenTK;

namespace L2D
{
    class ShaderManager
    {
        private static Dictionary<Tuple<ShaderFlags, ShaderTypeL2D>, Shader> Shaders = new Dictionary<Tuple<ShaderFlags, ShaderTypeL2D>, Shader>();
        public static Shader CurrentShader;
        private static Shader CurrentActiveShader;

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
            CurrentShader = shader;
        }

        public static Shader GetCurrentShader()
        {
            return CurrentShader;
        }

        public static void UpdateCurrentShader()
        {
            CurrentShader.BindMatrix4("viewprojection", CameraManager.GetCurrentCamera().GetViewAndProjectionMatrix());

        }

        public static void BindModelMatrix(Matrix4 modelMatrix)
        {
            CurrentShader.BindMatrix4("model", modelMatrix);
        }

        public static void UseShader()
        {
#if SHADER_STATE_FIX // FIX Naive shader state system 
            if (CurrentShader != CurrentActiveShader)
            {
                CurrentShader.UseShader();
                CurrentActiveShader = CurrentShader;
            }
            else
            {
                return;
            }
#else
            CurrentShader.UseShader();
#endif
        }
    }
}