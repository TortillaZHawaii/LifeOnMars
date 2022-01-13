using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Bitmaps
{
    public class NormalColorBitmap : SingleColorBitmap
    {
        private static Color DefaultNormalColor = Color.FromArgb(128, 128, 255); 

        public NormalColorBitmap() : base(DefaultNormalColor)
        {
        }
    }
}
