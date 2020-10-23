using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crabination
{
    public class Vector3
    {
        public float x, y, z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(float x1, float y1, float z1)
        {
            x = x1;
            y = y1;
            z = z1;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v, float a)
        {
            return new Vector3(v.x * a, v.y * a, v.z * a);
        }
        public static Vector3 operator *(float a, Vector3 v)
        {
            return new Vector3(v.x * a, v.y * a, v.z * a);
        }

        public float Dot(Vector3 v)
        {
            return (x * v.x) + (y * v.y) + (z * v.z);
        }

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(y * v.z - z * v.y,z * v.x - x * v.z,(x * v.y - y * v.x));
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public void Normalize()
        {
            Vector3 v = new Vector3(x / Magnitude(), y / Magnitude(), z / Magnitude());
            x = v.x;
            y = v.y;
            z = v.z;
        }
    }
}
