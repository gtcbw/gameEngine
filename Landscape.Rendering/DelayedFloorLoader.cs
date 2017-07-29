using Engine.Contracts;
using Graphics.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class DelayedFloorLoader : IDelayedFloorLoader
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private IHeightCalculator _heightCalculator;
        private IMeshUnitCollection _floorCollection;
        private uint _indexBufferId;
        private int _numberOfIndices;

        private int _numberOfRows;
        private int _metersPerTriangleSide;

        private class FieldVertices
        {
            public FieldCoordinates Field { set; get; }
            public float[] Vertices { set; get; }
        }

        private List<FieldCoordinates> _fieldQueue = new List<FieldCoordinates>();
        private List<FieldVertices> _fieldVertexQueue = new List<FieldVertices>();
        private Dictionary<int, uint> _vertexIdByFieldId = new Dictionary<int, uint>();

        public DelayedFloorLoader(IBufferObjectFactory bufferObjectFactory,
            IHeightCalculator heightCalculator,
            IMeshUnitCollection floorCollection,
            int numberOfRows, 
            int metersPerTriangleSide)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _heightCalculator = heightCalculator;
            _floorCollection = floorCollection;
            _numberOfRows = numberOfRows;
            _metersPerTriangleSide = metersPerTriangleSide;

            ushort[] indices = CreateStandardIndexMesh();
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);
        }

        void IDelayedFloorLoader.UpdateMesh(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields)
        {
            foreach(FieldCoordinates field in removedFields)
            {
                if (_vertexIdByFieldId.Keys.Contains(field.ID))
                {
                    _floorCollection.RemoveMeshUnit(field.ID);
                    _bufferObjectFactory.Delete(_vertexIdByFieldId[field.ID]);
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
                        var vertexField = _fieldVertexQueue.First(x => x.Field.ID == field.ID);
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

                BufferedMeshUnit bufferedMeshUnit = new BufferedMeshUnit
                {
                    IndexBufferId = _indexBufferId,
                    NumberOfIndices = _numberOfIndices,
                    VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(fieldVertices.Vertices)
                };
                _vertexIdByFieldId.Add(fieldVertices.Field.ID, bufferedMeshUnit.VertexBufferId);
                _floorCollection.AddMeshUnit(fieldVertices.Field.ID, bufferedMeshUnit);
            }
            else if (_fieldQueue.Count > 0)
            {
                FieldCoordinates field = _fieldQueue.ElementAt(0);
                _fieldQueue.RemoveAt(0);
                float[] vertices = CreateVertices(field);
                _fieldVertexQueue.Add(new FieldVertices { Field = field, Vertices = vertices });
            }
        }

        private float[] CreateVertices(FieldCoordinates field)
        {
            int startx = field.X * _metersPerTriangleSide * _numberOfRows;
            int startz = field.Z * _metersPerTriangleSide * _numberOfRows;

            float[] vertices = new float[(_numberOfRows + 1) * (_numberOfRows + 1) * 3];

            for (int z = 0; z < _numberOfRows + 1; z++)
            {
                for (int x = 0; x < _numberOfRows + 1; x++)
                {
                    float xcoord = (x * _metersPerTriangleSide) + startx;
                    float zcoord = (z * _metersPerTriangleSide) + startz;

                    vertices[(((z * (_numberOfRows + 1)) + x) * 3)] = xcoord;
                    vertices[(((z * (_numberOfRows + 1)) + x) * 3) + 1] = (float)_heightCalculator.CalculateHeight(xcoord, zcoord);
                    vertices[(((z * (_numberOfRows + 1)) + x) * 3) + 2] = zcoord;
                }
            }

            return vertices;
        }

        private ushort[] CreateStandardIndexMesh()
        {
            ushort[] indices = new ushort[_numberOfRows * _numberOfRows * 6];

            for (int rowz = 0; rowz < _numberOfRows; rowz++)
            {
                for (int rowx = 0; rowx < _numberOfRows; rowx++)
                {
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 0] = (ushort)(0 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 1] = (ushort)(1 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 2] = (ushort)(1 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));

                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 3] = (ushort)(0 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 4] = (ushort)(1 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 5] = (ushort)(0 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));
                }
            }

            return indices;
        }
    }
}