using Engine.Contracts;
using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class VehicleProvider : IVehicleProvider, IComplexShapeProvider
    {
        private readonly IEnumerable<Vehicle> _vehicles;
        private IEnumerable<Vehicle> _activeVehicles;
        private IEnumerable<ComplexShapeInstance> _collisionModels;

        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly double _fieldLength;
        private IReadOnlyPosition _lastPosition;

        public VehicleProvider(IEnumerable<Vehicle> vehicles,
            IPlayerPositionProvider playerPositionProvider, 
            double fieldLength)
        {
            _vehicles = vehicles;
            _playerPositionProvider = playerPositionProvider;
            _fieldLength = fieldLength;
            _lastPosition = new Position { X = -10000, Z = -10000 };
        }

        public void UpdateVehicles()
        {
            IReadOnlyPosition position = _playerPositionProvider.GetPlayerPosition();

            if (!PositionIsNearerThan(_lastPosition, position, 50))
                return;

            _lastPosition = position;

            UpdateVehicleLists();
        }

        private void UpdateVehicleLists()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            foreach(Vehicle vehicle in _vehicles)
            {
                if (PositionIsNearerThan(vehicle.Position, _lastPosition, _fieldLength + 50))
                {
                    vehicles.Add(vehicle);
                }
            }
            _activeVehicles = vehicles;
        }

        private bool PositionIsNearerThan(IReadOnlyPosition positionOne, IReadOnlyPosition positionTwo, double distance)
        {
            if (positionOne.X < positionTwo.X)
            {
                if (positionTwo.X - positionOne.X > distance)
                    return true;
            }
            else
            {
                if (positionOne.X - positionTwo.X > distance)
                    return true;
            }

            if (positionOne.Z < positionTwo.Z)
            {
                if (positionTwo.Z - positionOne.Z > distance)
                    return true;
            }
            else
            {
                if (positionOne.Z - positionTwo.Z > distance)
                    return true;
            }
            return false;
        }

        IEnumerable<Vehicle> IVehicleProvider.GetVehicles()
        {
            return _activeVehicles;
        }

        IEnumerable<ComplexShapeInstance> IComplexShapeProvider.GetComplexShapes()
        {
            return _collisionModels;
        }
    }
}
