using Math.Contracts;

namespace Math
{
    public sealed class HeightCalculator : IHeightCalculator
    {
        private readonly float[] _heightValues;
        private int _sideLength;
        private int _meters;

        private int Ax, Az, Bx, Bz, Cx, Cz, Dx, Dz;
        private double hoehea, hoeheb, hoehec, hoehed;

        private double VektorAx;
        private double VektorAz;
        private double VektorCx;
        private double VektorCz;
        private double VektorlaengeA;
        private double VektorlaengeC;
        private double langerVektorY;
        private double lamdakurz;
        private double lamda;
        private double prozentY;
        private double langerVektorX;
        private double langerVektorZ;

        public HeightCalculator(float[] heightValues, int sideLength, int meters)
        {
            _sideLength = sideLength;
            _meters = meters;
            _heightValues = heightValues;
        }

        double IHeightCalculator.CalculateHeight(double x, double z)
        {
            if ((x < 0) || (z < 0) || (x > (_sideLength - 1) * _meters) || (z >= (_sideLength - 1) * _meters))
                return 0.0;

            Ax = (int)x;
            Az = (int)z;
            if (Ax % _meters != 0)
                Ax -= Ax % _meters;
            if (Az % _meters != 0)
                Az -= Az % _meters;

            if ((x == Ax) && (z == Az))
                return _heightValues[((Az / _meters) * _sideLength) + (Ax / _meters)];

            Bx = Ax + _meters;
            Bz = Az;
            Cx = Ax + _meters;
            Cz = Az + _meters;
            Dx = Ax;
            Dz = Az + _meters;

            VektorAx = x - Ax;
            VektorAz = z - Az;
            VektorCx = x - Cx;
            VektorCz = z - Cz;
            VektorlaengeA = (VektorAx * VektorAx) + (VektorAz * VektorAz);
            VektorlaengeC = (VektorCx * VektorCx) + (VektorCz * VektorCz);

            if (VektorlaengeA < VektorlaengeC)
            {
                hoehea = _heightValues[((Az / _meters) * _sideLength) + (Ax / _meters)];
                hoeheb = _heightValues[((Bz / _meters) * _sideLength) + (Bx / _meters)];
                hoehed = _heightValues[((Dz / _meters) * _sideLength) + (Dx / _meters)];
                hoehec = ((hoeheb + hoehed) / 2) - (hoehea - ((hoeheb + hoehed) / 2));
            }
            else
            {
                hoehec = _heightValues[((Cz / _meters) * _sideLength) + (Cx / _meters)];
                hoeheb = _heightValues[((Bz / _meters) * _sideLength) + (Bx / _meters)];
                hoehed = _heightValues[((Dz / _meters) * _sideLength) + (Dx / _meters)];
                hoehea = ((hoeheb + hoehed) / 2) - (hoehec - ((hoeheb + hoehed) / 2));
            }

            ///2Dvektoren sind Ax, Az.   Was ist Lamda?
            if (VektorAx > VektorAz)
            {
                if (VektorAx != 0)
                    lamda = (Bx - Ax) / VektorAx;
                else
                    lamda = 0;

                prozentY = (Cz - (Az + (lamda * VektorAz))) / (Cz - Bz);

                langerVektorX = Bx - Ax;
                langerVektorY = (prozentY * (hoeheb - hoehec)) + hoehec - hoehea;
                if (langerVektorX != 0)
                    lamdakurz = (x - Ax) / langerVektorX;
                else
                    lamdakurz = 0;
            }
            else
            {
                if (VektorAz != 0)
                    lamda = (Dz - Az) / VektorAz;
                else
                    lamda = 0;

                prozentY = (Cx - (Ax + (lamda * VektorAx))) / (Cx - Dx);

                langerVektorZ = Dz - Az;
                langerVektorY = (prozentY * (hoehed - hoehec)) + hoehec - hoehea;
                if (langerVektorZ != 0)
                    lamdakurz = (z - Az) / langerVektorZ;
                else
                    lamdakurz = 0;
            }

            return hoehea + (lamdakurz * langerVektorY);
        }
    }
}
