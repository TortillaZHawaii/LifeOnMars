using System.Drawing;

namespace Jednosc.Bitmaps
{
    public interface IReadBitmap
    {
        public int Width { get; }
        public int Height { get; }

        public Color GetPixel(int x, int y);
    }
}