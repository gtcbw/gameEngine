using Engine.Contracts;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class ColorRenderer : IRenderingElement
    {
        private IRenderingElement _innerElement;
        private IColorSetter _colorSetter;

        public ColorRenderer(IRenderingElement innerElement, 
            IColorSetter colorSetter)
        {
            _innerElement = innerElement;
            _colorSetter = colorSetter;
        }

        void IRenderingElement.Render()
        {
            _colorSetter.SetColor(0.33f, 0.66f, 0.0f);
            _innerElement.Render();
            _colorSetter.DisableColor();
        }
    }
}
