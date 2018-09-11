using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class ShaderFactory
    {
        public int CreateShaderProgram()
        {
            int shaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shaderId, fragmentShader);
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
        }

        public void DeactivateShaderProgram()
        {
            GL.UseProgram(0);
        }

        public void DeleteShaderProgram(int programId)
        {
            GL.DeleteProgram(programId);
        }

        private static string fragmentShader = @"

            uniform sampler2D renderedScene;

            vec4 GetColor(float sum)
            {
                if (sum > 2.5)//(sum > 573)
                {   
                    return vec4(1, 1, 1, 1);
                }
                else if (sum > 1.5)//(sum > 382)
                {   
                    return vec4(0.75, 0.75, 0.75, 1);
                }
                else if (sum > 0.5)//(sum > 191)
                {   
                    return vec4(0.5, 0.5, 0.5, 1);
                }
                else
                    return vec4(0.25, 0.25, 0.25, 1);
            }

            void main()
            {
                gl_FragColor = texture2D(renderedScene, gl_TexCoord[0].xy);
                //gl_FragColor.r = 1.0;
                //float sum = gl_FragColor.r + gl_FragColor.g + gl_FragColor.b;
                //gl_FragColor = GetColor(sum);
            }
        ";
    }
}
