using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class RayWithFacesTester : IRayWithFacesTester
    {
        private IIntersectionCalculator _intersectionCalculator;
        private IObtuseAngleTester _obtuseAngleTester;
        private readonly IPositionDistanceTester _positionDistanceTester;

        public RayWithFacesTester(IIntersectionCalculator intersectionCalculator, 
            IObtuseAngleTester obtuseAngleTester,
            IPositionDistanceTester positionDistanceTester)
        {
            _intersectionCalculator = intersectionCalculator;
            _obtuseAngleTester = obtuseAngleTester;
            _positionDistanceTester = positionDistanceTester;
        }

        Position IRayWithFacesTester.SearchCollision(double[] rayStartPosition, double[] rayDirection, Face[] faces)
        {
            Position collisionPosition = null;

            foreach (Face face in faces)
            {
                double[] vector = new double[]
                {
                    rayStartPosition[0] - face.Triangles[0].Corner1[0],
                    rayStartPosition[1] - face.Triangles[0].Corner1[1],
                    rayStartPosition[2] - face.Triangles[0].Corner1[2]
                };
                if (!_obtuseAngleTester.AngleIsOver90Degree(face.Normal, vector))
                {
                    for(int i = 0; i<face.Triangles.Length; i++)
                    {
                        Position position = _intersectionCalculator.RayHitsTriangle(rayStartPosition, rayDirection, 
                            face.Triangles[i].Corner2,
                            face.Triangles[i].Corner1,
                            face.Triangles[i].Corner3);

                        if (position != null)
                        {
                            if (collisionPosition == null)
                                collisionPosition = position;
                            else if (_positionDistanceTester.FirstPositionIsNearerToPoint(position, collisionPosition, rayStartPosition))
                                collisionPosition = position;
                        }
                    }
                }
            }

            return collisionPosition;
        }
    }
}
