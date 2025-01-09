namespace MailManager.Application.UseCases.Base;

public abstract class HandlerBase<TCommand, Thandler> where TCommand : CommandBase<TCommand, Thandler>
{
    public abstract Task<Result> Execute(TCommand? command, CancellationToken cancellationToken = default);
}
