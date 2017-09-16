namespace Engine.Contracts
{
    public interface IPercentProvider
    {
        double GetPercent();

        void Start();

        bool IsOver();
    }
}
