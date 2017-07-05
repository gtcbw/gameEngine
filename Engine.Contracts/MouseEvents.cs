namespace Engine.Contracts
{
    public sealed class MouseEvents
    {
        public double PositionX { set; get; }

        public double PositionY { set; get; }

        public double PositionXDelta { get; set; }

        public double PositionYDelta { get; set; }

        public int WheelDelta { get; set; }

        public bool LeftButtonPressed { get; set; }

        public bool RightButtonPressed { get; set; }

        public bool LeftButtonReleased { get; set; }

        public bool RightButtonReleased { get; set; }
    }
}
