using Engine.Contracts;
using Engine.Contracts.Input;

namespace Engine.Framework
{
    public class PlayerViewDirectionProvider : IPlayerViewDirectionProvider, IMousePositionDeltaObserver
    {
        private bool _invertMouse;
        private double _mouseSensitivity;
        private double _viewDegreeXZ;
        private double _viewDegreeY;
        private double _maxDegreeY;

        public PlayerViewDirectionProvider(bool invertMouse, double mouseSensitivity, double maxDegreeY)
        {
            _invertMouse = invertMouse;
            _mouseSensitivity = mouseSensitivity;
            _maxDegreeY = maxDegreeY;
        }

        ViewDirection IPlayerViewDirectionProvider.GetViewDirection()
        {
            return new ViewDirection { DegreeXZ = _viewDegreeXZ, DegreeY = _viewDegreeY };
        }

        void IMousePositionDeltaObserver.MousePositionDeltaUpdated(MousePositionDelta mousePositionDelta)
        {
            _viewDegreeXZ += mousePositionDelta.PositionDeltaX * _mouseSensitivity;

            if (_invertMouse)
                _viewDegreeY -= mousePositionDelta.PositionDeltaY * _mouseSensitivity;
            else
                _viewDegreeY += mousePositionDelta.PositionDeltaY * _mouseSensitivity;

            if (_viewDegreeXZ > 360.0)
                _viewDegreeXZ -= 360.0;
            else if (_viewDegreeXZ < 0.0)
                _viewDegreeXZ += 360.0;

            if (_viewDegreeY > _maxDegreeY)
                _viewDegreeY = _maxDegreeY;
            else if (_viewDegreeY < -_maxDegreeY)
                _viewDegreeY = -_maxDegreeY;
        }
    }
}
