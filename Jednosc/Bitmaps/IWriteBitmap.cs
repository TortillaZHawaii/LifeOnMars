using System.Drawing;

namespace Jednosc.Bitmaps
{
    public interface IWriteBitmap
    {
        public void SetPixel(int x, int y, Color color);
    }
}