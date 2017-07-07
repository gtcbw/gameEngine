using System;
using OpenTK.Input;
using Engine.Contracts.Input;

namespace Game.OpenTkDependencies
{
    public sealed class PressedKeyDetector : IPressedKeyDetector
    {
        private KeyboardDevice KeyboardDevice { set; get; }

        public PressedKeyDetector(KeyboardDevice device)
        {
            KeyboardDevice = device;
        }

        bool IPressedKeyDetector.IsKeyDown(Keys key)
        {
            return KeyboardDevice[MapKeys(key)];
        }

        private Key MapKeys(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    return Key.A;
                case Keys.B:
                    return Key.B;
                case Keys.C:
                    return Key.C;
                case Keys.D:
                    return Key.D;
                case Keys.E:
                    return Key.E;
                case Keys.F:
                    return Key.F;
                case Keys.G:
                    return Key.G;
                case Keys.H:
                    return Key.H;
                case Keys.I:
                    return Key.I;
                case Keys.J:
                    return Key.J;
                case Keys.K:
                    return Key.K;
                case Keys.L:
                    return Key.L;
                case Keys.M:
                    return Key.M;
                case Keys.N:
                    return Key.N;
                case Keys.O:
                    return Key.O;
                case Keys.P:
                    return Key.P;
                case Keys.Q:
                    return Key.Q;
                case Keys.R:
                    return Key.R;
                case Keys.S:
                    return Key.S;
                case Keys.T:
                    return Key.T;
                case Keys.U:
                    return Key.U;
                case Keys.V:
                    return Key.V;
                case Keys.W:
                    return Key.W;
                case Keys.X:
                    return Key.X;
                case Keys.Y:
                    return Key.Y;
                case Keys.Z:
                    return Key.Z;
                case Keys.Num0:
                    return Key.Number0;
                case Keys.Num1:
                    return Key.Number1;
                case Keys.Num2:
                    return Key.Number2;
                case Keys.Num3:
                    return Key.Number3;
                case Keys.Num4:
                    return Key.Number4;
                case Keys.Num5:
                    return Key.Number5;
                case Keys.Num6:
                    return Key.Number6;
                case Keys.Num7:
                    return Key.Number7;
                case Keys.Num8:
                    return Key.Number8;
                case Keys.Num9:
                    return Key.Number9;
                case Keys.ShiftLeft:
                    return Key.ShiftLeft;
                case Keys.ShiftRight:
                    return Key.ShiftRight;
                case Keys.ControlLeft:
                    return Key.ControlLeft;
                case Keys.ControlRight:
                    return Key.ControlRight;
                case Keys.Escape:
                    return Key.Escape;
                case Keys.Up:
                    return Key.Up;
                case Keys.Down:
                    return Key.Down;
                case Keys.Left:
                    return Key.Left;
                case Keys.Right:
                    return Key.Right;
                case Keys.Space:
                    return Key.Space;
                case Keys.F1:
                    return Key.F1;
                case Keys.F2:
                    return Key.F2;
                case Keys.F3:
                    return Key.F3;
                case Keys.F4:
                    return Key.F4;
                case Keys.F5:
                    return Key.F5;
                case Keys.F6:
                    return Key.F6;
                case Keys.F7:
                    return Key.F7;
                case Keys.F8:
                    return Key.F8;
                case Keys.F9:
                    return Key.F9;
                case Keys.F10:
                    return Key.F10;
                case Keys.F11:
                    return Key.F11;
                case Keys.F12:
                    return Key.F12;
                case Keys.Enter:
                    return Key.Enter;
                case Keys.Delete:
                    return Key.Delete;
            }

            throw new Exception("missing key mapping!");
        }
    }
}
