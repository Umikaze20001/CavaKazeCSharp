using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public class Triangle : Primitive
    {
        public Point[] points;
        public Normal normal;
        public Triangle()
        {
            points = new Point[3];
            for (int i = 0; i < 3; ++i)
            {
                points[i] = new Point();
            }
            normal = new Normal();
        }
        public Triangle(Point[] _points, Color _materialColor)
        {
            centre.x = (_points[0].x + _points[1].x + _points[2].x);
            centre.y = (_points[0].y + _points[1].y + _points[2].y);
            centre.z = (_points[0].z + _points[1].z + _points[2].z);
            materialColor = _materialColor;
            points[0] = _points[0];
            points[1] = _points[1];
            points[2] = _points[2];
        }
         
        public override void SendToCuda()
        {
            //CudaUtil.SendTriangle(
            //    center.x, center.y, center.z,
            //    materialColor.r, materialColor.g, materialColor.b, materialColor.a,
            //    points[0].x, points[0].y, points[0].z,
            //    points[1].x, points[1].y, points[1].z,
            //    points[2].x, points[2].y, points[2].z
            //    );
            //float temp = CudaUtil.GetTrianglePoint0X(1);
        }
    }
}
