using Engine.Contracts;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class AlphaTestRenderer : IRenderingElement
    {
        private IRenderingElement _innerElement;
        private IAlphaTester _alphaTester;

        public AlphaTestRenderer(IRenderingElement innerElement,
            IAlphaTester alphaTester)
        {
            _innerElement = innerElement;
            _alphaTester = alphaTester;
        }

        void IRenderingElement.Render()
        {
            _alphaTester.Begin();
            _innerElement.Render();
            _alphaTester.End();
        }
    }
}
