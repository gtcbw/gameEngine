using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class ShaderFactory
    {
        private int _emptyProgram;

        public int CreateShaderProgram()
        {
            // empty for program for standard rendering
            _emptyProgram = GL.CreateProgram();
            GL.LinkProgram(_emptyProgram);

            int shaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shaderId, fragmentShader);
            GL.CompileShader(shaderId);

            int programId = GL.CreateProgram();
            GL.AttachShader(programId, shaderId);

            GL.LinkProgram(programId);

            GL.DetachShader(programId, shaderId);
            GL.DeleteShader(shaderId);

            return programId;
        }

        public void ActivateShaderProgram(int programId)
        {
            GL.UseProgram(programId);
        }

        public void DeactivateShaderProgram()
        {
            GL.UseProgram(_emptyProgram);
        }

        public void DeleteShaderProgram(int programId)
        {
            GL.DeleteProgram(programId);
        }

        private static string fragmentShader = @"
            out vec3 color;
            void main()
            {
                color = vec3(1,0,0);
            }
        ";
    }
}
