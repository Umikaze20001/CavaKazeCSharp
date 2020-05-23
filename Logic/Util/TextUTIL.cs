using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render 
{
    public static class TextUtil
    {
        public static float GetFloat(string s)
        {
            if (s.Length == 0 || s == "-")
            {
                return 0.0f;
            }
            else
            {
                return float.Parse(s);
            }
        }
    }
}
