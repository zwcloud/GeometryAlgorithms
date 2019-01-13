using System;
using System.Numerics;
using Xunit;

namespace _03_ray_sphere_intersect
{
    public class RaySphereIntersect
    {
        /// <summary>
        /// Get the first intersection of a ray and a sphere.
        /// </summary>
        /// <param name="C">center of the sphere</param>
        /// <param name="r">radius of the sphere</param>
        /// <param name="P">origin of the ray</param>
        /// <param name="d">direction vector of the ray</param>
        /// <param name="I">first intersection point</param>
        /// <param name="n">sphere surface normal at the point</param>
        /// <returns>whether the ray and sphere are intersected</returns>
        public static bool Intersect(Vector3 C, float r, Vector3 P, Vector3 d, out Vector3 I, out Vector3 n)
        {
            var PC = C - P;
            if (Vector3.Dot(PC, d) < 0)
            {
                I = Vector3.Zero;
                n = Vector3.Zero;
                return false;
            }
            var PA = _02_vector_projection.VectorProjection.Project(PC, d);
            var A = P + PA;
            var CA = A - C;
            var dCA = CA.Length();
            if (dCA > r)
            {
                I = Vector3.Zero;
                n = Vector3.Zero;
                return false;
            }
            var dIA = MathF.Sqrt(r * r - dCA * dCA);
            var dPA = PA.Length();
            var dPI = dPA - dIA;
            I = P + d / d.Length() * dPI;
            var CI = I - C;
            n = CI / r;//normalized
            return true;
        }

        [Fact]
        public void RunIntersect1()
        {
            var sphereCenter = new Vector3(0,0,0);
            var sphereRadius = 1.0f;
            var rayOrigin = new Vector3(0, 0, 10);
            var rayDirection = new Vector3(0, 0, -1);

            var hit = Intersect(sphereCenter, sphereRadius, rayOrigin, rayDirection, out var I, out var n);

            Assert.True(hit);
            Assert.Equal(0, I.X, 3);
            Assert.Equal(0, I.Y, 3);
            Assert.Equal(1, I.Z, 3);
            Assert.Equal(0, n.X, 3);
            Assert.Equal(0, n.Y, 3);
            Assert.Equal(1, n.Z, 3);
        }

        [Fact]
        public void RunIntersect2()
        {
            var sqrt_2_div_2 = MathF.Sqrt(2) / 2;

            var sphereCenter = new Vector3(0,0,0);
            var sphereRadius = 1.0f;
            var rayOrigin = new Vector3(0.5f, 0.5f, 100);
            var rayDirection = new Vector3(0, 0, -1);

            var hit = Intersect(sphereCenter, sphereRadius, rayOrigin, rayDirection, out var I, out var n);

            Assert.True(hit);
            var expectedPoint = new Vector3(0.5f, 0.5f, sqrt_2_div_2);
            var expectedNormal = Vector3.Normalize(expectedPoint);
            Assert.Equal(expectedPoint.X, I.X, 3);
            Assert.Equal(expectedPoint.Y, I.Y, 3);
            Assert.Equal(expectedPoint.Z, I.Z, 3);
            Assert.Equal(expectedNormal.X, n.X, 3);
            Assert.Equal(expectedNormal.Y, n.Y, 3);
            Assert.Equal(expectedNormal.Z, n.Z, 3);
        }
    }
}
