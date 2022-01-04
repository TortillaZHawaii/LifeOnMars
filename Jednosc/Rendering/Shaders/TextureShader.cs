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
    public class TextureShader : IShader
    {
        private RenderObject _prop;
        private Matrix4x4 _mvp;
        private Vector3 _textureXs;
        private Vector3 _textureYs;

        public TextureShader(RenderObject prop, Matrix4x4 viewPerspective)
        {
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
        }

        public Color? Fragment(Vector3 bary)
        {
            int textureX = (int)(Vector3.Dot(_textureXs, bary) * (_prop.Texture!.Width - 1));
            int textureY = (int)((1 - Vector3.Dot(_textureYs, bary)) * (_prop.Texture!.Height - 1));

            Color textureColor = _prop.Texture!.GetPixel(textureX, textureY);
            return textureColor;
        }

        public Triangle3 Triangle(int iFace)
        {
            var indexes = _prop.VertexIndexes[iFace];
            var triangle4 = RenderObject.GetTriangle4FromIndexes(indexes, _prop.Vertices);

            var textureIndexes = _prop.TextureIndexes[iFace];
            var textureTriangle = RenderObject.GetTriangle3FromIndexes(textureIndexes, _prop.TextureCoordinates);

            _textureXs = new Vector3(textureTriangle.a.X, textureTriangle.b.X, textureTriangle.c.X);
            _textureYs = new Vector3(textureTriangle.a.Y, textureTriangle.b.Y, textureTriangle.c.Y);

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
