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
    public class LightShader : IShader
    {
        private RenderScene _scene;
        private RenderObject _prop;
        private Matrix4x4 _mvp;

        private Triangle3 _position;
        private Triangle3 _normal;

        public LightShader(RenderObject prop, Matrix4x4 viewPerspective, RenderScene scene)
        {
            _prop = prop;
            _mvp = _prop.ModelMatrix * viewPerspective;
            _scene = scene;
        }

        public Color? Fragment(Vector3 bary)
        {
            var position = QMath.InterpolateFromBary(_position, bary);
            var normal = QMath.InterpolateFromBary(_normal, bary);

            return Shading(Vector3.One, position, normal);
        }

        public Triangle3 Triangle(int iFace)
        {
            var vertex4 = _prop.GetVertex4(iFace);

            _normal = _prop.GetNormals(iFace).Transform(_prop.ModelMatrix);
            _position = vertex4.Apply(ParseTo3).Transform(_prop.ModelMatrix);
            
            return vertex4.Apply(VnMvp);
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
