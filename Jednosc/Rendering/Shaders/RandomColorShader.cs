using Jednosc.Scene;
using Jednosc.Scene.Props;
using Jednosc.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Rendering.Shaders
{
    internal class RandomColorShader : IShader
    {
        private Color _color;
        private RenderObject _prop;
        private Matrix4x4 _mvp;

        public RandomColorShader(RenderObject prop, Matrix4x4 viewPerspective)
        {
            _color = Color.White;
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
        }

        public Color Fragment(Vector3 bary)
        {
            return _color;
        }

        public Triangle3 Triangle(int iFace)
        {
            var rng = new Random(iFace);
            _color = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256));

            var indexes = _prop.VertexIndexes[iFace];
            var triangle4 = RenderObject.GetTriangle4FromIndexes(indexes, _prop.Vertices);

            return triangle4.Apply(VnMvp);
        }

        private Vector3 VnMvp(Vector4 vectorIn)
        {
            var vector = Vector4.Transform(vectorIn, _mvp);

            return new Vector3(
                vector.X,
                vector.Y,
                vector.Z) 
                / vector.W;
        }
    }
}
