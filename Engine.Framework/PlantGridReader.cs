using System.Drawing;

namespace Engine.Framework
{
    public class PlantGridReader
    {
        public bool[][] ConvertBitmapToGridByColor(string filename, int red, int green, int blue)
        {
            Bitmap heightmap = new Bitmap(filename);

            bool[][] values = new bool[heightmap.Height][];

            for (int z = 0; z < heightmap.Height; z++)
            {
                values[z] = new bool[heightmap.Width];

                for (int x = 0; x < heightmap.Width; x++)
                {
                    Color pixel = heightmap.GetPixel(x, heightmap.Height - z - 1);

                    values[z][x] = pixel.R == red && pixel.G == green && pixel.B == blue;
                }
            }
            heightmap.Dispose();

            return values;
        }
    }
}
