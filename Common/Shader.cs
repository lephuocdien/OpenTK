using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;
using OpenTK;

namespace Common
{
    public class Shader
    {
        int Handle;
        private bool disposedValue = false;
        private readonly Dictionary<string, int> _uniformLocations;
        public  void Use()
        {
            GL.UseProgram(Handle);
        }
        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;
            Stream streamVertex = new FileStream(vertexPath, FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(streamVertex))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;
            Stream streamFragment = new FileStream(fragmentPath, FileMode.Open, FileAccess.Read,
                       FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(streamFragment))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            var  VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            var  FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }

        }
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.UseProgram(0);
                GL.DeleteProgram(Handle);
               
                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
