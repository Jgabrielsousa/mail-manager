using Microsoft.Extensions.Logging;

namespace MailManager.Application.UseCases.Base;

public class UseCaseHandler<TCommand, Thandler> : IUseCaseHandler<TCommand, Thandler>
where TCommand : CommandBase<TCommand, Thandler>
where Thandler : HandlerBase<TCommand, Thandler>
{
    public Result Result { get; set; }
    public string _commandName;
    public string _handlerName;
    public Thandler _handler;
    private readonly ILogger<TCommand> _logger;

    public UseCaseHandler(Thandler handler,ILogger<TCommand> logger)
    {
        _handler = handler;
        _logger= logger;
    }

    public async Task<Result> Handle(TCommand? command, CancellationToken cancellationToken = default)
    {

        _commandName = command != null ? command.GetType().Name : "CommandUndefined";
        _handlerName = _handler.GetType().Name;

        try
        {
            _logger.LogInformation("{Command} | {Handler} | Start ", _commandName, _handlerName);
            return await _handler.Execute(command, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Command} | {Handler} | Exception ", _commandName, _handlerName);
            return new Result(null!, [e.Message]);
        }
        finally
        {
            _logger.LogInformation("{Command} | {Handler} | Finish ", _commandName, _handlerName);
        }
    }


}
