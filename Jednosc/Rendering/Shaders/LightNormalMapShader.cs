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
    public class LightNormalMapShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;

        private Triangle3 _position;

        private Vector3 _textureXs;
        private Vector3 _textureYs;

        public LightNormalMapShader(RenderObject prop, Matrix4x4 viewPerspective, RenderScene scene)
        {
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
            _scene = scene;
        }

        public Color? Fragment(Vector3 bary)
        {
            var position = QMath.InterpolateFromBary(_position, bary);
            var normalFromMap = GetNormalFromMap(bary);
            var normal = Vector3.Transform(normalFromMap, _prop.ModelMatrix);

            return Shading(Vector3.One, position, normal);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            _position = vertex4.Apply(ParseTo3).Transform(_prop.ModelMatrix);

            SetTextureXYs(iFace);
            
            return vertex4.Apply(VnMvp);
        }
        
        private void SetTextureXYs(int iFace)
        {
            var textureIndexes = _prop.TextureIndexes[iFace];
            var textureTriangle = RenderObject.GetTriangle3FromIndexes(textureIndexes, _prop.TextureCoordinates);

            _textureXs = new Vector3(textureTriangle.a.X, textureTriangle.b.X, textureTriangle.c.X);
            _textureYs = new Vector3(textureTriangle.a.Y, textureTriangle.b.Y, textureTriangle.c.Y);
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

        private static Vector3 ParseTo3(Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
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

        private Color Shading(Vector3 ambientColor, Vector3 position, Vector3 normal)
        {
            float ks = .4f; // specular
            float kd = .6f; // diffuse
            float ka = .0f; // ambient
            int alpha = 100; // shininess

            Vector3 ambient = ambientColor * ka;
            Vector3 sum = Vector3.Zero;

            Vector3 toObserver = Vector3.Normalize(position - _scene.Camera.Position);

            foreach (var light in _scene.Lights)
            {
                Vector3 toLight = light.GetVersorFrom(position);
                float lin = Vector3.Dot(toLight, normal);

                Vector3 reflection = Vector3.Normalize(2 * lin * normal - toLight);
                float rv = Vector3.Dot(reflection, toObserver);

                Vector3 diffuse = kd * lin * light.DiffuseLight;
                Vector3 specular = ks * MathF.Pow(rv, alpha) * light.SpecularLight;

                float attenuation = light.GetAttenuation(position);

                sum += attenuation * (diffuse + specular);
            }

            Vector3 colorVector = ambient + sum;

            return QMath.GetColorFromVector(colorVector);
        }
    }
}
