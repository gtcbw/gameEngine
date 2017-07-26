using System.Drawing;

namespace Engine.Framework
{
    public sealed class BitmapToHeightConverter
    {
        public float[] ConvertBitmap(string filename, float maxHeight)
        {
            Bitmap heightmap = new Bitmap(filename);

            float[] values = new float[heightmap.Width * heightmap.Height];

            for(int z = 0; z< heightmap.Height; z++)
            {
                for (int x = 0; x < heightmap.Width; x++)
                {
                    Color pixel = heightmap.GetPixel(x, heightmap.Height - z - 1);
                    values[x + (z * heightmap.Width)] = pixel.GetBrightness() * maxHeight;
                }
            }

            return values;
        }
    }
}
