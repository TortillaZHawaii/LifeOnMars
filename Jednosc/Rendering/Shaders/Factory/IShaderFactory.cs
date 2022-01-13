using Jednosc.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Rendering.Shaders.Factory
{
    public interface IShaderFactory
    {
        public IShader CreateShader(RenderObject prop, Matrix4x4 viewPerspective, Matrix4x4 modelIT, RenderScene scene);
    }
}
