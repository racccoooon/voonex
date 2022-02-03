using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace voonex.eventhub
{
    public class EventHub : IEventHub
    {
        private readonly Subject<EventBase> _subject = new();

        public IDisposable Listen<TEvent>(IObserver<TEvent> handler) where TEvent : EventBase =>
            _subject.OfType<TEvent>().Subscribe(handler);
        
        public IDisposable Listen<TEvent>(Action<TEvent> handler) where TEvent : EventBase =>
            _subject.OfType<TEvent>().Subscribe(handler);

        public void Send(EventBase @event) => _subject.OnNext(@event);
    }
}