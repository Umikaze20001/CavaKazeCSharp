using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public abstract class Primitive
    {
        public Point centre;
        public Color materialColor;
        public Primitive_type type = Primitive_type.Default;
        public Primitive()
        {
            centre = new Point();
            materialColor = new Color();
        }
         
        public abstract void SendToCuda();
    }
}
