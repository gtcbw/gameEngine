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
            _colorSetter.SetColor(1f, 1.0f, 1.0f);
            _innerElement.Render();
            _colorSetter.DisableColor();
        }
    }
}
