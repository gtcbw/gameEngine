using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class DelayedMeshUnitLoader : IFieldChangeObserver
    {
        private IVertexByFieldCreator _vertexByFieldCreator;
        private IMeshUnitCreator _meshUnitCreator;
        private IMeshUnitCollection _meshUnitCollection;

        private class FieldVertices
        {
            public FieldCoordinates Field { set; get; }
            public float[] Vertices { set; get; }
        }

        private List<FieldCoordinates> _fieldQueue = new List<FieldCoordinates>();
        private List<FieldVertices> _fieldVertexQueue = new List<FieldVertices>();
        private Dictionary<int, VertexBufferUnit> _vertexIdByFieldId = new Dictionary<int, VertexBufferUnit>();

        public DelayedMeshUnitLoader(IVertexByFieldCreator vertexByFieldCreator,
            IMeshUnitCreator meshUnitCreator,
            IMeshUnitCollection floorCollection)
        {
            _vertexByFieldCreator = vertexByFieldCreator;
            _meshUnitCreator = meshUnitCreator;
            _meshUnitCollection = floorCollection;
        }

        void IFieldChangeObserver.NotifyChangedFields(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields)
        {
            foreach(FieldCoordinates field in removedFields)
            {
                if (_vertexIdByFieldId.Keys.Contains(field.ID))
                {
                    _meshUnitCollection.RemoveMeshUnit(field.ID);
                    _meshUnitCreator.DeleteMeshUnit(_vertexIdByFieldId[field.ID]);
                    _vertexIdByFieldId.Remove(field.ID);
                }
                else
                {
                    var queuedField = _fieldQueue.FirstOrDefault(x => x.ID == field.ID);
                    if (queuedField != null)
                    {
                        _fieldQueue.Remove(queuedField);
                    }
                    else
                    {
                        var vertexField = _fieldVertexQueue.FirstOrDefault(x => x.Field.ID == field.ID);
                        if(vertexField != null)
                            _fieldVertexQueue.Remove(vertexField);
                    }
                }
            }

            if(addedFields.Count() > 0)
                _fieldQueue.AddRange(addedFields);

            if (_fieldVertexQueue.Count > 0)
            {
                FieldVertices fieldVertices = _fieldVertexQueue.ElementAt(0);
                _fieldVertexQueue.RemoveAt(0);

                VertexBufferUnit bufferedMeshUnit = _meshUnitCreator.CreateMeshUnit(fieldVertices.Vertices);

                _vertexIdByFieldId.Add(fieldVertices.Field.ID, bufferedMeshUnit);
                _meshUnitCollection.AddMeshUnit(fieldVertices.Field.ID, bufferedMeshUnit);
            }
            else if (_fieldQueue.Count > 0)
            {
                FieldCoordinates field = _fieldQueue.ElementAt(0);
                _fieldQueue.RemoveAt(0);
                float[] vertices = _vertexByFieldCreator.CreateVertices(field);
                if (vertices != null)
                    _fieldVertexQueue.Add(new FieldVertices { Field = field, Vertices = vertices });
            }
        }
    }
}