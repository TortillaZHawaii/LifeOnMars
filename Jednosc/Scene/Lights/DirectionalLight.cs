using Jednosc.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene.Lights
{
    public class DirectionalLight : ILight
    {
        public Vector3 Color = Vector3.One;

        /// <summary>
        /// To light direction.
        /// </summary>
        public Vector3 Direction 
        { 
            get => _direction;
            set => _direction = value.GetNormalized(); 
        }

        private Vector3 _direction;

        public DirectionalLight(Vector3 color, Vector3 direction)
        {
            Color = color;
            Direction = direction;
        }

        public DirectionalLight(Vector3 direction)
        {
            Color = Vector3.One;
            Direction = direction;
        }

        public float GetAttenuation(Vector3 position)
        {
            return 1f;
        }

        public Vector3 GetLightColor(Vector3 position)
        {
            return Color;
        }

        public Vector3 GetToLightVersor(Vector3 position)
        {
            return _direction;
        }
    }
}
