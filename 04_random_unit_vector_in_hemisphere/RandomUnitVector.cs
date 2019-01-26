using System;
using System.Diagnostics;
using System.Numerics;
using Xunit;

namespace _04_random_unit_vector_in_hemisphere
{
    public class RandomUnitVector
    {
        public static Vector3 RandomUnitVectorOnUnitSphere()
        {
            //ref: http://mathworld.wolfram.com/SpherePointPicking.html
            var x0 = Random.NextDouble()*2 - 1;
            var x1 = Random.NextDouble()*2 - 1;
            var x2 = Random.NextDouble()*2 - 1;
            var x3 = Random.NextDouble()*2 - 1;
            var divider = x0 * x0 + x1 * x1 + x2 * x2 + x3 * x3;
            var pX = (float)(2 * (x1 * x3 + x0 * x2) / divider);
            var pY = (float)(2 * (x2 * x3 - x0 * x1) / divider);
            var pZ = (float)((x0 * x0 + x3 * x3 - x1 * x1 - x2 * x2) / divider);
            return new Vector3(pX, pY, pZ);
        }

        public static Vector3 RandomUnitVectorOnNorthernHemisphere()
        {
            var p = RandomUnitVectorOnUnitSphere();
            Debug.Assert(Math.Abs(p.LengthSquared()-1) <= 0.001);
            //move the point onto northern hemisphere surface
            p.Y = Math.Abs(p.Y);

            return p;
        }

        public static float AngleBetween(Vector3 a, Vector3 b)
        {
            return (float)Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
        }

        public static Vector3 RotateUnitVector(Vector3 p, Vector3 a, Vector3 b)
        {
            a = Vector3.Normalize(a);
            b = Vector3.Normalize(b);
            var axis = Vector3.Normalize(Vector3.Cross(a, b));
            var angle = AngleBetween(a, b);
            var quaternion = Quaternion.CreateFromAxisAngle(axis, angle);
            return Vector3.Transform(p, quaternion);
        }

        public static Random Random = new Random();
        public static Vector3 RandomUnitVectorInHemisphereOf(Vector3 dir)
        {
            var p = RandomUnitVectorOnNorthernHemisphere();
            //now p is distributed around the north unit vector: Vector3.UnitY
            p = RotateUnitVector(p, Vector3.UnitY, Vector3.Normalize(dir));//rotate the vector to make it surround dir
            //now p is distributed around dir
            return p;
        }

        [Fact]
        public void GenerateRandomUnitVectorOnNorthernHemisphere()
        {
            for (var i = 0; i < 2000; i++)
            {
                var p = RandomUnitVectorOnNorthernHemisphere();
                Assert.True(Vector3.Dot(p, Vector3.UnitY) >= 0);
            }
        }

        [Fact]
        public void TryRotateVector()
        {
            var p = Vector3.Normalize(new Vector3(1.34f,1,1.3f));
            var a = Vector3.Normalize(new Vector3(1.34f,1,1.3f));
            var b = Vector3.Normalize(new Vector3(0,0.5f,1));
            p = RotateUnitVector(p, a, b);
            Assert.Equal(p.X, b.X, 3);
            Assert.Equal(p.Y, b.Y, 3);
            Assert.Equal(p.Z, b.Z, 3);
        }

        [Fact]
        public void GenerateRandomUnitVectorInHemisphereOf()
        {
            var dir = new Vector3(1, 1, 0);
            for (int i = 0; i < 200; i++)
            {
                var v = RandomUnitVectorInHemisphereOf(dir);
                var dotProduct = Vector3.Dot(v, dir);
                Console.WriteLine($"{dotProduct:F3}");
                Assert.True(dotProduct >= 0 || Math.Abs(dotProduct) < 0.001);
            }
        }

    }
}
