using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Jednosc;

/// <remarks>
/// Adapted from https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
/// </remarks>
public class DirectBitmap : IDisposable
{
    public Bitmap Bitmap { get; private set; }
    public int[] Bits { get; private set; }
    public bool Disposed { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    protected GCHandle BitsHandle { get; private set; }

    public DirectBitmap(int width, int height)
    {
        Width = width;
        Height = height;
        Bits = new int[width * height];
        BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
        Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
    }

    public DirectBitmap(Bitmap bitmap) : this(bitmap.Width, bitmap.Height)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                SetPixel(i, j, bitmap.GetPixel(i, j));
            }
        }
    }

    public DirectBitmap(DirectBitmap bitmap) : this(bitmap.Width, bitmap.Height)
    {
        this.CopyFrom(bitmap);
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (IsOutside(x, y))
            return;

        int index = x + y * Width;
        int col = color.ToArgb();

        Bits[index] = col;
    }

    public Color GetPixel(int x, int y)
    {
        if (IsOutside(x, y))
            return Color.Empty;

        int index = x + y * Width;
        int col = Bits[index];
        Color result = Color.FromArgb(col);

        return result;
    }

    public bool IsOutside(int x, int y)
    {
        return x < 0 || y < 0 || x >= Width || y >= Height;
    }

    public void CopyFrom(DirectBitmap bitmap)
    {
        if (bitmap.Width != Width || bitmap.Height != Height)
        {
            throw new ArgumentException($"Incompatible size. Destination width: {Width}, height: {Height}. Source width: {bitmap.Width}, height: {bitmap.Height}.");
        }

        Array.Copy(bitmap.Bits, Bits, Bits.Length);
    }

    public void Dispose()
    {
        if (Disposed) return;
        Disposed = true;
        Bitmap.Dispose();
        BitsHandle.Free();
        GC.SuppressFinalize(this);
    }
}
