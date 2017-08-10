using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Engine.Framework
{
    public sealed class FieldVisibilityDeterminator : IFieldVisibilityDeterminator, IActiveFieldCalculator
    {
        private IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private IActiveFieldCalculator _activeFieldCalculator;
        private IEnumerable<FieldCoordinates> _activeFields;

        private int[][] _invisibleFields = new int[8][];

        public FieldVisibilityDeterminator(IPlayerViewDirectionProvider playerViewDirectionProvider,
            IActiveFieldCalculator activeFieldCalculator)
        {
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _activeFieldCalculator = activeFieldCalculator;

            _invisibleFields[0] = new[] { 0, 3, 6 };
            _invisibleFields[1] = new[] { 0, 6, 2 };
            _invisibleFields[2] = new[] { 0, 1, 2 };
            _invisibleFields[3] = new[] { 0, 2, 8 };
            _invisibleFields[4] = new[] { 2, 5, 8 };
            _invisibleFields[5] = new[] { 2, 8, 6 };
            _invisibleFields[6] = new[] { 6, 7, 8 };
            _invisibleFields[7] = new[] { 6, 0, 8 };
        }

        IEnumerable<FieldCoordinates> IActiveFieldCalculator.CalculateActiveFields(Position position)
        {
            _activeFields = _activeFieldCalculator.CalculateActiveFields(position);
            return _activeFields;
        }

        bool IFieldVisibilityDeterminator.FieldIsVisible(int id)
        {
            double degreeXZ = _playerViewDirectionProvider.GetViewDirection().DegreeXZ;

            FieldCoordinates field = _activeFields.FirstOrDefault(x => x.ID == id);

            if (field == null)
                return true;

            int relativeId = field.RelativeID;

            int[] invisibleFields = GetInvisibleFields(degreeXZ);
            return invisibleFields[0] != relativeId && invisibleFields[1] != relativeId && invisibleFields[2] != relativeId;
        }

        private int[] GetInvisibleFields(double degreeXZ)
        {
            var direction = _playerViewDirectionProvider.GetViewDirection();

            var correction = 20.0;

            if (degreeXZ < 22.5 + correction)
                return _invisibleFields[0];
            if (degreeXZ < 45 + 22.5 - correction)
                return _invisibleFields[1];
            if (degreeXZ < 90 + 22.5 + correction)
                return _invisibleFields[2];
            if (degreeXZ < 135 + 22.5 - correction)
                return _invisibleFields[3];
            if (degreeXZ < 180 + 22.5 + correction)
                return _invisibleFields[4];
            if (degreeXZ < 225 + 22.5 - correction)
                return _invisibleFields[5];
            if (degreeXZ < 270 + 22.5 + correction)
                return _invisibleFields[6];
            if (degreeXZ < 315 + 22.5 - correction)
                return _invisibleFields[7];

            return _invisibleFields[0];
        }
    }
}
