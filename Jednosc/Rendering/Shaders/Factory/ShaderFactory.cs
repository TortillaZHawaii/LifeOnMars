using Jednosc.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Rendering.Shaders.Factory
{
    public enum Shaders
    {
        Flat,
        Gouroud,
        Phong,
    }

    public class ShaderFactory : IShaderFactory
    {
        public Shaders SelectedShader = Shaders.Phong;

        public ShaderFactory()
        {

        }


        public IShader CreateShader(RenderObject prop, Matrix4x4 viewPerspective, Matrix4x4 modelIT, RenderScene scene)
        {
            return SelectedShader switch
            {
                Shaders.Flat => new FlatShader(prop, viewPerspective, modelIT, scene),
                Shaders.Gouroud => new GouraudShader(prop, viewPerspective, modelIT, scene),
                Shaders.Phong => new PhongShader(prop, viewPerspective, modelIT, scene),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
