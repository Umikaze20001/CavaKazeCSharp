using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Render;
namespace Render
{
    public class Point
    {
        public float x, y, z;
        public Point(float a, float b, float c){
            x = a;
            y = b;
            c = z;
        }

        public Point()
        {
            x = y = z = 0;
        }
        public Point(Vec3 c)
        {
            x = c.x;
            y = c.y;
            z = c.z;
        }
        public float this[int i]
        {
            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else
                    return z;
            }
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else
                    z = value;
            }
        }
        public static Point operator+(Point a, Vec3 b)
        {
            Point p = new Point();
            p[0] = a[0] + b[0];
            p[1] = a[1] + b[1];
            p[2] = a[2] + b[2];
            return p;
        }

        public static Vec3 operator-(Point a,Point b)
        {
            Vec3 v = new Vec3();
            v[0] = a[0] - b[0];
            v[1] = a[1] - b[1];
            v[2] = a[2] - b[2];
            return v;
        }

        public static Point operator-(Point a,Vec3 b)
        {
            Point v = new Point();
            v[0] = a[0] - b[0];
            v[1] = a[1] - b[1];
            v[2] = a[2] - b[2];
            return v;
        }
       // public static 

        public static Point operator*(Point a, float index)
        {
            Point p = new Point();
            p[0] = a[0] * index;
            p[1] = a[1] * index;
            p[2] = a[2] * index;
            return p;
        }

        public static Point operator/(Point a, float index)
        {
            if(index == 0)
            {
                //报错
            }
            float inv = 1 / index;
            Point p = new Point();
            p[0] = a[0] / index;
            p[1] = a[1] / index;
            p[2] = a[2] / index;
            return p;
        }

     
    }
}
