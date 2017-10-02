using Math.Contracts;
using OpenTK;

namespace Math
{
    public sealed class PositionRotator : IPositionRotator
    {
        public double[] Rotate(double x, double y, double z, double rotationXZ)
        {
            double angleXZ = rotationXZ / 180.0 * System.Math.PI;

            Matrix4d matrixRotation = Matrix4d.CreateRotationY(angleXZ);
            //Matrix4d matrixTranslation = Matrix4d.CreateTranslation(0, 0, 0);
            //Matrix4d fullMatrix = Matrix4d.Mult(matrixTranslation, matrixRotation);

            Vector3d vector = Vector3d.Transform(new Vector3d(x, y, z), matrixRotation);


            return new[] { vector.X, vector.Y, vector.Z };
        }
    }
}
