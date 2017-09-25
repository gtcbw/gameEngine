namespace World.Model
{
    public sealed class Position : IReadOnlyPosition
    {
        public double X { set; get; }

        public double Y { set; get; }

        public double Z { set; get; }

        public Position Clone()
        {
            return new Position { X = X, Y = Y, Z = Z };
        }
    }
}
