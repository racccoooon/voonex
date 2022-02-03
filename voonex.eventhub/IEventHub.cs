namespace voonex.eventhub
{
    public interface IEventHub
    {
        IDisposable Listen<TEvent>(IObserver<TEvent> handler) where TEvent : EventBase;
        IDisposable Listen<TEvent>(Action<TEvent> handler) where TEvent : EventBase;
        void Send(EventBase @event);
    }
}