using Jednosc.Scene.Lights;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public class RenderScene
{
    public Camera Camera { get; set; }
    public List<ILight> Lights { get; init; }
    public List<RenderObject> Objects { get; init; }

    public Color BackgroundColor { get; set; } = Color.Black;

    public RenderScene(Camera camera)
    {
        Camera = camera;
        Lights = new List<ILight>();
        Objects = new List<RenderObject>();
    }
}
