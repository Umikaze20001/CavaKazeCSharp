using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Normal
    {
        public float x, y, z;
        public Normal()
        {
            x = 0.0f;
            y = 1.0f;
            z = 0.0f;
        }
        public Normal(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }
}
