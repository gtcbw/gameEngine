using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class StreetVertexCreator : IVertexByFieldCreator
    {
        private IVectorHelper _vectorHelper;
        private IHeightCalculator _heightCalculator;
        private double _halfStreetWidth;
        private double _radius;
        private float _minHeight = 0.1f;

        public StreetVertexCreator(IVectorHelper vectorHelper,
            IHeightCalculator heightCalculator,
            double streetWidth,
            double radius)
        {
            _vectorHelper = vectorHelper;
            _heightCalculator = heightCalculator;
            _halfStreetWidth = streetWidth / 2.0;
            _radius = radius;
        }

        float[] IVertexByFieldCreator.CreateVertices(FieldCoordinates field)
        {
            if (!_streetShapesByFieldId.Keys.Contains(field.ID))
                return null;

            StreetShape shape = _streetShapesByFieldId[field.ID];
            double leftLowCornerX = field.X * _radius * 2;
            double leftLowCornerZ = field.Z * _radius * 2;

            switch(shape)
            {
                case StreetShape.FirstQuarter:
                    return CreateVertices(leftLowCornerX, leftLowCornerZ, 0, 90);
                case StreetShape.SecondQuarter:
                    return CreateVertices(leftLowCornerX, leftLowCornerZ + (_radius * 2), 90, 180);
                case StreetShape.ThirdQuarter:
                    return CreateVertices(leftLowCornerX + (_radius * 2), leftLowCornerZ + (_radius * 2), 180, 270);
                case StreetShape.FourthQuarter:
                    return CreateVertices(leftLowCornerX + (_radius * 2), leftLowCornerZ, 270, 360);
            }
            return CreateVertices(500, 500, 0, 180);
        }

        private float[] CreateVertices(double centerX, double centerZ, int startDegree, int endDegree)
        {
            float[] vertices = new float[(endDegree - startDegree + 1) * 6];
            int index = 0;

            for (int degree = startDegree; degree <= endDegree; degree++)
            {
                var vector = _vectorHelper.ConvertDegreeToVector(degree);

                Position innerVertex = new Position
                {
                    X = centerX + (vector.X * (_radius - _halfStreetWidth)),
                    Z = centerZ + (vector.Z * (_radius - _halfStreetWidth)),
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = centerX + (vector.X * (_radius + _halfStreetWidth)),
                    Z = centerZ + (vector.Z * (_radius + _halfStreetWidth)),
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

        private enum StreetShape
        {
            FirstQuarter,
            SecondQuarter,
            ThirdQuarter,
            FourthQuarter
        }

        private Dictionary<int, StreetShape> _streetShapesByFieldId = new Dictionary<int, StreetShape>
        {
            {1, StreetShape.FirstQuarter },
            {3, StreetShape.SecondQuarter },
            {5, StreetShape.ThirdQuarter },
            {7, StreetShape.FourthQuarter }
        };
    }
}
