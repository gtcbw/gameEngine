using Engine.Contracts.PlayerMotion;
using Engine.Contracts;
using World.Model;
using Math.Contracts;

namespace Engine.Framework.PlayerMotion
{
    public sealed class PlayerMotionEncapsulator : IPlayerMotionEncapsulator
    {
        private Position _position;
        private Ray _ray;
        TwoComponentRotation _direction;
        private readonly IVectorHelper _vectorHelper;

        public PlayerMotionEncapsulator(IVectorHelper vectorHelper,
            Position startPosition)
        {
            _vectorHelper = vectorHelper;

            _position = startPosition;
            _direction = new TwoComponentRotation { DegreeXZ = 0, DegreeY = 0 };
            _ray = new Ray { Direction = new Vector { X = 0, Y = 0, Z = 1 }, StartPosition = _position };
        }

        public IReadOnlyPosition GetPlayerPosition()
        {
            return _position;
        }

        public Ray GetPlayerViewRay()
        {
            return _ray;
        }

        public TwoComponentRotation GetViewDirection()
        {
            return _direction;
        }

        public void SetMotion(Position position, double height, double degreeXZ, double degreeY, Vector2D vectorXZ = null)
        {
            _position = position;

            if (degreeXZ > 360.0)
                degreeXZ -= 360.0;
            else if (degreeXZ < 0.0)
                degreeXZ  += 360.0;

            if (vectorXZ == null)
                vectorXZ = _vectorHelper.ConvertDegreeToVector(degreeXZ);

            _direction = new TwoComponentRotation { DegreeXZ = degreeXZ, DegreeY = degreeY };
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(degreeY);
            _ray.Direction = new Vector
            {
                X = vectorXZ.X * vectorY.X,
                Z = vectorXZ.Z * vectorY.X,
                Y = vectorY.Z
            };
            _ray.StartPosition = new Position { X = _position.X, Y = _position.Y + height, Z = _position.Z };
        }
    }
}
