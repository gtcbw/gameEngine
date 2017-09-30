using Engine.Contracts;
using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class VehicleManager : IVehicleManager, IComplexShapeProvider, IRenderingElement
    {
        private readonly IEnumerable<Vehicle> _vehicles;
        private List<Vehicle> _activeVehicles;
        private List<ComplexShapeInstance> _collisionModels;

        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly ISpriteRenderer _spriteRenderer;
        private readonly IPositionDistanceComparer _positionDistanceComparer;
        private readonly double _fieldLength;
        private IReadOnlyPosition _lastPosition;

        private Vehicle _enteredVehicle;

        public VehicleManager(IEnumerable<Vehicle> vehicles,
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
            List<ComplexShapeInstance> collisionModels = new List<ComplexShapeInstance>();

            foreach (Vehicle vehicle in _vehicles)
            {
                if (vehicle == _enteredVehicle)
                    continue;

                if (_positionDistanceComparer.PositionIsNearerThan(vehicle.Position, _lastPosition, _fieldLength + 50))
                {
                    vehicles.Add(vehicle);
                    collisionModels.Add(vehicle.CollisionModel);
                }
            }
            _activeVehicles = vehicles;
            _collisionModels = collisionModels;
        }

        IVehicle IVehicleManager.FindNearestVehicle(IReadOnlyPosition position)
        {
            foreach (IVehicle vehicle in _activeVehicles)
            {
                if (_positionDistanceComparer.PositionIsNearerThan(vehicle.Position, position, 2.5))
                    return vehicle;
            }
            return null;
        }

        IEnumerable<ComplexShapeInstance> IComplexShapeProvider.GetComplexShapes()
        {
            return _collisionModels;
        }

        void IRenderingElement.Render()
        {
            foreach (Vehicle vehicle in _activeVehicles)
            {
                _spriteRenderer.RenderSpriteAtPosition(vehicle.Position);
            }
        }

        void IVehicleManager.EnterVehicle(IVehicle vehicle)
        {
            _enteredVehicle = vehicle as Vehicle;
            UpdateVehicleLists();
        }

        void IVehicleManager.LeaveVehicle(IVehicle vehicle)
        {
            _enteredVehicle = null;
            UpdateVehicleLists();
        }
    }
}
