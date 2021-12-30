﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene;

public class RenderScene
{
    public Camera Camera { get; set; }
    public List<Light> Lights { get; init; }
    public List<RenderObject> Objects { get; init; }

    public RenderScene(Camera camera)
    {
        Camera = camera;
        Lights = new List<Light>();
        Objects = new List<RenderObject>();
    }
}