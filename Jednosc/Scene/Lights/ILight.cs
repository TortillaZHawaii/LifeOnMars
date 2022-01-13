using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene.Lights
{
    public interface ILight
    {
        public float GetAttenuation(Vector3 position);

        public Vector3 GetLightColor(Vector3 position);

        public Vector3 GetToLightVersor(Vector3 position);
    }
}
