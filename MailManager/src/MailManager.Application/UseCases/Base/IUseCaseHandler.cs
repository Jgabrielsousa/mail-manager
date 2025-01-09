namespace MailManager.Application.UseCases.Base;

public interface IUseCaseHandler<TCommand, Thandler>
    where TCommand : CommandBase<TCommand, Thandler>
{
    Task<Result> Handle(TCommand? command, CancellationToken cancellationToken = default);
}