using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public class Plane : Primitive
    {
        public Normal normal;
        public Plane()
        {
            normal = new Normal();
        }
        public Plane(Point _centre, Color _materialColor, Normal _normal)
        {
            centre = _centre;
            materialColor = _materialColor;
            normal = _normal;
        }
         
        public override void SendToCuda()
        {
            //CudaUtil.SendPlane(
            //    centre.x, centre.y, centre.z,
            //    materialColor.r, materialColor.g, materialColor.b, materialColor.a,
            //    normal.x, normal.y, normal.z
            //    );
            //float trmp = CudaUtil.GetPlaneNormalX(2);
        }
    }
}
