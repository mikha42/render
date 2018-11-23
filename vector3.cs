using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace render
{
    class vector3
    {
        public double x;
        public double y;
        public double z;

        //Initializing
        public vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

        }

        //Calculated values
        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }
        public double length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }
        public double lengthSqr
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }
        public vector3 copy
        {
            get
            {
                return new vector3(x, y, z);
            }
        }


        //Actions
        public vector3 rotateAbout(double theta, vector3 axis)
        {
            vector3 parallel = (dot(this, axis) / dot(axis, axis)) * axis;
            vector3 perpendicular = this - parallel;
            if (perpendicular.length == 0)
                return this;
            vector3 w = cross(axis, perpendicular);
            double x1 = Math.Cos(theta) / perpendicular.length;
            double x2 = Math.Sin(theta) / w.length;
            vector3 perpendicularRotated = perpendicular.length * (x1 * perpendicular + x2 * w);
            vector3 rotated = perpendicularRotated + parallel;
            x = rotated.x;
            y = rotated.y;
            z = rotated.z;
            return this;
        }
        public vector3 rotateLike(vector3 a)
        {
            double t = Math.Atan(a.y / a.x);
            double p = Math.Acos(a.z / a.length);
            vector3 rotated = copy;
            rotated.rotateX(t);
            rotated.rotateZ(p);
            x = rotated.x;
            y = rotated.y;
            z = rotated.z;
            return this;
        }
        public vector3 rotateX(double theta)
        {
            vector3 Rx1 = new vector3(
                1, 0, 0
                );
            vector3 Rx2 = new vector3(
                0, Math.Cos(theta), -Math.Sin(theta)
                );
            vector3 Rx3 = new vector3(
                0, Math.Sin(theta), Math.Cos(theta)
                );

            vector3 rotated = new vector3(
                dot(this, Rx1),
                dot(this, Rx2),
                dot(this, Rx3)
                );

            x = rotated.x;
            y = rotated.y;
            z = rotated.z;
            return this;
        }
        public vector3 rotateY(double theta)
        {
            vector3 Ry1 = new vector3(
                Math.Cos(theta), 0, Math.Sin(theta)
                );
            vector3 Ry2 = new vector3(
                0, 1, 0
                );
            vector3 Ry3 = new vector3(
                -Math.Sin(theta), 0, Math.Cos(theta)
                );

            vector3 rotated = new vector3(
                dot(this, Ry1),
                dot(this, Ry2),
                dot(this, Ry3)
                );

            x = rotated.x;
            y = rotated.y;
            z = rotated.z;
            return this;
        }
        public vector3 rotateZ(double theta)
        {
            vector3 Rz1 = new vector3(
                Math.Cos(theta), -Math.Sin(theta), 0
                );
            vector3 Rz2 = new vector3(
                Math.Sin(theta), Math.Cos(theta), 0
                );
            vector3 Rz3 = new vector3(
                0, 0, 1
                );

            vector3 rotated = new vector3(
                dot(this, Rz1),
                dot(this, Rz2),
                dot(this, Rz3)
                );

            x = rotated.x;
            y = rotated.y;
            z = rotated.z;
            return this;
        }
        public vector3 normalize()
        {
            vector3 norm = this / length;
            x = norm.x;
            y = norm.y;
            z = norm.z;
            return this;
        }

        //Operators and overrides
        public static vector3 operator +(vector3 a, vector3 b)
        {
            return new vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static vector3 operator -(vector3 a, vector3 b)
        {
            return new vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static vector3 operator *(vector3 a, double b)
        {
            return new vector3(a.x * b, a.y * b, a.z * b);
        }
        public static vector3 operator *(double a, vector3 b)
        {
            return b * a;
        }
        public static vector3 operator /(vector3 a, double b)
        {
            return a * (1d / b);
        }
        public static double dot(vector3 a, vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        public static vector3 cross(vector3 a, vector3 b)
        {
            return new vector3( a.y * b.z - a.z * b.y,
                                a.z * b.x - a.x * b.z,
                                a.x * b.y - a.y * b.x);
        }

    }
}
