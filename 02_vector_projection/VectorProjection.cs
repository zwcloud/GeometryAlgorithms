using System.Numerics;
using Xunit;

namespace _02_vector_projection
{
    public class VectorProjection
    {
        public static Vector3 Project(Vector3 from, Vector3 onto)
        {
            var u = from;
            var v = onto;
            return Vector3.Dot(v, u) / v.LengthSquared() * v;
        }

        [Fact]
        public void RunProject1()
        {
            Vector3 u = new Vector3(1, 1, 1);
            Vector3 v = new Vector3(2, 1, 0);

            Vector3 projection = Project(u, v);

            Assert.Equal(new Vector3(1.2f, 0.6f, 0), projection);
        }

        [Fact]
        public void RunProject2()
        {
            Vector3 u = new Vector3(1, 0, 0);
            Vector3 v = new Vector3(1, 1, 1);

            Vector3 projection = Project(u, v);

            Assert.Equal(new Vector3(1/3.0f, 1/3.0f, 1/3.0f), projection);
        }
    }
}
