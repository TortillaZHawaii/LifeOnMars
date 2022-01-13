using Jednosc.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene.Lights
{
    public class SpotLight : ILight
    {
        public Vector3 Color = Vector3.One;
        public int Power = 5;

        public float Ac = 1f;
        public float Al = 0.09f;
        public float Aq = 0.032f;

        /// <summary>
        /// To light direction.
        /// </summary>
        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value.GetNormalized();
        }

        public Vector3 Position;


        private Vector3 _direction;

        public SpotLight(Vector3 position, Vector3 color, Vector3 direction)
        {
            Position = position;
            Color = color;
            Direction = direction;
        }

        public SpotLight(Vector3 position, Vector3 direction) : this(position, Vector3.One, direction)
        { 
        }


        public float GetAttenuation(Vector3 pixelPosition)
        {
            Vector3 r = Position - pixelPosition;
            float distance = r.Length();

            return 1f / (Ac + Al * distance + Aq * distance * distance);
        }

        public Vector3 GetLightColor(Vector3 position)
        {
            var toLight = GetToLightVersor(position);

            float dl = Vector3.Dot(-Direction, toLight);
            
            return Color * MathF.Pow(dl, Power);
        }

        public Vector3 GetToLightVersor(Vector3 position)
        {
            return Vector3.Normalize(Position - position);
        }
    }
}
