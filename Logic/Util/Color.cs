using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Color
    {
        public Color()
        {
            r = g = b = 0.0f;
            a = 1.0f;
        }
        public Color(float _r, float _g, float _b, float _a)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }
        public float r = 0.0f, g = 0.0f, b = 0.0f, a = 0.0f;
        public static readonly Color Black = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Color Red = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Color Green = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        public static readonly Color Blue = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        public static readonly Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);


    }
}
