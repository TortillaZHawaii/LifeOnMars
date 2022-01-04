using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public record struct TriangleIndexes(int a, int b, int c);
public record struct Triangle2(Vector2 a, Vector2 b, Vector2 c);
public record struct TriangleInt2(Point a, Point b, Point c);
public record struct Triangle3(Vector3 a, Vector3 b, Vector3 c)
{
    public Triangle3 Apply(Func<Vector3, Vector3> func)
    {
        return new Triangle3(func(a), func(b), func(c));
    }

    public Triangle3 Transform(Matrix4x4 matrix)
    {
        return new Triangle3(Vector3.Transform(a, matrix), Vector3.Transform(b, matrix), Vector3.Transform(c, matrix));
    }
}

public record struct Triangle4(Vector4 a, Vector4 b, Vector4 c)
{
    public Triangle3 Apply(Func<Vector4, Vector3> func)
    {
        return new Triangle3(func(a), func(b), func(c));
    }
}

public class RenderObject
{
    /// <summary>
    /// List of geometric vertices, with (x, y, z [,w]) coordinates, w is optional and defaults to 1.0.
    /// In .obj file the representation starts with v.
    /// </summary>
    public Vector4[] Vertices { get; init; }

    /// <summary>
    /// Indexes of vertices that form triangles.
    /// Indexes start at 0 and end at <see cref="Vertices"/> length - 1.
    /// </summary>
    public TriangleIndexes[] VertexIndexes { get; init; }
    public TriangleIndexes[] NormalIndexes { get; init; }

    public TriangleIndexes[] TextureIndexes { get; init; }

    public Vector3[] VertexNormals { get; init; }

    public Vector3[] TextureCoordinates { get; init; }

    /// <summary>
    /// Position of the object in the space.
    /// </summary>
    public Vector3 Position { get; set; } = Vector3.Zero;

    /// <summary>
    /// Unit vector showing direction in which object is orientated.
    /// </summary>
    public Vector3 Forward { get; set; } = Vector3.UnitX;

    /// <summary>
    /// Unit vector showing where up is. It always evaluates to <see cref="Vector3.UnitY"/>.
    /// </summary>
    public static Vector3 Up => Vector3.UnitY;

    public Matrix4x4 ModelMatrix => Matrix4x4.CreateWorld(Position, Forward, Up);

    public DirectBitmap? Texture { get; set; }
    public DirectBitmap? NormalMap { get; set; }

    public RenderObject()
    {
        
    }

    private static RenderObject ProcessObjLines(string[] lines)
    {
        var triangleIndexes = new List<TriangleIndexes>();
        var textureIndexes = new List<TriangleIndexes>();
        var normalIndexes = new List<TriangleIndexes>();

        var vertices = new List<Vector4>();
        var vertexNormals = new List<Vector3>();
        var texturesCoords = new List<Vector3>();

        foreach (var line in lines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            bool isLineWorthAnalysing = parts.Length > 1;
            if (!isLineWorthAnalysing)
                continue;

            switch (parts[0])
            {
                case "v": // Vertex
                    vertices.Add(GetVertexFromParts(parts));
                    break;
                case "f": // Indices
                    var faces = GetFacesIndicesFromParts(parts);
                    triangleIndexes.Add(faces.vertex);
                    if (faces.texture != null)
                        textureIndexes.Add(faces.texture.Value);
                    if (faces.normal != null)
                        normalIndexes.Add(faces.normal.Value);
                    else
                        normalIndexes.Add(faces.vertex);
                    break;
                case "vn": // vertex normal
                    vertexNormals.Add(GetVertexNormalFromParts(parts));
                    break;
                case "vt": // texture
                    texturesCoords.Add(GetVector3FromParts(parts));
                    break;
                case "vp": // space vertices
                    break;

            }
        }

        return new RenderObject()
        {
            Vertices = vertices.ToArray(),
            VertexIndexes = triangleIndexes.ToArray(),
            VertexNormals = vertexNormals.ToArray(),
            TextureIndexes = textureIndexes.ToArray(),
            TextureCoordinates = texturesCoords.ToArray(),
            NormalIndexes = normalIndexes.ToArray(),
        };
    }

    public static RenderObject FromFilename(string filename)
    {
        string[] lines = File.ReadAllLines(filename);

        return ProcessObjLines(lines);
    }

    public static async Task<RenderObject> FromFilenameAsync(string filename)
    {
        string[] lines = await File.ReadAllLinesAsync(filename);

        return ProcessObjLines(lines);
    }

    public static Triangle3 GetTriangle3FromIndexes(TriangleIndexes indexes, Vector3[] tab)
    {
        return new Triangle3(tab[indexes.a], tab[indexes.b], tab[indexes.c]);
    }

    public static Triangle4 GetTriangle4FromIndexes(TriangleIndexes indexes, Vector4[] tab)
    {
        return new Triangle4(tab[indexes.a], tab[indexes.b], tab[indexes.c]);
    }

    private static (TriangleIndexes vertex, TriangleIndexes? texture, TriangleIndexes? normal)
        GetFacesIndicesFromParts(string[] parts)
    {
        var separated = parts.Skip(1) // skip f
            .Select(x =>
                x.Split('/', StringSplitOptions.RemoveEmptyEntries).Select(y =>
                    int.Parse(y) - 1 // we move indexes so that they start at 0
                ).ToArray()
            ).ToArray();

        var vertex = new TriangleIndexes(separated[0][0], separated[1][0], separated[2][0]);

        bool hasTexture = separated.GetLength(0) > 1 && separated[0][1] != -1;
        bool hasNormals = separated.GetLength(0) == 3;

        TriangleIndexes? texture = hasTexture ?
            new TriangleIndexes(separated[0][1], separated[1][1], separated[2][1]) : null;
        TriangleIndexes? normals = hasNormals ?
            new TriangleIndexes(separated[0][2], separated[1][2], separated[2][2]) : null;

        return (vertex, texture, normals);
    }

    private static Vector4 GetVertexFromParts(string[] array)
    {
        // default to 1
        float w = array.Length == 5 ? float.Parse(array[4]) : 1.0f;

        return new Vector4()
        {
            X = float.Parse(array[1]),
            Y = float.Parse(array[2]),
            Z = float.Parse(array[3]),
            W = w,
        };
    }

    private static Vector3 GetVertexNormalFromParts(string[] array)
    {
        var vector = GetVector3FromParts(array);
        return Vector3.Normalize(vector);
    }

    private static Vector3 GetVector3FromParts(string[] array)
    {
        return new Vector3()
        {
            X = float.Parse(array[1]),
            Y = array.Length > 2 ? float.Parse(array[2]) : 0f,
            Z = array.Length > 3 ? float.Parse(array[3]) : 0f,
        };
    }

    public void LoadTextureFromFilename(string filename)
    {
        var bitmap = new Bitmap(filename);

        Texture = new DirectBitmap(bitmap);
    }

    public void LoadNormalMapFromFilename(string filename)
    {
        var bitmap = new Bitmap(filename);

        NormalMap = new DirectBitmap(bitmap);
    }

    public Triangle4 GetVertex4(int iFace)
    {
        var indexes = VertexIndexes[iFace];
        return GetTriangle4FromIndexes(indexes, Vertices);
    }

    public Triangle3 GetNormals(int iFace)
    {
        var indexes = NormalIndexes[iFace];
        return GetTriangle3FromIndexes(indexes, VertexNormals);
    }
}
