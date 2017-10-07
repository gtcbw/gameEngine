using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework
{
    public sealed class CollisionModelCache : IComplexShapeProvider
    {
        private readonly IComplexShapeProvider _complexShapeProvider;
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IPositionDistanceComparer _positionDistanceComparer;
        private readonly double _maxDistance;
        private readonly double _testRadius;
        private IReadOnlyPosition _lastUpdatedPosition;
        private List<ComplexShapeInstance> _cachedItems;

        public CollisionModelCache(IComplexShapeProvider complexShapeProvider,
            IPlayerPositionProvider playerPositionProvider,
            IPositionDistanceComparer positionDistanceComparer,
            double maxDistance,
            double testRadius)
        {
            _complexShapeProvider = complexShapeProvider;
            _playerPositionProvider = playerPositionProvider;
            _positionDistanceComparer = positionDistanceComparer;
            _maxDistance = maxDistance;
            _testRadius = testRadius;
            _lastUpdatedPosition = new Position { X = 50000, Z = 50000 };
        }

        IEnumerable<ComplexShapeInstance> IComplexShapeProvider.GetComplexShapes()
        {
            if (_positionDistanceComparer.PositionIsLargerThan(_lastUpdatedPosition, _playerPositionProvider.GetPlayerPosition(), _maxDistance))
            {
                _lastUpdatedPosition = _playerPositionProvider.GetPlayerPosition();
                UpdateCachedList();
            }

            return _cachedItems;
        }

        private void UpdateCachedList()
        {
            IEnumerable<ComplexShapeInstance> instances = _complexShapeProvider.GetComplexShapes();
            _cachedItems = new List<ComplexShapeInstance>();

            foreach (ComplexShapeInstance complexShapeInstance in instances)
            {
                if (_positionDistanceComparer.PositionIsNearerThan(_lastUpdatedPosition, complexShapeInstance.Position, _testRadius + complexShapeInstance.ComplexShape.RadiusXZ))
                    _cachedItems.Add(complexShapeInstance);
            }
        }
    }
}
