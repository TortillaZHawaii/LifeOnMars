using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jednosc.Scene
{
    public struct Material
    {
        /// <summary>
        /// Specular coeeficient.
        /// </summary>
        /// <remarks>
        /// <see cref="Ks"/> +
        /// <see cref="Kd"/> +
        /// <see cref="Ka"/> should be equal to 1 to avoid artefacts 
        /// </remarks>
        public float Ks = .2f;

        /// <summary>
        /// Diffuse coeeficient.
        /// </summary>
        /// <remarks>
        /// <see cref="Ks"/> +
        /// <see cref="Kd"/> +
        /// <see cref="Ka"/> should be equal to 1 to avoid artefacts 
        /// </remarks>
        public float Kd = .5f;

        /// <summary>
        /// Ambient coeeficient.
        /// </summary>
        /// <remarks>
        /// <see cref="Ks"/> +
        /// <see cref="Kd"/> +
        /// <see cref="Ka"/> should be equal to 1 to avoid artefacts 
        /// </remarks>
        public float Ka = 0.3f;

        /// <summary>
        /// Shininess factor.
        /// </summary>
        public uint Alpha = 10;
    }
}
