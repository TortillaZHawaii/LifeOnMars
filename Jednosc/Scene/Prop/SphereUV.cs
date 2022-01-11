using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Jednosc.Utilities;

namespace Jednosc.Scene.Prop
{
    public class SphereUV : RenderObject
    {
        private int _parallelCount;
        private int _meridianCount;

        public SphereUV(int parallelCount, int meridianCount, float radius)
        {
            _meridianCount = meridianCount;
            _parallelCount = parallelCount;

            float dLat = MathF.PI / parallelCount;
            float dLon = 2 * MathF.PI / meridianCount;

            var vertices = new List<Vector4>();
            var textureCoords = new List<Vector3>();
            var indexes = new List<TriangleIndexes>();

            for(int p = 0; p <= parallelCount; ++p)
            {
                float lat = p * dLat;

                for(int m = 0; m <= meridianCount; ++m)
                {
                    float lon = m * dLon;

                    vertices.Add(GetVector4FromSpherical(radius, lat, lon));
                    textureCoords.Add(GetTextureVector(p, m));

                    if(m != meridianCount)
                    {
                        //        p       p + 1
                        // m      a ----- b
                        //        | \     |
                        //        |   \   |
                        //        |     \ |
                        // m + 1  c ----- d

                        int aIndex = GetIndex(p, m);
                        int bIndex = GetIndex(p + 1, m);
                        int cIndex = GetIndex(p, m + 1);
                        int dIndex = GetIndex(p + 1, m + 1);

                        // counterclockwise to use backculling
                        indexes.Add(new TriangleIndexes(aIndex, cIndex, dIndex));
                        indexes.Add(new TriangleIndexes(aIndex, dIndex, bIndex));
                    }
                }
            }

            var normals = vertices.Select(v => new Vector3(v.X, v.Y, v.Z).GetNormalized()).ToArray();

            VertexIndexes = NormalIndexes = TextureIndexes = indexes.ToArray();
            VertexNormals = normals;
            TextureCoordinates = textureCoords.ToArray();
            Vertices = vertices.ToArray();
        }

        private int GetIndex(int p, int m)
        {
            return (p % (_parallelCount + 1)) * (_meridianCount + 1) + m;
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

        private Vector3 GetTextureVector(int p, int m)
        {
            return new Vector3((float)m / _meridianCount, (float)p / _parallelCount, 1f);
        }
    }
}
