using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene.Examples
{
    public class Cube : RenderObject
    {
        public Cube()
        {
            float A = 1.0f;

            Vertices = new Vector4[]
            {
                new Vector4(-A, A, A, 1), // 1
                new Vector4(A, A, A, 1), // 2
                new Vector4(A, A, -A, 1), // 3
                new Vector4(-A, A, -A, 1), // 4

                new Vector4(-A, -A, A, 1), // 5
                new Vector4(A, -A, A, 1), // 6
                new Vector4(A, -A, -A, 1), // 7
                new Vector4(-A, -A, -A, 1), // 8
            };

            VertexIndexes = new TriangleIndexes[]
            {
                new TriangleIndexes(0, 1, 2),
                new TriangleIndexes(0, 2, 3),

                new TriangleIndexes(0, 4, 5),
                new TriangleIndexes(0, 5, 1),

                new TriangleIndexes(2, 5, 1),
                new TriangleIndexes(2, 6, 5),

                new TriangleIndexes(3, 6, 2),
                new TriangleIndexes(3, 7, 6),

                new TriangleIndexes(3, 6, 2),
                new TriangleIndexes(3, 6, 2),

                new TriangleIndexes(3, 0, 4),
                new TriangleIndexes(3, 7, 4),

                new TriangleIndexes(7, 6, 4),
                new TriangleIndexes(6, 5, 4),
            };
        }
    }
}
