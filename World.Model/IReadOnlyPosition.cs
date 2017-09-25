namespace World.Model
{
    public interface IReadOnlyPosition
    {
        double X { get; }

        double Y { get; }

        double Z {  get; }

        Position Clone();
    }
}
