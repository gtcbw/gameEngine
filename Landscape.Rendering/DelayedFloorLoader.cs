using Engine.Contracts;
using Graphics.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class DelayedFloorLoader
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private IHeightCalculator _heightCalculator;
        private IFloorCollection _floorCollection;
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

        public DelayedFloorLoader(IBufferObjectFactory bufferObjectFactory,
            IHeightCalculator heightCalculator,
            IFloorCollection floorCollection,
            int numberOfRows, 
            int metersPerTriangleSide)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _heightCalculator = heightCalculator;
            _floorCollection = floorCollection;
            _numberOfRows = numberOfRows;
            _metersPerTriangleSide = metersPerTriangleSide;

            ushort[] indices = CreateStandardIndexMesh(_numberOfRows);
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);
        }

        public void UpdateMesh(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields)
        {
            foreach(FieldCoordinates field in removedFields)
            {
                _floorCollection.RemoveFloorPart(field.ID);
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
                _floorCollection.AddFloorPart(fieldVertices.Field.ID, bufferedMeshUnit);
            }
            else if (_fieldQueue.Count > 0)
            {
                FieldCoordinates field = _fieldQueue.ElementAt(0);
                _fieldQueue.RemoveAt(0);
                float[] vertices = CreateVertices(_numberOfRows, _metersPerTriangleSide, field.X * _metersPerTriangleSide, field.Z * _metersPerTriangleSide);
                _fieldVertexQueue.Add(new FieldVertices { Field = field, Vertices = vertices });
            }
        }

        private float[] CreateVertices(int numberOfRows, int metersPerTriangleSide, int startx, int startz)
        {
            float[] vertices = new float[(numberOfRows + 1) * (numberOfRows + 1) * 3];

            for (int z = 0; z < numberOfRows + 1; z++)
            {
                for (int x = 0; x < numberOfRows + 1; x++)
                {
                    float xcoord = (x * metersPerTriangleSide) + startx;
                    float zcoord = (z * metersPerTriangleSide) + startz;

                    vertices[(((z * (numberOfRows + 1)) + x) * 3)] = xcoord;
                    vertices[(((z * (numberOfRows + 1)) + x) * 3) + 1] = (float)_heightCalculator.CalculateHeight(xcoord, zcoord);
                    vertices[(((z * (numberOfRows + 1)) + x) * 3) + 2] = zcoord;
                }
            }

            return vertices;
        }

        private ushort[] CreateStandardIndexMesh(int numberOfRows)
        {
            ushort[] indices = new ushort[numberOfRows * numberOfRows * 6];

            for (int rowz = 0; rowz < numberOfRows; rowz++)
            {
                for (int rowx = 0; rowx < numberOfRows; rowx++)
                {
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 0] = (ushort)(0 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 1] = (ushort)(1 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 2] = (ushort)(1 + (rowx + ((rowz + 1) * (numberOfRows + 1))));

                    indices[((rowx + (rowz * numberOfRows)) * 6) + 3] = (ushort)(0 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 4] = (ushort)(1 + (rowx + ((rowz + 1) * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 5] = (ushort)(0 + (rowx + ((rowz + 1) * (numberOfRows + 1))));
                }
            }

            return indices;
        }
    }
}
