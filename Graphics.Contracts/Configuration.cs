namespace Graphics.Contracts
{
    public sealed class Configuration
    {
        public Resolution Resolution { set; get; }

        public bool InvertMouse { set; get; }

        public int SoundVolume { set; get; }

        public double MouseSensitivity { set; get; }
    }
}
