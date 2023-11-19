namespace ToonBlastClone.Interface
{
    public interface EventListener<T> : IEventListener 
    {
        void OnEventTrigger(T currentEvent);
    }
}
