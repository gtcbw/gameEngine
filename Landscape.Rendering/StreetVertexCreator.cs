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
                case StreetShape.LineHorizontal:
                    return CreateHorizontalLine(leftLowCornerX + (_radius * 2), leftLowCornerZ + _radius);
                case StreetShape.LineCurveHorizontal:
                    return CreateHorizontalCurvedLine(leftLowCornerX + (_radius * 2), leftLowCornerZ + _radius);
                case StreetShape.LineVertical:
                    return CreateVerticalLine(leftLowCornerX + _radius, leftLowCornerZ);
                case StreetShape.LineCurveVertical:
                    return CreateVerticalCurvedLine(leftLowCornerX + _radius, leftLowCornerZ);
                case StreetShape.CurveFirstQuarter:
                    return CreateVerticesForCurve(leftLowCornerX, leftLowCornerZ, 0, 90);
                case StreetShape.CurveFourthQuarter:
                    return CreateVerticesForCurve(leftLowCornerX + (_radius * 2), leftLowCornerZ, 90, 180);
                case StreetShape.CurveThirdQuarter:
                    return CreateVerticesForCurve(leftLowCornerX + (_radius * 2), leftLowCornerZ + (_radius * 2), 180, 270);
                case StreetShape.CurveSecondQuarter:
                    return CreateVerticesForCurve(leftLowCornerX, leftLowCornerZ + (_radius * 2), 270, 360);
            }
            return null;
        }

        private float[] CreateVerticalCurvedLine(double centerX, double centerZ)
        {
            float[] vertices = new float[91 * 6];
            int index = 0;

            double innerX, z, outerX;

            innerX = centerX - _halfStreetWidth;
            outerX = centerX + _halfStreetWidth;
            z = centerZ;

            for (int i = 0; i < 91; i++)
            {
                double sinus = System.Math.Sin(i / 22.5 * System.Math.PI) * 10;
                sinus *= 1.0 - (System.Math.Abs(centerZ + _radius - z) / _radius);

                Position innerVertex = new Position
                {
                    X = innerX + sinus,
                    Z = z,
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = outerX + sinus,
                    Z = z,
                };
                outerVertex.Y = _heightCalculator.CalculateHeight(outerVertex.X, outerVertex.Z) + _minHeight;

                z += _radius * 2 / 90.0;

                vertices[index++] = (float)innerVertex.X;
                vertices[index++] = (float)innerVertex.Y;
                vertices[index++] = (float)innerVertex.Z;

                vertices[index++] = (float)outerVertex.X;
                vertices[index++] = (float)outerVertex.Y;
                vertices[index++] = (float)outerVertex.Z;
            }

            return vertices;
        }

        private float[] CreateHorizontalCurvedLine(double centerX, double centerZ)
        {
            float[] vertices = new float[91 * 6];
            int index = 0;

            double innerZ, x, outerZ;

            innerZ = centerZ - _halfStreetWidth;
            outerZ = centerZ + _halfStreetWidth;
            x = centerX;

            for (int i = 0; i < 91; i++)
            {
                double sinus = System.Math.Sin(i / 22.5 * System.Math.PI) * 10;
                sinus *= 1.0 - (System.Math.Abs(centerX - _radius - x) / _radius);
                
                Position innerVertex = new Position
                {
                    X = x,
                    Z = innerZ + sinus,
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = x,
                    Z = outerZ + sinus,
                };
                outerVertex.Y = _heightCalculator.CalculateHeight(outerVertex.X, outerVertex.Z) + _minHeight;

                x -= _radius * 2 / 90.0;

                vertices[index++] = (float)innerVertex.X;
                vertices[index++] = (float)innerVertex.Y;
                vertices[index++] = (float)innerVertex.Z;

                vertices[index++] = (float)outerVertex.X;
                vertices[index++] = (float)outerVertex.Y;
                vertices[index++] = (float)outerVertex.Z;
            }

            return vertices;
        }

        private float[] CreateHorizontalLine(double centerX, double centerZ)
        {
            float[] vertices = new float[91 * 6];
            int index = 0;

            double innerZ, x, outerZ;

            innerZ = centerZ - _halfStreetWidth;
            outerZ = centerZ + _halfStreetWidth;
            x = centerX;

            for (int i = 0; i < 91; i++)
            {
                Position innerVertex = new Position
                {
                    X = x,
                    Z = innerZ,
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = x,
                    Z = outerZ,
                };
                outerVertex.Y = _heightCalculator.CalculateHeight(outerVertex.X, outerVertex.Z) + _minHeight;

                x -= _radius * 2 / 90.0;

                vertices[index++] = (float)innerVertex.X;
                vertices[index++] = (float)innerVertex.Y;
                vertices[index++] = (float)innerVertex.Z;

                vertices[index++] = (float)outerVertex.X;
                vertices[index++] = (float)outerVertex.Y;
                vertices[index++] = (float)outerVertex.Z;
            }

            return vertices;
        }

        private float[] CreateVerticalLine(double centerX, double centerZ)
        {
            float[] vertices = new float[91 * 6];
            int index = 0;

            double innerX, z, outerX;

            innerX = centerX - _halfStreetWidth;
            outerX = centerX + _halfStreetWidth;
            z = centerZ;

            for (int i = 0; i < 91; i++)
            {
                Position innerVertex = new Position
                {
                    X = innerX,
                    Z = z,
                };
                innerVertex.Y = _heightCalculator.CalculateHeight(innerVertex.X, innerVertex.Z) + _minHeight;

                Position outerVertex = new Position
                {
                    X = outerX,
                    Z = z,
                };
                outerVertex.Y = _heightCalculator.CalculateHeight(outerVertex.X, outerVertex.Z) + _minHeight;

                z += _radius * 2 / 90.0;

                vertices[index++] = (float)innerVertex.X;
                vertices[index++] = (float)innerVertex.Y;
                vertices[index++] = (float)innerVertex.Z;

                vertices[index++] = (float)outerVertex.X;
                vertices[index++] = (float)outerVertex.Y;
                vertices[index++] = (float)outerVertex.Z;
            }

            return vertices;
        }

        private float[] CreateVerticesForCurve(double centerX, double centerZ, int startDegree, int endDegree)
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
            LineVertical,
            LineHorizontal,
            LineCurveVertical,
            LineCurveHorizontal,
            CurveFirstQuarter,
            CurveSecondQuarter,
            CurveThirdQuarter,
            CurveFourthQuarter
        }

        private Dictionary<int, StreetShape> _streetShapesByFieldId = new Dictionary<int, StreetShape>
        {
            {8, StreetShape.CurveFirstQuarter },
            {2, StreetShape.CurveSecondQuarter },
            {0, StreetShape.CurveThirdQuarter },
            {6, StreetShape.CurveFourthQuarter },
            {1, StreetShape.LineHorizontal },
            {7, StreetShape.LineCurveHorizontal },
            {3, StreetShape.LineVertical },
            {5, StreetShape.LineCurveVertical },
        };
    }
}
