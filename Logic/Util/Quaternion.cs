using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Quaternion
    {
        public Vec3 imaginaryPart = new Vec3();
        public float realPart;
        public Quaternion()
        {
            imaginaryPart.x = 0.0f;
            imaginaryPart.y = 0.0f;
            imaginaryPart.z = 1.0f;
            realPart = 0.0f;
        }

        public Quaternion(Vec3 axis, float theta)
        {
            axis.normalize();
            imaginaryPart = (float)Math.Sin(0.5f * theta) * axis;
            realPart = (float)Math.Cos(0.5f * theta);
        }
        public Quaternion(float x, float y, float z, float w)
        {
            imaginaryPart = new Vec3(x, y, z);
            realPart = w;
        }

        public Quaternion(Vec3 v)
        {
            imaginaryPart.x = v.x;
            imaginaryPart.y = v.y;
            imaginaryPart.z = v.z;
            realPart = 0.0f;
        }
        public Quaternion(Point p)
        {
            imaginaryPart.x = p.x;
            imaginaryPart.y = p.y;
            imaginaryPart.z = p.z;
            realPart = 0.0f;
        }

        public Quaternion inv()
        {
            return new Quaternion(-imaginaryPart.x, -imaginaryPart.y, -imaginaryPart.z, realPart);
        }
        public static Quaternion operator *(Quaternion q, Quaternion p)
        {
            Quaternion temp = new Quaternion();
            temp.imaginaryPart = q.realPart * p.imaginaryPart + p.realPart * q.imaginaryPart + Vec3.cross(q.imaginaryPart, p.imaginaryPart);
            temp.realPart = q.realPart * p.realPart - Vec3.dot(q.imaginaryPart, p.imaginaryPart);
            return temp;
        }
        public static Quaternion operator *(Quaternion q, Point _p)
        {
            Quaternion p = new Quaternion(_p);
            Quaternion result = q * p;
            return result;
        }
        public void rotatePoint(ref Point p)
        {
            Quaternion temp = this * p * this.inv();
            p = new Point(temp.imaginaryPart);
        }

    }
}
