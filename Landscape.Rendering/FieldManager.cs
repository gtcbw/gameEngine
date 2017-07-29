using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FieldManager
    {
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IEnumerable<IMeshUnitByFieldLoader> _delayedLoaders;
        private readonly IFieldChangeAnalyzer _fieldChangeAnalyzer;
        private readonly IActiveFieldCalculator _activeFieldCalculator;
        private IEnumerable<FieldCoordinates> _activeFields = new List<FieldCoordinates>();

        public FieldManager(IPlayerPositionProvider playerPositionProvider,
            IEnumerable<IMeshUnitByFieldLoader> delayedFloorLoaders,
            IFieldChangeAnalyzer fieldChangeAnalyzer,
            IActiveFieldCalculator activeFieldCalculator)
        {
            _playerPositionProvider = playerPositionProvider;
            _delayedLoaders = delayedFloorLoaders;
            _fieldChangeAnalyzer = fieldChangeAnalyzer;
            _activeFieldCalculator = activeFieldCalculator;
        }

        public void UpdateFieldsByPlayerPosition()
        {
            Position playerPosition = _playerPositionProvider.GetPlayerPosition();

            var allActiveFields = _activeFieldCalculator.CalculateActiveFields(playerPosition);
            var addedFields = _fieldChangeAnalyzer.FindAddedFields(_activeFields, allActiveFields);
            var removedFields = _fieldChangeAnalyzer.FindRemovedFields(_activeFields, allActiveFields);

            foreach(IMeshUnitByFieldLoader loader in _delayedLoaders)
            {
                loader.UpdateMesh(addedFields, removedFields);
            }
            
            _activeFields = allActiveFields;
        }
    }
}
