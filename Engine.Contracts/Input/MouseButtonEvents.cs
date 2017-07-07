namespace Engine.Contracts.Input
{
    public sealed class MouseButtonEvents
    {
        public int WheelDelta { get; set; }

        public bool LeftButtonPressed { get; set; }

        public bool RightButtonPressed { get; set; }

        public bool LeftButtonReleased { get; set; }

        public bool RightButtonReleased { get; set; }
    }
}
