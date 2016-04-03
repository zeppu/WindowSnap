namespace Overlay.Core
{
    public interface IMessageHandler<in TMessage>
        where TMessage : class
    {
        void HandleMessage(TMessage message);
    }
}