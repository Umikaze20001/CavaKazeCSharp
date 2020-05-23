using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public class Sphere : Primitive
    {
        public float radius { get; set; }
        public Sphere()
        {
            radius = 1;
        }
        public Sphere(Point _center, float _radius)
        {
            centre = _center;
            radius = _radius;
        }
        public Sphere(Point _center, float _radius, Color _materialColor)
        {
            centre = _center;
            radius = _radius;
            materialColor = _materialColor;
        }
        public Sphere(float cx, float cy, float cz, float _radius)
        {
            centre = new Point(cx, cy, cz);
            radius = _radius;
        }

        

        public override void SendToCuda()
        {
            //CudaUtil.SendSphere(center, materialColor, radius);
        }
    }
}
