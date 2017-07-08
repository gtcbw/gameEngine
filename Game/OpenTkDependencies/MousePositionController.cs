using Engine.Contracts.Input;
using OpenTK.Input;
using System.Drawing;

namespace Game.OpenTkDependencies
{
    public sealed class MousePositionController : IMousePositionController
    {
        private MouseDevice _mouseDevice;
        private const int _fixPixelCountForMouseCenter = 200;
        private IMousePositionDeltaObserver _mousePositionDeltaObserver;

        public MousePositionController(MouseDevice mouseDevice, IMousePositionDeltaObserver mousePositionDeltaObserver)
        {
            _mouseDevice = mouseDevice;
            _mousePositionDeltaObserver = mousePositionDeltaObserver;
        }

        void IMousePositionController.MeasureMousePositionDelta()
        {
            MousePositionDelta mousePositionDelta = new MousePositionDelta();

            Point cursor = System.Windows.Forms.Cursor.Position;

            mousePositionDelta.PositionDeltaX = cursor.X - _fixPixelCountForMouseCenter;
            mousePositionDelta.PositionDeltaY = _fixPixelCountForMouseCenter - cursor.Y;

            System.Windows.Forms.Cursor.Position = new Point(_fixPixelCountForMouseCenter, _fixPixelCountForMouseCenter);

            _mousePositionDeltaObserver.MousePositionDeltaUpdated(mousePositionDelta);
        }
    }
}
