
namespace ToonBlastClone
{
    public interface ICollectable
    {
        bool IsGoal { get; }

        public void Collect();
    }
}
