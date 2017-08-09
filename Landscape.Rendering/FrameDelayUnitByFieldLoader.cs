using Engine.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FrameDelayUnitByFieldLoader : IFieldChangeObserver
    {
        private IFieldChangeObserver _meshUnitByFieldLoader;
        private List<FieldCoordinates> _addedFields = new List<FieldCoordinates>();
        private int _delay;
        private int _numberOfFramesToDelay;

        public FrameDelayUnitByFieldLoader(IFieldChangeObserver meshUnitByFieldLoader, 
            int numberOfFramesToDelay)
        {
            _meshUnitByFieldLoader = meshUnitByFieldLoader;
            _numberOfFramesToDelay = numberOfFramesToDelay;
        }

        void IFieldChangeObserver.NotifyChangedFields(IEnumerable<FieldCoordinates> addedFields, 
            IEnumerable<FieldCoordinates> removedFields)
        {
            if (removedFields.Any() && _addedFields.Any())
            {
                List<FieldCoordinates> removedFieldList = new List<FieldCoordinates>();
                foreach(FieldCoordinates field in removedFields)
                {
                    FieldCoordinates foundField = _addedFields.FirstOrDefault(x => x.ID == field.ID);
                    if (foundField != null)
                        _addedFields.Remove(foundField);
                    else
                        removedFieldList.Add(field);
                }

                removedFields = removedFieldList;
            }

            if (addedFields.Any())
            {
                _addedFields.AddRange(addedFields);
                _delay += _numberOfFramesToDelay;
            }

            if (_delay == 0)
            {
                _meshUnitByFieldLoader.NotifyChangedFields(_addedFields, removedFields);
                _addedFields.Clear();
            }
            else
            {
                _meshUnitByFieldLoader.NotifyChangedFields(addedFields.Any() 
                    ? new List<FieldCoordinates>() 
                    : addedFields, 
                removedFields);
            }

            if (_delay > 0)
                _delay--;
        }
    }
}
