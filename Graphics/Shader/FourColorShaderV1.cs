using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Shader
{
    public static class FourColorShaderV1
    {
        public static string Shader = @"

            uniform sampler2D renderedScene;
            uniform float resolutionX;
            uniform float resolutionY;

            vec4 GetColor(float sum, float x, float y)
            {
                if (sum > 2.5)
                {   
                    return vec4(1, 1, 1, 1);
                }
                else if (sum > 1.5)
                {   
                    return vec4(0.75, 0.75, 0.75, 1);
                }
                else if (sum > 1.0)
                {   
                    //float modx = mod(x * 1000.0, 10.0);
                    //float mody = mod(y * 1000.0, 10.0);
                    float modx = mod(x * 1000.0, 5.2);
                    float mody = mod(y * 1000.0, 9.2);
                    if (modx < 2.6)
                    {
                        if (mody < 5.0)
                            return vec4(0.75, 0.75, 0.75, 1);
                        return vec4(0.5, 0.5, 0.5, 1);
                    }
                    if (mody < 5.0)
                            return vec4(0.5, 0.5, 0.5, 1);
                    return vec4(0.75, 0.75, 0.75, 1);
                }
                else if (sum > 0.5)
                {   
                    return vec4(0.5, 0.5, 0.5, 1);
                }
                else
                    return vec4(0.25, 0.25, 0.25, 1);
            }
            float GetBrightnessFactor(float sum, float x, float y)
            {
                if (sum > 2.5)
                {   
                    return 1.0;
                }
                else if (sum > 1.5)
                {   
                    return 0.75;
                }
                else if (sum > 1.0)
                {   
                    float modx = mod(x * 1000.0, 10.0);
                    float mody = mod(y * 1000.0, 10.0);
                    if (modx < 5.0)
                    {
                        if (mody < 5.0)
                            return 0.75;
                        return 0.5;
                    }
                    if (mody < 5.0)
                            return 0.5;
                    return 0.75;
                }
                else if (sum > 0.5)
                {   
                    return 0.5;
                }
                else
                    return 0.25;
            }

            void main()
            {
                gl_FragColor = texture2D(renderedScene, gl_TexCoord[0].xy);
                float sum = gl_FragColor.r + gl_FragColor.g + gl_FragColor.b;
                // gl_FragColor = GetColor(sum, gl_TexCoord[0].x, gl_TexCoord[0].y);
                float factor = GetBrightnessFactor(sum, gl_TexCoord[0].x, gl_TexCoord[0].y);

                gl_FragColor = vec4(factor * 0.5, factor, factor * 0.5, 1);
            }
        ";

    }
}
