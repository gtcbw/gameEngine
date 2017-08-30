namespace World.Model
{
    public sealed class ComplexShape
    {
        public double RadiusXZ { set; get; }

        public Cuboid MainCuboid { set; get; }

        public Face[] Faces { set; get; }

        public Cuboid[] SubCuboids { set; get; }
    }
}
