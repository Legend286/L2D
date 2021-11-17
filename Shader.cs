using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using static L2D.ShaderManager;
using OpenTK;

namespace L2D
{
    struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }

    class Shader
    {
        public readonly string Name;
        public int Program { get; private set; }
        private readonly Dictionary<string, int> UniformToLocation = new Dictionary<string, int>();
        private bool Initialized = false;
        public string SourceCode_frag, SourceCode_vert;
        private (ShaderType Type, string Path)[] Files;

        public string inject(ShaderFlags flags)
        {
            String define = ""; //String injection
            foreach (ShaderFlags i in Enum.GetValues(typeof(ShaderFlags)))
            {
                if (flags.HasFlag(i))
                {
                    define += "#define " + Enum.GetName(typeof(ShaderFlags), i) + "\n";
                }

            }
            return define;
        }

        public Shader(string name, string vertexPath, string fragmentPath)
        {
            string VertexShaderSource = "#version 330 core\n";
            string FragmentShaderSource = "#version 330 core\n";

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource += reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource += reader.ReadToEnd();
            }

            Name = name;
            Files = new[]{
                (ShaderType.VertexShader, VertexShaderSource),
                (ShaderType.FragmentShader, FragmentShaderSource),
            };
            Program = CreateProgram(name, Files);
        }

        public Shader(string frag, string vert)
        {
            string VertexShaderSource = vert;
            string FragmentShaderSource = frag;

            SourceCode_frag = FragmentShaderSource;
            SourceCode_vert = VertexShaderSource;

            Files = new[]{
                (ShaderType.VertexShader, VertexShaderSource),
                (ShaderType.FragmentShader, FragmentShaderSource),
            };

            Program = CreateProgram(Name, Files);
        }

        public Shader(ShaderTypeL2D type, ShaderFlags flags)
        {
            string VertexShaderSource = "#version 330 core\n";
            string FragmentShaderSource = "#version 330 core\n";
            string injectionCode = inject(flags);

            FragmentShaderSource += injectionCode;
            VertexShaderSource += injectionCode;

            using (StreamReader reader = new StreamReader("./Shaders/" + type.ToString() + ".vert", Encoding.UTF8))
            {
                VertexShaderSource += reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader("./Shaders/" + type.ToString() + ".frag", Encoding.UTF8))
            {
                FragmentShaderSource += reader.ReadToEnd();
            }

            SourceCode_frag = FragmentShaderSource;
            SourceCode_vert = VertexShaderSource;

            Files = new[]{
                (ShaderType.VertexShader, VertexShaderSource),
                (ShaderType.FragmentShader, FragmentShaderSource),
            };

            Name = type.ToString();

            Program = CreateProgram(Name, Files);
        }


        public void UseShader()
        {
            GL.UseProgram(Program);
        }

        public void Dispose()
        {
            if (Initialized)
            {
                GL.DeleteProgram(Program);
                Initialized = false;
            }
        }

        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out int UniformCount);

            UniformFieldInfo[] Uniforms = new UniformFieldInfo[UniformCount];

            for (int i = 0; i < UniformCount; i++)
            {
                string Name = GL.GetActiveUniform(Program, i, out int Size, out ActiveUniformType Type);

                UniformFieldInfo FieldInfo;
                FieldInfo.Location = GetUniformLocation(Name);
                FieldInfo.Name = Name;
                FieldInfo.Size = Size;
                FieldInfo.Type = Type;

                Uniforms[i] = FieldInfo;
            }

            return Uniforms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string uniform)
        {
            if (UniformToLocation.TryGetValue(uniform, out int location) == false)
            {
                location = GL.GetUniformLocation(Program, uniform);
                UniformToLocation.Add(uniform, location);

                if (location == -1)
                {
                    Debug.Print($"The uniform '{uniform}' does not exist in the shader '{Name}'!");
                }
            }

            return location;
        }

        private int CreateProgram(string name, params (ShaderType Type, string source)[] shaderPaths)
        {
            Util.CreateProgram(name, out int Program);

            int[] Shaders = new int[shaderPaths.Length];
            for (int i = 0; i < shaderPaths.Length; i++)
            {
                Shaders[i] = CompileShader(name, shaderPaths[i].Type, shaderPaths[i].source);
            }

            foreach (var shader in Shaders)
                GL.AttachShader(Program, shader);

            GL.LinkProgram(Program);

            GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out int Success);
            if (Success == 0)
            {
                string Info = GL.GetProgramInfoLog(Program);
                Debug.WriteLine($"GL.LinkProgram had info log [{name}]:\n{Info}");
            }

            foreach (var Shader in Shaders)
            {
                GL.DetachShader(Program, Shader);
                GL.DeleteShader(Shader);
            }

            Initialized = true;

            return Program;
        }

        private int CompileShader(string name, ShaderType type, string source)
        {
            Util.CreateShader(type, name, out int Shader);
            GL.ShaderSource(Shader, source);
            GL.CompileShader(Shader);

            GL.GetShader(Shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string Info = GL.GetShaderInfoLog(Shader);
                Debug.WriteLine($"GL.CompileShader for shader '{Name}' [{type}] had info log:\n{Info}");
            }

            return Shader;
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Program, attribName);
        }

        public void BindInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Program, name);

            GL.Uniform1(location, value);
        }

        public void BindMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Program, name);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void BindFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(Program, name);
            GL.Uniform1(location, value);
        }

        public void BindVector2(string name, Vector2 value)
        {
            int location = GL.GetUniformLocation(Program, name);
            GL.Uniform2(location, value);
        }

        public void BindVector3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(Program, name);
            GL.Uniform3(location, value);
        }

        public void BindVector4(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(Program, name);
            GL.Uniform4(location, value);
        }
    }
}