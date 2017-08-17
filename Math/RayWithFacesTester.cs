using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class RayWithFacesTester
    {
        private IIntersectionCalculator _intersectionCalculator;
        private IObtuseAngleTester _obtuseAngleTester;

        public RayWithFacesTester(IIntersectionCalculator intersectionCalculator, 
            IObtuseAngleTester obtuseAngleTester)
        {
            _intersectionCalculator = intersectionCalculator;
            _obtuseAngleTester = obtuseAngleTester;
        }

        public Position SearchCollision(Ray ray, Face[] faces)
        {
            Position collisionPosition = null;

            double[] rayStartPosition = new double[] 
            {
                 ray.StartPosition.X,
                 ray.StartPosition.Y,
                 ray.StartPosition.Z
            };
            double[] rayDirection = new double[]
           {
               ray.Direction.X,
               ray.Direction.Y,
               ray.Direction.Z
           };

            foreach (Face face in faces)
            {
                double[] vector = new double[] 
                {
                    face.Triangles[0].Corner1[0] - ray.StartPosition.X,
                    face.Triangles[1].Corner1[1] - ray.StartPosition.Y,
                    face.Triangles[2].Corner1[2] - ray.StartPosition.Z,
                };
                if (!_obtuseAngleTester.AngleIsObtuse(face.Normal, vector))
                {
                    for(int i = 0; i<face.Triangles.Length; i++)
                    {
                        Position position = _intersectionCalculator.RayHitsTriangle(rayStartPosition, rayDirection, face.Triangles[i].Corner1, face.Triangles[i].Corner2, face.Triangles[i].Corner3);

                        if (position != null)
                        {
                            if (collisionPosition == null)
                                collisionPosition = position;
                               
                            else
                            {
                                double squareDistance = ((collisionPosition.X - rayStartPosition[0]) * (collisionPosition.X - rayStartPosition[0])) +
                                    ((collisionPosition.Y - rayStartPosition[1]) * (collisionPosition.Y - rayStartPosition[1])) +
                                    ((collisionPosition.Z - rayStartPosition[2]) * (collisionPosition.Z - rayStartPosition[2]));

                                double squareDistanceNew = ((position.X - rayStartPosition[0]) * (position.X - rayStartPosition[0])) +
                                    ((position.Y - rayStartPosition[1]) * (position.Y - rayStartPosition[1])) +
                                    ((position.Z - rayStartPosition[2]) * (position.Z - rayStartPosition[2]));

                                if (squareDistanceNew < squareDistance)
                                    collisionPosition = position;
                            }
                        }
                    }
                }
            }

            return collisionPosition;
        }
    }
}
