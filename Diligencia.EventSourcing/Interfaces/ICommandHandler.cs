namespace Diligencia.EventSourcing
{
    public interface ICommandHandler<T> where T : Command
    {
        void Handle(T command);
    }
}
