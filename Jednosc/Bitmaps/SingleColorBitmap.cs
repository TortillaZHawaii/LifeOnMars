using Jednosc.Bitmaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Bitmaps
{
    public class SingleColorBitmap : IReadBitmap
    {
        public int Width => 1;

        public int Height => 1;

        public Color Color;

        public SingleColorBitmap(Color color)
        {
            Color = color;
        }

        public Color GetPixel(int x, int y)
        {
            return Color;
        }
    }
}
