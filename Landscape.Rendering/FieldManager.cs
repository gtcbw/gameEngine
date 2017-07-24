using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FieldManager
    {
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IDelayedFloorLoader _delayedFloorLoader;
        private readonly IFieldChangeAnalyzer _fieldChangeAnalyzer;
        private readonly IActiveFieldCalculator _activeFieldCalculator;
        private IEnumerable<FieldCoordinates> _activeFields = new List<FieldCoordinates>();

        public FieldManager(IPlayerPositionProvider playerPositionProvider,
            IDelayedFloorLoader delayedFloorLoader,
            IFieldChangeAnalyzer fieldChangeAnalyzer,
            IActiveFieldCalculator activeFieldCalculator)
        {
            _playerPositionProvider = playerPositionProvider;
            _delayedFloorLoader = delayedFloorLoader;
            _fieldChangeAnalyzer = fieldChangeAnalyzer;
            _activeFieldCalculator = activeFieldCalculator;
        }

        public void UpdateFieldsByPlayerPosition()
        {
            Position playerPosition = _playerPositionProvider.GetPlayerPosition();

            var allActiveFields = _activeFieldCalculator.CalculateActiveFields(playerPosition);
            var addedFields = _fieldChangeAnalyzer.FindAddedFields(_activeFields, allActiveFields);
            var removedFields = _fieldChangeAnalyzer.FindRemovedFields(_activeFields, allActiveFields);

            _delayedFloorLoader.UpdateMesh(addedFields, removedFields);

            _activeFields = allActiveFields;
        }
    }
}
