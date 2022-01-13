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
    public class GouraudShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;
        private Matrix4x4 _modelIT;

        private Vector3 _textureXs;
        private Vector3 _textureYs;

        private Triangle3 _triangleShade;

        public GouraudShader(RenderObject prop, Matrix4x4 viewPerspective, Matrix4x4 modelIT, RenderScene scene)
        {
            _scene = scene;
            _prop = prop;
            _modelIT = modelIT;
            _mvp = _prop.ModelMatrix * viewPerspective;
        }

        public Color Fragment(Vector3 bary)
        {
            var textureColor = ShadingUtils.GetBitmapColorVector(bary, _prop.Texture!,
                _textureXs, _textureYs);

            var shade = QMath.InterpolateFromBary(_triangleShade, bary);

            return QMath.GetColorFromVector(shade * textureColor);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            (_textureXs, _textureYs) = ShadingUtils.GetTextureXYs(iFace, _prop);

            var posTriangle = vertex4.Transform(_prop.ModelMatrix).Apply(ShadingUtils.VnTo3);
            var normalTriangle = _prop.GetNormals(iFace)
                .TransformNormal(posTriangle, _modelIT);

            // per vertex shading
            _triangleShade = GetShadingTriangle(posTriangle, normalTriangle);

            return vertex4.Apply(_mvp.GetVnMvp());
        }

        private Triangle3 GetShadingTriangle(Triangle3 positions, Triangle3 normals)
        {
            // per vertex shading
            var shadeA = ShadingWrapper(positions.a, normals.a);
            var shadeB = ShadingWrapper(positions.b, normals.b);
            var shadeC = ShadingWrapper(positions.c, normals.c);

            return new Triangle3(shadeA, shadeB, shadeC);
        }

        private Vector3 ShadingWrapper(Vector3 position, Vector3 normal)
        {
            return ShadingUtils.Shading(Vector3.One, position, normal, _prop.Material, _scene.Camera, _scene.Lights);
        }
    }
}
