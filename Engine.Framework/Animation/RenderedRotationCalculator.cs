using Engine.Contracts.Animation;
using Engine.Contracts.PlayerMotion;

namespace Engine.Framework.Animation
{
    public sealed class RenderedRotationCalculator : IRenderedRotationCalculator
    {
        private readonly IPlayerPositionProvider _playerPositionProvider;

        public RenderedRotationCalculator(IPlayerPositionProvider playerPositionProvider)
        {
            _playerPositionProvider = playerPositionProvider;
        }

        RotationDegrees IRenderedRotationCalculator.CalculateRotationRelativeToCamera(RotationDegrees absoluteRotation, double positionX, double positionZ)
        {
            var position = _playerPositionProvider.GetPlayerPosition();

            return MapDegreeByCameraPerspective(MapVectorToDegree(positionX - position.X, positionZ - position.Z), absoluteRotation);
        }

        private static RotationDegrees MapDegreeByCameraPerspective(RotationDegrees camera, RotationDegrees element)
        {
            int test = (int)element + 5 - (int)camera;

            if (test < 1)
                return (RotationDegrees)test + 8;

            if (test > 8)
                return (RotationDegrees)test - 8;

            return (RotationDegrees)test;
        }

        private static RotationDegrees MapVectorToDegree(double x, double y)
        {
            double ratio = y != 0 ? x / y : x > 0 ? 10000 : -10000;

            double degreeRation22dot5 = 2.414;
            double degreeRatio67dot5 = 0.414;

            if (y >= 0)
            {
                if (x >= 0)
                {
                    if (ratio >= degreeRation22dot5)
                        return RotationDegrees.degree_0;
                    if (ratio >= degreeRatio67dot5)
                        return RotationDegrees.degree_45;
                    return RotationDegrees.degree_90;
                }

                if (ratio <= -degreeRation22dot5)
                    return RotationDegrees.degree_180;
                if (ratio <= -degreeRatio67dot5)
                    return RotationDegrees.degree_135;
                return RotationDegrees.degree_90;
            }
            else
            {
                if (x > 0)
                {
                    if (ratio <= -degreeRation22dot5)
                        return RotationDegrees.degree_0;
                    if (ratio <= -degreeRatio67dot5)
                        return RotationDegrees.degree_315;
                    return RotationDegrees.degree_270;
                }

                if (ratio >= degreeRation22dot5)
                    return RotationDegrees.degree_180;
                if (ratio >= degreeRatio67dot5)
                    return RotationDegrees.degree_225;
                return RotationDegrees.degree_270;
            }
        }
    }
}
