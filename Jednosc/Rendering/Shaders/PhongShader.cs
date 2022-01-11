using Jednosc.Scene;
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
    public class PhongShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;

        private Triangle3 _positionInDisplayCube;
        private Triangle3 _positionInWorld;
        private Triangle3 _normal;

        private Vector3 _textureXs;
        private Vector3 _textureYs;

        public PhongShader(RenderObject prop, Matrix4x4 viewPerspective, RenderScene scene)
        {
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
            _scene = scene;
        }

        public Color? Fragment(Vector3 bary)
        {
            var position = QMath.InterpolateFromBary(_positionInWorld, bary);
            var normal = QMath.InterpolateFromBary(_normal, bary);

            var shade = ShadingUtils.Shading(Vector3.One, position, normal, _prop.Material, _scene.Camera, _scene.Lights);
            var textureColor = GetBitmapColor(bary, _prop.Texture!);
            var colorVector = QMath.GetVectorFromColor(textureColor);

            return QMath.GetColorFromVector(shade * colorVector);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            _positionInDisplayCube = vertex4.Apply(_mvp.GetVnMvp());
            (_textureXs, _textureYs) = ShadingUtils.GetTextureXYs(iFace, _prop);

            _positionInWorld = vertex4.Transform(_prop.ModelMatrix).Apply(ShadingUtils.VnTo3);
            _normal = _prop.GetNormals(iFace)
                .TransformNormal(_positionInWorld, _prop.ModelMatrix);

            return _positionInDisplayCube;
        }
        
        private Color GetBitmapColor(Vector3 bary, DirectBitmap bitmap)
        {
            int textureX = (int)(Vector3.Dot(_textureXs, bary) * (bitmap.Width - 1));
            int textureY = (int)((1 - Vector3.Dot(_textureYs, bary)) * (bitmap.Height - 1));

            return bitmap.GetPixel(textureX, textureY);
        }

        private Vector3 GetNormalFromMap(Vector3 bary)
        {
            var color = GetBitmapColor(bary, _prop.NormalMap!);
            var vector = QMath.GetVectorFromColor(color);

            return Vector3.Normalize(vector * 2 - Vector3.One);
        }

        private Vector3 GetNormalIJ(Vector3 bary)
        {
            Vector3 a0 = _positionInDisplayCube.b - _positionInDisplayCube.a;
            Vector3 a1 = _positionInDisplayCube.c - _positionInDisplayCube.a;
            Vector3 a2 = QMath.InterpolateFromBary(_normal, bary).GetNormalized();

            var ai = new Triangle3(a0, a1, a2);

            Vector3 vectorI = new Vector3(_textureXs.Y - _textureXs.X, _textureXs.Z - _textureXs.X, 0);
            Vector3 vectorJ = new Vector3(_textureYs.Y - _textureYs.X, _textureYs.Z - _textureYs.X, 0);

            Vector3 i = QMath.InterpolateFromBary(ai, vectorI);
            Vector3 j = QMath.InterpolateFromBary(ai, vectorJ);

            // TODO: https://github.com/ssloy/tinyrenderer/blob/907bb561c38e7bd86db8d99678c0108f2e53d54d/main.cpp

            throw new NotImplementedException();
        }
    }
}
