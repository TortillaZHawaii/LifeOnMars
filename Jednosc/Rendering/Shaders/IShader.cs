using Jednosc.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Rendering.Shaders
{
    public interface IShader
    {
        public Triangle3 Triangle(int iFace);

        public Color? Fragment(Vector3 bary);
    }
}
