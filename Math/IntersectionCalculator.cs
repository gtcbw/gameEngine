using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class IntersectionCalculator : IIntersectionCalculator
    {
        Position IIntersectionCalculator.RayHitsTriangle(Ray ray, Position corner1, Position corner2, Position corner3)
        {
            double[] orig = new double[3] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };
            double[] dir = new double[3] { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
            double[] vert0 = new double[3] { corner1.X, corner1.Y, corner1.Z };
            double[] vert1 = new double[3] { corner2.X, corner2.Y, corner2.Z };
            double[] vert2 = new double[3] { corner3.X, corner3.Y, corner3.Z };
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

        private void CROSS(double[] dest, double[] v1, double[] v2)
        {
            dest[0] = v1[1] * v2[2] - v1[2] * v2[1];
            dest[1] = v1[2] * v2[0] - v1[0] * v2[2];
            dest[2] = v1[0] * v2[1] - v1[1] * v2[0];
        }

        private double DOT(double []v1, double []v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }

        private void SUB(double[] dest, double[] v1, double[] v2)
        {
            dest[0] = v1[0] - v2[0];
            dest[1] = v1[1] - v2[1];
            dest[2] = v1[2] - v2[2];
        }

        private double EPSILON = 0.000001;

        private int IntersectTriangleWithFaceCulling(double[] orig, double[] dir,
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

            //# ifdef TEST_CULL           /* define TEST_CULL if culling is desired */
            if (det < EPSILON)
                return 0;

            /* calculate distance from vert0 to ray origin */
            SUB(tvec, orig, vert0);

            /* calculate U parameter and test bounds */
            result[1] = DOT(tvec, pvec);
            if (result[1] < 0.0 || result[1] > det)
                return 0;

            /* prepare to test V parameter */
            CROSS(qvec, tvec, edge1);

            /* calculate V parameter and test bounds */
            result[2] = DOT(dir, qvec);
            if (result[2] < 0.0 || result[1] + result[2] > det)
                return 0;

            /* calculate t, scale parameters, ray intersects triangle */
            //result[0] = DOT(edge2, qvec);
            inv_det = 1.0 / det;
            //result[0] *= inv_det;
            result[1] *= inv_det;
            result[2] *= inv_det;

            return 1;
        }

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
