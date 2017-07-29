using Engine.Contracts;
using Graphics.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class DelayedStreetLoader : IDelayedFloorLoader
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private IHeightCalculator _heightCalculator;
        private IMeshUnitCollection _streetCollection;
        private uint _indexBufferId;
        private uint _texCoordBufferId;
        private int _numberOfIndices;

        private int _numberOfQuads;
        private int _metersPerTriangleSide;

        private class FieldVertices
        {
            public FieldCoordinates Field { set; get; }
            public float[] Vertices { set; get; }
        }

        private List<FieldCoordinates> _fieldQueue = new List<FieldCoordinates>();
        private List<FieldVertices> _fieldVertexQueue = new List<FieldVertices>();

        public DelayedStreetLoader(IBufferObjectFactory bufferObjectFactory,
            IHeightCalculator heightCalculator,
            IMeshUnitCollection streetCollection,
            int numberOfQuads, 
            int metersPerTriangleSide)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _heightCalculator = heightCalculator;
            _streetCollection = streetCollection;
            _numberOfQuads = numberOfQuads;
            _metersPerTriangleSide = metersPerTriangleSide;

            ushort[] indices = CreateIndexArray(_numberOfQuads);
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);

            float[] texcoords = CreateTextureCoordinates(_numberOfQuads);
            _texCoordBufferId = _bufferObjectFactory.GenerateTextureCoordBuffer(texcoords);
        }

        void IDelayedFloorLoader.UpdateMesh(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields)
        {
            foreach(FieldCoordinates field in removedFields)
            {
                _streetCollection.RemoveMeshUnit(field.ID);
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
                _streetCollection.AddMeshUnit(fieldVertices.Field.ID, bufferedMeshUnit);
            }
            else if (_fieldQueue.Count > 0)
            {

            }
        }

        private ushort[] CreateIndexArray(int numberOfQuads)
        {
            ushort[] indices = new ushort[numberOfQuads * 6];

            for (int i = 0; i < numberOfQuads; i++)
            {
                indices[i * 6] = (ushort)(i * 2);
                indices[(i * 6) + 1] = (ushort)(((i + 1) * 2) + 1);
                indices[(i * 6) + 2] = (ushort)((i + 1) * 2);

                indices[(i * 6) + 3] = (ushort)(i * 2);
                indices[(i * 6) + 4] = (ushort)((i * 2) + 1);
                indices[(i * 6) + 5] = (ushort)(((i + 1) * 2) + 1);
            }

            return indices;
        }

        private float[] CreateTextureCoordinates(int numberOfQuads)
        {
            float[] texCoords = new float[(numberOfQuads + 1) * 4];

            for (int i = 0; i <= numberOfQuads; i++)
            {
                if (i % 2 == 0)
                {
                    texCoords[i * 4] = 0;
                    texCoords[(i * 4) + 1] = 0;
                    texCoords[(i * 4) + 2] = 1;
                    texCoords[(i * 4) + 3] = 0;
                }
                else
                {
                    texCoords[i * 4] = 0;
                    texCoords[(i * 4) + 1] = 1;
                    texCoords[(i * 4) + 2] = 1;
                    texCoords[(i * 4) + 3] = 1;
                }
            }

            return texCoords;
        }
    }
}