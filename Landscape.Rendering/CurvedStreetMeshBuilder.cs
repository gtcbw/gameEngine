using Graphics.Contracts;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class CurvedStreetMeshBuilder
    {
        private IVectorHelper _vectorHelper;
        private IHeightCalculator _heightCalculator;
        private IBufferObjectFactory _bufferObjectFactory;
        private double _halfStreetWidth;
        private double _radius;
        private  float _minHeight = 0.06f;

        public CurvedStreetMeshBuilder(IVectorHelper vectorHelper, 
            IHeightCalculator heightCalculator,
            IBufferObjectFactory bufferObjectFactory,
            double streetWidth,
            double radius)
        {
            _vectorHelper = vectorHelper;
            _heightCalculator = heightCalculator;
            _bufferObjectFactory = bufferObjectFactory;
            _halfStreetWidth = streetWidth / 2.0;
            _radius = radius;
        }

        public BufferedMeshUnit BuildMeshUnit(Position circleCenter, int startDegree, int endDegree)
        {
            float[] vertices = CreateVertices(circleCenter, startDegree, endDegree);

            ushort[] indexArray = CreateIndexArray(90);
            return new BufferedMeshUnit
            {
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices),
                IndexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indexArray),
                NumberOfIndices = indexArray.Length
            };
        }

        private ushort[] CreateIndexArray(int numberOfQuads)
        {
            ushort[] indices = new ushort[numberOfQuads * 6];

            for(int i = 0; i < numberOfQuads; i++)
            {
                indices[(i * 6) + 0] = (ushort)((i * 2) + 0);
                indices[(i * 6) + 1] = (ushort)(((i + 1) * 2) + 1);
                indices[(i * 6) + 2] = (ushort)(((i + 1) * 2) + 0);

                indices[(i * 6) + 3] = (ushort)((i * 2) + 0);
                indices[(i * 6) + 4] = (ushort)((i * 2) + 1);
                indices[(i * 6) + 5] = (ushort)(((i + 1) * 2) + 1);
            }

            return indices;
        }

        public float[] CreateVertices(Position circleCenter, int startDegree, int endDegree)
        {
            float[] vertices = new float[(endDegree - startDegree + 1) * 6];
            int index = 0;

            for (int degree = startDegree; degree <= endDegree; degree++)
            {
                var vector = _vectorHelper.ConvertDegreeToVector(degree);

                Position innerVertex = new Position
                {
                    X = circleCenter.X + (vector.X * (_radius - _halfStreetWidth)),
                    Z = circleCenter.Z + (vector.Z * (_radius - _halfStreetWidth)),
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = circleCenter.X + (vector.X * (_radius + _halfStreetWidth)),
                    Z = circleCenter.Z + (vector.Z * (_radius + _halfStreetWidth)),
                };
                outerVertex.Y = _heightCalculator.CalculateHeight(outerVertex.X, outerVertex.Z) + _minHeight;

                vertices[index++] = (float)innerVertex.X;
                vertices[index++] = (float)innerVertex.Y;
                vertices[index++] = (float)innerVertex.Z;

                vertices[index++] = (float)outerVertex.X;
                vertices[index++] = (float)outerVertex.Y;
                vertices[index++] = (float)outerVertex.Z;
            }

            return vertices;
        }
    }
}
