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
    public class FlatShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;
        private Matrix4x4 _modelIT;

        private Vector3 _textureXs;
        private Vector3 _textureYs;

        private Vector3 _triangleShade;

        public FlatShader(RenderObject prop, Matrix4x4 viewPerspective, Matrix4x4 modelIT, RenderScene scene)
        {
            _scene = scene;
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
            _modelIT = modelIT;
        }


        public Color Fragment(Vector3 bary)
        {
            var textureColor = ShadingUtils.GetBitmapColorVector(bary, _prop.Texture!,
                _textureXs, _textureYs);

            return QMath.GetColorFromVector(_triangleShade * textureColor);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            (_textureXs, _textureYs) = ShadingUtils.GetTextureXYs(iFace, _prop);

            var baryCenter = new Vector3(1 / 3f);
            var posTriangle = vertex4.Transform(_prop.ModelMatrix).Apply(ShadingUtils.VnTo3);
            var normalTriangle = _prop.GetNormals(iFace).TransformNormal(posTriangle, _modelIT);

            var normal = QMath.InterpolateFromBary(normalTriangle, baryCenter);
            var position = QMath.InterpolateFromBary(posTriangle, baryCenter);

            _triangleShade = ShadingUtils.Shading(Vector3.One, position,
                normal, _prop.Material, _scene.Camera, _scene.Lights);

            return vertex4.Apply(_mvp.GetVnMvp());
        }
    }
}
