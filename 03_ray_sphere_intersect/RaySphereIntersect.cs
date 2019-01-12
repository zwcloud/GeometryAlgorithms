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
        /// <returns>I: first intersection point; n: sphere surface normal at the point</returns>
        public static (Vector3 I, Vector3 n) Intersect(Vector3 C, float r, Vector3 P, Vector3 d)
        {
            var PC = C - P;
            var PA = _02_vector_projection.VectorProjection.Project(PC, d);
            var A = P + PA;
            var CA = A - C;
            var dCA = CA.Length();
            var dIA = MathF.Sqrt(r * r - dCA * dCA);
            var dPA = PA.Length();
            var dPI = dPA - dIA;
            var I = P + d / d.Length() * dPI;
            var CI = I - C;
            var n = CI / r;//normalized
            return (I: I, n: n);
        }

        [Fact]
        public void RunIntersect1()
        {
            var sphereCenter = new Vector3(0,0,0);
            var sphereRadius = 1.0f;
            var rayOrigin = new Vector3(0, 0, 10);
            var rayDirection = new Vector3(0, 0, -1);

            var hit = Intersect(sphereCenter, sphereRadius, rayOrigin, rayDirection);

            Assert.Equal(0, hit.I.X, 3);
            Assert.Equal(0, hit.I.Y, 3);
            Assert.Equal(1, hit.I.Z, 3);
            Assert.Equal(0, hit.n.X, 3);
            Assert.Equal(0, hit.n.Y, 3);
            Assert.Equal(1, hit.n.Z, 3);
        }

        [Fact]
        public void RunIntersect2()
        {
            var sqrt_2_div_2 = MathF.Sqrt(2) / 2;

            var sphereCenter = new Vector3(0,0,0);
            var sphereRadius = 1.0f;
            var rayOrigin = new Vector3(0.5f, 0.5f, 100);
            var rayDirection = new Vector3(0, 0, -1);

            var hit = Intersect(sphereCenter, sphereRadius, rayOrigin, rayDirection);

            var expectedPoint = new Vector3(0.5f, 0.5f, sqrt_2_div_2);
            var expectedNormal = Vector3.Normalize(expectedPoint);
            Assert.Equal(expectedPoint.X, hit.I.X, 3);
            Assert.Equal(expectedPoint.Y, hit.I.Y, 3);
            Assert.Equal(expectedPoint.Z, hit.I.Z, 3);
            Assert.Equal(expectedNormal.X, hit.n.X, 3);
            Assert.Equal(expectedNormal.Y, hit.n.Y, 3);
            Assert.Equal(expectedNormal.Z, hit.n.Z, 3);
        }
    }
}
