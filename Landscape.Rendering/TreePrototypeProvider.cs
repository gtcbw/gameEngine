using Math.Contracts;

namespace Landscape.Rendering
{
    public class TreePrototypeProvider
    {
        private IVectorHelper _vectorHelper;

        public TreePrototypeProvider(IVectorHelper vectorHelper)
        {
            _vectorHelper = vectorHelper;
        }

        public float[][] GetPrototype(double width, double height)
        {
            float[][] vertices = new float[8][];

            for (int i = 0; i < 8; i++)
            {
                vertices[i] = new float[12];

                var vector = _vectorHelper.ConvertDegreeToVector(i * 45);

                vertices[i][0] =(float) (vector.X * width / 2.0);
                vertices[i][1] = 0f;
                vertices[i][2] = (float)(vector.Z * width / 2.0);

                vertices[i][3] = (float)(vector.X * width / 2.0);
                vertices[i][4] = (float)height;
                vertices[i][5] = (float)(vector.Z * width / 2.0);

                vector = _vectorHelper.Rotate180Degree(vector);

                vertices[i][6] = (float)(vector.X * width / 2.0);
                vertices[i][7] = (float)height;
                vertices[i][8] = (float)(vector.Z * width / 2.0);

                vertices[i][9] = (float)(vector.X * width / 2.0);
                vertices[i][10] = 0f;
                vertices[i][11] = (float)(vector.Z * width / 2.0);
            }

            return vertices;
        }
    }
}
