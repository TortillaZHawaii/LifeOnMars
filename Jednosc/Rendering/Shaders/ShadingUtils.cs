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
    internal static class ShadingUtils
    {
        /// <summary>
        /// Gets the function that transforms vector using <paramref name="mvp"/>.
        /// Then translates that <see cref="Vector4"/> to <see cref="Vector3"/>.
        /// </summary>
        /// <param name="mvp">Model * View * Perspective matrix</param>
        /// <returns>Functor to transform <see cref="Vector4"/> to <see cref="Vector3"/>.</returns>
        public static Func<Vector4, Vector3> GetVnMvp(this Matrix4x4 mvp)
        {
            return (Vector4 vectorIn) =>
            {
                var vector = Vector4.Transform(vectorIn, mvp);

                return new Vector3(
                    vector.X,
                    vector.Y,
                    vector.Z)
                    / vector.W;
            };
        }

        public static Triangle3 TransformNormal(this Triangle3 normals, Triangle3 positionsAfterTransform, Matrix4x4 matrix)
        {
            var na = new Vector4(normals.a, Vector3.Dot(normals.a, positionsAfterTransform.a));
            var nb = new Vector4(normals.b, Vector3.Dot(normals.b, positionsAfterTransform.b));
            var nc = new Vector4(normals.c, Vector3.Dot(normals.c, positionsAfterTransform.c));

            var t4 = new Triangle4(na, nb, nc);
            var transformed = t4.Transform(matrix);
            var t3 = transformed.Apply(ParseTo3).Apply(Vector3.Normalize);
            return t3;
        }

        public static Color GetBitmapColor(Vector3 bary, DirectBitmap bitmap, Vector3 textureXs,
            Vector3 textureYs)
        {
            int textureX = (int)(Vector3.Dot(textureXs, bary) * (bitmap.Width - 1));
            int textureY = (int)((1 - Vector3.Dot(textureYs, bary)) * (bitmap.Height - 1));

            return bitmap.GetPixel(textureX, textureY);
        }

        public static Vector3 GetBitmapColorVector(Vector3 bary, DirectBitmap bitmap,
            Vector3 textureXs, Vector3 textureYs)
        {
            var color = GetBitmapColor(bary, bitmap, textureXs, textureYs);
            return QMath.GetVectorFromColor(color);
        }

        public static (Vector3 textureXs, Vector3 textureYs) GetTextureXYs(int iFace, RenderObject prop)
        {
            var textureIndexes = prop.TextureIndexes[iFace];
            var textureTriangle = RenderObject.GetTriangle3FromIndexes(textureIndexes,
                prop.TextureCoordinates);

            var textureXs = new Vector3(textureTriangle.a.X, textureTriangle.b.X, textureTriangle.c.X);
            var textureYs = new Vector3(textureTriangle.a.Y, textureTriangle.b.Y, textureTriangle.c.Y);
        
            return (textureXs, textureYs);
        }

        public static Vector3 Shading(Vector3 ambientColor, Vector3 position, Vector3 normal,
            Material material, Camera camera, IEnumerable<Light> lights)
        {
            Vector3 ambient = ambientColor * material.Ka;
            
            Vector3 sum = Vector3.Zero;
            Vector3 toObserver = Vector3.Normalize(camera.Position - position);

            foreach (var light in lights)
            {
                Vector3 toLight = light.GetVersorFrom(position);
                float lin = MathF.Max(0, Vector3.Dot(toLight, normal));

                Vector3 reflection = Vector3.Normalize(2 * lin * normal - toLight);
                float rv = MathF.Max(0, Vector3.Dot(reflection, toObserver));

                Vector3 diffuse = material.Kd * lin * light.DiffuseLight;
                Vector3 specular = material.Ks * MathF.Pow(rv, material.Alpha) * light.SpecularLight;

                float attenuation = light.GetAttenuation(position);

                sum += attenuation * (diffuse + specular);
            }

            Vector3 colorVector = ambient + sum;

            return colorVector;
        }

        public static Vector3 ParseTo3(this Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        }

        public static Vector3 VnTo3(this Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z) / vector4.W;
        }
    }
}
