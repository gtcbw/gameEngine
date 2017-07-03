namespace Graphics.Contracts
{
    public interface IScreen
    {
        double AspectRatio { get; }

        Resolution CurrentResolution { get; }

        void ChangeResolution(Resolution desiredResolution);
    }
}
