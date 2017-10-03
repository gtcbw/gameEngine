namespace World.Model
{
    public sealed class Cuboid
    {
        public double SideLengthY
        {
            set { HalfSideLengthY = value / 2.0; }
            get { return HalfSideLengthY * 2.0; }
        }

        public double SideLengthX
        {
            set { HalfSideLengthX = value / 2.0; }
            get { return HalfSideLengthX * 2.0; }
        }

        public double SideLengthZ
        {
            set { HalfSideLengthZ = value / 2.0; }
            get { return HalfSideLengthZ * 2.0; }
        }

        public double HalfSideLengthY { set; get; }

        public double HalfSideLengthX { set; get; }

        public double HalfSideLengthZ { set; get; }

        public Position Center { set; get; }
    }
}
