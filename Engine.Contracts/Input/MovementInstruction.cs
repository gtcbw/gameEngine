namespace Engine.Contracts.Input
{
    public sealed class MovementInstruction
    {
        public bool WalkForward { set; get; }

        public bool WalkBackward { set; get; }

        public bool StrafeLeft { set; get; }

        public bool StrafeRight { set; get; }
    }
}
