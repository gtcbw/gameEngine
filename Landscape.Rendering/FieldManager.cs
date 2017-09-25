using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FieldManager
    {
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IEnumerable<IFieldChangeObserver> _observers;
        private readonly IFieldChangeAnalyzer _fieldChangeAnalyzer;
        private readonly IActiveFieldCalculator _activeFieldCalculator;
        private IEnumerable<FieldCoordinates> _activeFields = new List<FieldCoordinates>();

        public FieldManager(IPlayerPositionProvider playerPositionProvider,
            IEnumerable<IFieldChangeObserver> delayedFloorLoaders,
            IFieldChangeAnalyzer fieldChangeAnalyzer,
            IActiveFieldCalculator activeFieldCalculator)
        {
            _playerPositionProvider = playerPositionProvider;
            _observers = delayedFloorLoaders;
            _fieldChangeAnalyzer = fieldChangeAnalyzer;
            _activeFieldCalculator = activeFieldCalculator;
        }

        public void UpdateFieldsByPlayerPosition()
        {
            IReadOnlyPosition playerPosition = _playerPositionProvider.GetPlayerPosition();

            var allActiveFields = _activeFieldCalculator.CalculateActiveFields(playerPosition);
            var addedFields = _fieldChangeAnalyzer.FindAddedFields(_activeFields, allActiveFields);
            var removedFields = _fieldChangeAnalyzer.FindRemovedFields(_activeFields, allActiveFields);

            foreach(IFieldChangeObserver observer in _observers)
            {
                observer.NotifyChangedFields(addedFields, removedFields);
            }
            
            _activeFields = allActiveFields;
        }
    }
}
