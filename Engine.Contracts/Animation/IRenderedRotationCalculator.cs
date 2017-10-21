namespace Engine.Contracts.Animation
{
    public interface IRenderedRotationCalculator
    {
        RotationDegrees CalculateRotationRelativeToCamera(RotationDegrees absoluteRotation, double positionX, double positionZ);
    }
}
