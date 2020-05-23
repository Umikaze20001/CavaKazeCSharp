using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Vec4
    {
        public Vec4()
        {
            x = y = z = w = 0;
        }
        public Vec4(float a, float b, float c, float d)
        {
            x = a;
            y = b;
            c = z;
            d = w;
        }
        public Vec4(Vec4 v1)
        {
            x = v1.x;
            y = v1.y;
            z = v1.z;
            w = v1.w;
        }
        public float this[int i]
        {
            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else if (i == 2)
                    return z;
                else if (i == 3)
                    return w;
                else
                    return 0; // 这里是报错

            }
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else if (i == 2)
                    z = value;
                else if (i == 3)
                    w = value;
                else
                    return; // 这里其实应该是一句报错的语句
            }
        }
        public static Vec4 operator +(Vec4 v1, Vec4 v2)
        {
            return new Vec4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);

        }
        public static Vec4 operator -(Vec4 v1, Vec4 v2)
        {
            return new Vec4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }
        public static Vec4 operator *(Vec4 v1, Vec4 v2)
        {
            return new Vec4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }
        public static Vec4 operator /(Vec4 v1, Vec4 v2)
        {
            if (v2.x == 0 || v2.y == 0 || v2.z == 0 || v2.w == 0)
            {
                //报错
            }
            return new Vec4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }
        public static float dot(Vec4 v1, Vec4 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;
        }

        //public static Vec4 operator*(Transform t,Vec4 v)
        //{
        //    Vec4 result = new Vec4();

        //}

        public float x, y, z, w;
    }
}
