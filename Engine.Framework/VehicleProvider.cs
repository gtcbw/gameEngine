using Engine.Contracts;
using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class VehicleProvider : IVehicleProvider, IComplexShapeProvider, IRenderingElement
    {
        private readonly IEnumerable<Vehicle> _vehicles;
        private IEnumerable<Vehicle> _activeVehicles;
        private IEnumerable<ComplexShapeInstance> _collisionModels;

        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly ISpriteRenderer _spriteRenderer;
        private readonly IPositionDistanceComparer _positionDistanceComparer;
        private readonly double _fieldLength;
        private IReadOnlyPosition _lastPosition;

        public VehicleProvider(IEnumerable<Vehicle> vehicles,
            IPlayerPositionProvider playerPositionProvider,
            ISpriteRenderer spriteRenderer,
            IPositionDistanceComparer positionDistanceComparer,
            double fieldLength)
        {
            _vehicles = vehicles;
            _playerPositionProvider = playerPositionProvider;
            _spriteRenderer = spriteRenderer;
            _positionDistanceComparer = positionDistanceComparer;
            _fieldLength = fieldLength;
            _lastPosition = new Position { X = -10000, Z = -10000 };
        }

        public void UpdateVehicles()
        {
            IReadOnlyPosition position = _playerPositionProvider.GetPlayerPosition();

            if (!_positionDistanceComparer.PositionIsLargerThan(_lastPosition, position, 50))
                return;

            _lastPosition = position;

            UpdateVehicleLists();
        }

        private void UpdateVehicleLists()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            foreach(Vehicle vehicle in _vehicles)
            {
                if (_positionDistanceComparer.PositionIsNearerThan(vehicle.Position, _lastPosition, _fieldLength + 50))
                {
                    vehicles.Add(vehicle);
                }
            }
            _activeVehicles = vehicles;
        }

        IEnumerable<IVehicle> IVehicleProvider.GetVehicles()
        {
            return _activeVehicles;
        }

        IEnumerable<ComplexShapeInstance> IComplexShapeProvider.GetComplexShapes()
        {
            return _collisionModels;
        }

        void IRenderingElement.Render()
        {
            foreach (Vehicle vehicle in _vehicles)
            {
                _spriteRenderer.RenderSpriteAtPosition(vehicle.Position);
            }
        }
    }
}
