using Engine.Contracts.Input;

namespace Engine.Framework
{
    public sealed class KeyMapper : IKeyMapper
    {
        private readonly IPressedKeyDetector _pressedKeyDetector;

        public KeyMapper(IPressedKeyDetector pressedKeyDetector)
        {
            _pressedKeyDetector = pressedKeyDetector;
        }

        MovementInstruction IKeyMapper.GetMappedKeys()
        {
            MovementInstruction movementInstruction = new MovementInstruction();

            movementInstruction.WalkForward = (_pressedKeyDetector.IsKeyDown(Keys.Up) || _pressedKeyDetector.IsKeyDown(Keys.W));
            movementInstruction.WalkBackward = (_pressedKeyDetector.IsKeyDown(Keys.Down) || _pressedKeyDetector.IsKeyDown(Keys.S));
            movementInstruction.StrafeLeft = (_pressedKeyDetector.IsKeyDown(Keys.Left) || _pressedKeyDetector.IsKeyDown(Keys.A));
            movementInstruction.StrafeRight = (_pressedKeyDetector.IsKeyDown(Keys.Right) || _pressedKeyDetector.IsKeyDown(Keys.D));

            if (movementInstruction.WalkForward && movementInstruction.WalkBackward)
            {
                movementInstruction.WalkForward = movementInstruction.WalkBackward = false;
            }

            if (movementInstruction.StrafeLeft && movementInstruction.StrafeRight)
            {
                movementInstruction.StrafeLeft = movementInstruction.StrafeRight = false;
            }

            return movementInstruction;
        }
    }
}
