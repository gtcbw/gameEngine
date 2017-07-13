using Math.Contracts;

namespace Math
{
    public sealed class HeightCalculator : IHeightCalculator
    {
        private readonly float[] _heightValues;
        private int _sideLength;
        private int _meters;

        private int Ax, Az, Bx, Bz, Cx, Cz, Dx, Dz;
        private double hoehea, hoeheb, hoehec, hoehed, differenz;

        private double VektorAx;
        private double VektorAz;
        private double VektorCx;
        private double VektorCz;
        private double VektorlaengeA;
        private double VektorlaengeC;
        private double langerVektorY;
        private double lamdakurz;
        private double hoechstesY;
        private double lamda;
        private double prozentY;
        private double langerVektorX;
        private double langerVektorZ;
        private double gesuchtesZ, gesuchtesX;

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

            if ((x == Ax) && (z == Az))
                return _heightValues[((Az / _meters) * _sideLength) + (Ax / _meters)];

            if (VektorlaengeA < VektorlaengeC)
            {
                hoehea = _heightValues[((Az / _meters) * _sideLength) + (Ax / _meters)];
                hoeheb = _heightValues[((Bz / _meters) * _sideLength) + (Bx / _meters)];
                hoehed = _heightValues[((Dz / _meters) * _sideLength) + (Dx / _meters)];
                differenz = hoehea - ((hoeheb + hoehed) / 2);

                hoehec = ((hoeheb + hoehed) / 2) - differenz;
            }
            else
            {
                hoehec = _heightValues[((Cz / _meters) * _sideLength) + (Cx / _meters)];
                hoeheb = _heightValues[((Bz / _meters) * _sideLength) + (Bx / _meters)];
                hoehed = _heightValues[((Dz / _meters) * _sideLength) + (Dx / _meters)];
                differenz = hoehec - ((hoeheb + hoehed) / 2);

                hoehea = ((hoeheb + hoehed) / 2) - differenz;
            }

            ///2Dvektoren sind Ax, Az.   Was ist Lamda?
            if (VektorAx > VektorAz)
            {
                if (VektorAx != 0)
                    lamda = (Bx - Ax) / VektorAx;
                else
                    lamda = 0;

                gesuchtesZ = Az + (lamda * VektorAz);
                prozentY = (Cz - gesuchtesZ) / (Cz - Bz);

                hoechstesY = (prozentY * (hoeheb - hoehec)) + hoehec;

                langerVektorX = Bx - Ax;
                langerVektorY = hoechstesY - hoehea;
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

                gesuchtesX = Ax + (lamda * VektorAx);
                prozentY = (Cx - gesuchtesX) / (Cx - Dx);

                hoechstesY = (prozentY * (hoehed - hoehec)) + hoehec;

                langerVektorZ = Dz - Az;
                langerVektorY = hoechstesY - hoehea;
                if (langerVektorZ != 0)
                    lamdakurz = (z - Az) / langerVektorZ;
                else
                    lamdakurz = 0;
            }

            return hoehea + (lamdakurz * langerVektorY);
        }
    }
}
