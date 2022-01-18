using Jednosc.Bitmaps;
using Jednosc.Scene;
using Jednosc.Scene.Props;
using Jednosc.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LifeOnMars.SceneProps
{
    internal static class PropsFactory
    {
        public static RenderObject GetMars()
        {
            var material = new Material()
            {
                Alpha = 1,
                Ka = 0,
                Kd = 0.8f,
                Ks = 0.2f,
            };

            var texture = new DirectBitmap(Mars.Resources.Texture);
            var normal = new NormalColorBitmap();

            var mars = SphereUV.Create(20, 20, 1, material, texture, normal);
            return mars;
        }

        public static RenderObject GetSun()
        {
            var material = new Material()
            {
                Alpha = 0,
                Ka = 1f,
                Kd = 0f,
                Ks = 0f,
            };

            var texture = new DirectBitmap(Sun.Resources.Texture);
            var normal = new NormalColorBitmap();

            var sun = SphereUV.Create(20, 20, 3f, material, texture, normal);
            return sun;
        }

        public static RenderObject GetWheatley()
        {
            var material = new Material()
            {
                Alpha = 20,
                Ka = 0,
                Ks = 0.9f,
                Kd = 0.6f,
            };

            var texture = new DirectBitmap(Wheatley.Resources.Texture);
            var normal = new NormalColorBitmap();

            var wheatley = RenderObject.FromUTF8Bytes(Wheatley.Resources.Model);
            wheatley.Texture = texture;
            wheatley.Material = material;
            wheatley.NormalMap = normal;

            return wheatley;
        }

        public static RenderObject GetSat()
        {
            var material = new Material()
            {
                Alpha = 20,
                Ka = 0,
                Ks = 0.9f,
                Kd = 0.6f,
            };

            var texture = new DirectBitmap(Sat.Resources.Texture);
            var normal = new NormalColorBitmap();

            var sat = RenderObject.FromUTF8Bytes(Sat.Resources.Model);
            sat.Texture = texture;
            sat.Material = material;
            sat.NormalMap = normal;

            return sat;
        }
    }
}
