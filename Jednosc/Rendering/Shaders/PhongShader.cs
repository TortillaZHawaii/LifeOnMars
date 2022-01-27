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
    public class PhongShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;
        private Matrix4x4 _modelIT;

        private Triangle3 _positionInDisplayCube;
        private Triangle3 _positionInWorld;
        private Triangle3 _normal;

        private Vector3 _textureXs;
        private Vector3 _textureYs;

        public PhongShader(RenderObject prop, Matrix4x4 viewPerspective, Matrix4x4 modelIT, RenderScene scene)
        {
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
            _scene = scene;
            _modelIT = modelIT;
        }

        public Color Fragment(Vector3 bary)
        {
            var position = QMath.InterpolateFromBary(_positionInWorld, bary);
            var normal = QMath.InterpolateFromBary(_normal, bary);

            var shade = ShadingUtils.Shading(Vector3.One, position, normal, _prop.Material, _scene.Camera, _scene.Lights);
            var colorVector = ShadingUtils.GetBitmapColorVector(bary, _prop.Texture, _textureXs, _textureYs);

            return QMath.GetColorFromVector(shade * colorVector);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            _positionInDisplayCube = vertex4.Apply(_mvp.GetVnMvp());
            (_textureXs, _textureYs) = ShadingUtils.GetTextureXYs(iFace, _prop);

            _positionInWorld = vertex4.Transform(_prop.ModelMatrix).Apply(ShadingUtils.VnTo3);
            _normal = _prop.GetNormals(iFace)
                .TransformNormal(_positionInWorld, _modelIT);

            return _positionInDisplayCube;
        }
    }
}
