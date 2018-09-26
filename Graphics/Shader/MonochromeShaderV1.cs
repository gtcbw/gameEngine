
namespace Graphics.Shader
{
    public static class MonochromeShaderV1
    {
        public static string Shader = @"

            uniform sampler2D renderedScene;
            uniform sampler2D caro;
            uniform float resolutionX;
            uniform float resolutionY;

            float GetBrightnessFactor(float sum, float x, float y)
            {
                if (sum > 2.0)
                {   
                    return 0.75;
                } 
                else if (sum > 1.0)
                {
                    return texture2D(caro, gl_TexCoord[0].xy).r;
                }
                return 0.25;
            }

            void main()
            {
                gl_FragColor = texture2D(renderedScene, gl_TexCoord[0].xy);
                float sum = gl_FragColor.r + gl_FragColor.g + gl_FragColor.b;
                float factor = GetBrightnessFactor(sum, gl_TexCoord[0].x, gl_TexCoord[0].y);

                gl_FragColor = vec4(factor * 0.5, factor, factor * 0.5, 1);
            }
        ";

    }
}
