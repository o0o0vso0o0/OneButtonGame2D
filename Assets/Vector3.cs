using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o0
{
    class Vector3
    {
        public double x;
        public double y;
        public double z;
        public Vector3() { }
        public Vector3(double[] v)
        {
            x = v[0];
            y = v[1];
            z = v[2];
        }
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3 assign(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            return this;
        }
        public double length()//length of vector
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public Vector3 normalize()
        {
            return this / length();
        }
        public Vector3 selfNormalize()
        {
            return assign(normalize());
        }
        public Vector3 vertical(Vector3 b)
        {
            Vector3 v = new Vector3(this[1] * b[2] - this[2] * b[1],
                this[2] * b[0] - this[0] * b[2],
                this[0] * b[1] - this[1] * b[0]);
            return v;
        }
        public double angle(Vector3 b)
        {
            return Math.Asin(Math.Sqrt(Math.Pow(this[0] - b[0], 2) + Math.Pow(this[1] - b[1], 2) + Math.Pow(this[2] - b[2], 2)) / 2) * 2;
        }
        public Vector3 rotate(Vector3 shaft, double angle)
        {
            double cosAngle = Math.Cos(angle);
            double sinAngle = Math.Sin(angle);
            double rotateDate = (shaft[0] * this[0] + shaft[1] * this[1] + shaft[2] * this[2]) * (1 - cosAngle);
            Vector3 outerProduct = vertical(shaft);
            Vector3 v = new Vector3(this[0] * cosAngle + outerProduct[0] * sinAngle + shaft[0] * rotateDate,
                this[1] * cosAngle + outerProduct[1] * sinAngle + shaft[1] * rotateDate,
                this[2] * cosAngle + outerProduct[2] * sinAngle + shaft[2] * rotateDate);
            return v;
        }
        public Vector3 selfRotate(Vector3 shaft, double angle)
        {
            return assign(rotate(shaft, angle));
        }

        public double this[int index]
        {
            get {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                    default:
                        return z;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                    default:
                        z = value;
                        break;
                }
            }
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }
    }
}

