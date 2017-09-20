using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class IndexFactorByViewDirectionProvider : IIndexFactorProvider
    {
        private IPlayerViewDirectionProvider _playerViewDirectionProvider;

        public IndexFactorByViewDirectionProvider(IPlayerViewDirectionProvider playerViewDirectionProvider)
        {
            _playerViewDirectionProvider = playerViewDirectionProvider;
        }

        int IIndexFactorProvider.GetFactor()
        {
            var direction = _playerViewDirectionProvider.GetViewDirection();

            if (direction.DegreeXZ < 22.5)
                return 6;
            if (direction.DegreeXZ < 45 + 22.5)
                return 7;
            if (direction.DegreeXZ < 90 + 22.5)
                return 0;
            if (direction.DegreeXZ < 135 + 22.5)
                return 1;
            if (direction.DegreeXZ < 180 + 22.5)
                return 2;
            if (direction.DegreeXZ < 225 + 22.5)
                return 3;
            if (direction.DegreeXZ < 270 + 22.5)
                return 4;
            if (direction.DegreeXZ < 315 + 22.5)
                return 5;

            return 6;
        }
    }
}
