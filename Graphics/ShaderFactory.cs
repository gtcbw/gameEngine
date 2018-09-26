using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;
using Graphics.Shader;

namespace Graphics
{
    public sealed class ShaderFactory
    {
        public int CreateShaderProgram()
        {
            int shaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shaderId, MonochromeShaderV1.Shader);
            GL.CompileShader(shaderId);

            var error = GL.GetShaderInfoLog(shaderId);
            if (error.Length > 0)
            {

            }

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
            int renderedScene = GL.GetUniformLocation(programId, "renderedScene");
            GL.Uniform1(renderedScene, 0);
            int caro = GL.GetUniformLocation(programId, "caro");
            GL.Uniform1(caro, 1);

            int resolutionX = GL.GetUniformLocation(programId, "resolutionX");
            GL.Uniform1(resolutionX, 1920.0f / 10.0f);
            int resolutionY = GL.GetUniformLocation(programId, "resolutionY");
            GL.Uniform1(resolutionY, 1080.0f / 10.0f);
        }

        public void DeactivateShaderProgram()
        {
            GL.UseProgram(0);
        }

        public void DeleteShaderProgram(int programId)
        {
            GL.DeleteProgram(programId);
        }
    }
}
