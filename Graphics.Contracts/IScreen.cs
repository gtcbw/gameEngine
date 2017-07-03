namespace Graphics.Contracts
{
    public class Resolution
    {
        public int X { set; get; }

        public int Y { set; get; }
    }

    public interface IScreen
    {
        double AspectRatio { get; }

        Resolution CurrentResolution { get; }

        void ChangeResolution(Resolution desiredResolution);
    }
}
