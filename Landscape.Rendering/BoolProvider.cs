using Engine.Contracts;

namespace Landscape.Rendering
{
    public sealed class BoolProvider : IBoolProvider
    {
        private bool[] _values;
        int _index;

        public BoolProvider(bool[] values)
        {
            _values = values;
        }

        bool IBoolProvider.GetNext()
        {
            bool value = _values[_index++];

            if (_index == _values.Length)
                _index = 0;

            return value;
        }
    }
}
