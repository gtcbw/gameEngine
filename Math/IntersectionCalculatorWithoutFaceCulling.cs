using Math.Contracts;
using World.Model;
using System.Runtime.CompilerServices;

namespace Math
{
    public sealed class IntersectionCalculatorWithoutFaceCulling : IIntersectionCalculator
    {
        Position IIntersectionCalculator.RayHitsTriangle(double[] orig, double[] dir, double[] vert0, double[] vert1, double[] vert2)
        {
            double[] result = new double[3];

            if (IntersectTriangleWithoutFaceCulling(orig, dir, vert0, vert1, vert2, result) == 1)
                return new Position
                {
                    X = vert0[0] + result[1] * (vert1[0] - vert0[0]) + result[2] * (vert2[0] - vert0[0]),
                    Y = vert0[1] + result[1] * (vert1[1] - vert0[1]) + result[2] * (vert2[1] - vert0[1]),
                    Z = vert0[2] + result[1] * (vert1[2] - vert0[2]) + result[2] * (vert2[2] - vert0[2])
                };

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CROSS(double[] dest, double[] v1, double[] v2)
        {
            dest[0] = v1[1] * v2[2] - v1[2] * v2[1];
            dest[1] = v1[2] * v2[0] - v1[0] * v2[2];
            dest[2] = v1[0] * v2[1] - v1[1] * v2[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double DOT(double []v1, double []v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SUB(double[] dest, double[] v1, double[] v2)
        {
            dest[0] = v1[0] - v2[0];
            dest[1] = v1[1] - v2[1];
            dest[2] = v1[2] - v2[2];
        }

        private const double EPSILON = 0.000001;

        private int IntersectTriangleWithoutFaceCulling(double[] orig, double[] dir,
           double[] vert0, double[] vert1, double[] vert2,
           double[] result)
        {
            double[] edge1 = new double[3];
            double[] edge2 = new double[3];
            double[] tvec = new double[3];
            double[] pvec = new double[3];
            double[] qvec = new double[3];

            double det, inv_det;

            /* find vectors for two edges sharing vert0 */
            SUB(edge1, vert1, vert0);
            SUB(edge2, vert2, vert0);

            /* begin calculating determinant - also used to calculate U parameter */
            CROSS(pvec, dir, edge2);

            /* if determinant is near zero, ray lies in plane of triangle */
            det = DOT(edge1, pvec);

            //#else                    /* the non-culling branch */
            if (det > -EPSILON && det < EPSILON)
                return 0;

            inv_det = 1.0 / det;

            /* calculate distance from vert0 to ray origin */
            SUB(tvec, orig, vert0);

            /* calculate U parameter and test bounds */
            result[1] = DOT(tvec, pvec) * inv_det;
            if (result[1] < 0.0 || result[1] > 1.0)
                return 0;

            /* prepare to test V parameter */
            CROSS(qvec, tvec, edge1);

            /* calculate V parameter and test bounds */
            result[2] = DOT(dir, qvec) * inv_det;
            if (result[2] < 0.0 || result[1] + result[2] > 1.0)
                return 0;

            /* calculate t, ray intersects triangle */
            //result[0] = DOT(edge2, qvec) * inv_det;
            //#endif
            return 1;
        }
    }
}
