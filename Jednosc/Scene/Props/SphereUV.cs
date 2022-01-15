using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Jednosc.Bitmaps;
using Jednosc.Utilities;

namespace Jednosc.Scene.Props
{
    public class SphereUV : RenderObject
    {
        public static SphereUV Create(int parallelCount, int meridianCount, float radius, Material material,
            IReadBitmap texture, IReadBitmap normalMap)
        {
            float dLat = MathF.PI / parallelCount;
            float dLon = 2 * MathF.PI / meridianCount;

            var vertices = new List<Vector4>();
            var textureCoords = new List<Vector3>();
            var indexes = new List<TriangleIndexes>();

            for (int p = 0; p <= parallelCount; ++p)
            {
                float lat = p * dLat;

                for (int m = 0; m <= meridianCount; ++m)
                {
                    float lon = m * dLon;

                    vertices.Add(GetVector4FromSpherical(radius, lat, lon));
                    textureCoords.Add(GetTextureVector(p, m, parallelCount, meridianCount));

                    if (m != meridianCount)
                    {
                        //        p       p + 1
                        // m      a ----- b
                        //        | \     |
                        //        |   \   |
                        //        |     \ |
                        // m + 1  c ----- d

                        int aIndex = GetIndex(p, m, parallelCount, meridianCount);
                        int bIndex = GetIndex(p + 1, m, parallelCount, meridianCount);
                        int cIndex = GetIndex(p, m + 1, parallelCount, meridianCount);
                        int dIndex = GetIndex(p + 1, m + 1, parallelCount, meridianCount);

                        // counterclockwise to use backculling
                        indexes.Add(new TriangleIndexes(aIndex, cIndex, dIndex));
                        indexes.Add(new TriangleIndexes(aIndex, dIndex, bIndex));
                    }
                }
            }

            var normals = vertices.Select(v => new Vector3(v.X, v.Y, v.Z).GetNormalized()).ToArray();

            var indexesArr = indexes.ToArray();

            return new SphereUV(vertices.ToArray(),
                indexesArr, normals, indexesArr, textureCoords.ToArray(), indexesArr,
                material, texture, normalMap);
        }

        private SphereUV(
            Vector4[] vertices, TriangleIndexes[] verticesIndexes,
            Vector3[] normals, TriangleIndexes[] normalsIndexes,
            Vector3[] textures, TriangleIndexes[] textureIndexes,
            Material material, IReadBitmap texture, IReadBitmap normalMap)
            : base(vertices, verticesIndexes, normals, normalsIndexes, textures, textureIndexes, material, texture, normalMap)
        {
        }

        private static int GetIndex(int p, int m, int parallelCount, int meridianCount)
        {
            return (p % (parallelCount + 1)) * (meridianCount + 1) + m;
        }

        private static Vector4 GetVector4FromSpherical(float radius, float lat, float lon)
        {
            return new Vector4()
            {
                X = radius * MathF.Sin(lat) * MathF.Cos(lon),
                Y = radius * MathF.Cos(lat),
                Z = radius * MathF.Sin(lat) * MathF.Sin(lon),
                W = 1f,
            };
        }

        private static Vector3 GetTextureVector(int p, int m, int parallelCount, int meridianCount)
        {
            return new Vector3((float)m / meridianCount, (float)p / parallelCount, 1f);
        }
    }
}
