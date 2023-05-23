using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ANY.Identity.Infrastructure.Services;

public class CheckAuthorizationService  : DefaultAuthorizationService, IAuthorizationService
{
    private readonly AuthorizationOptions _options;
    private readonly IAuthorizationHandlerContextFactory _contextFactory;
    private readonly IAuthorizationHandlerProvider _handlers;
    private readonly IAuthorizationEvaluator _evaluator;
    private readonly IAuthorizationPolicyProvider _policyProvider;
    private readonly ILogger _logger;

    public CheckAuthorizationService(IAuthorizationPolicyProvider policyProvider
        , IAuthorizationHandlerProvider handlers
        , ILogger<DefaultAuthorizationService> logger
        , IAuthorizationHandlerContextFactory contextFactory
        , IAuthorizationEvaluator evaluator
        , IOptions<AuthorizationOptions> options)
        : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
    {
        _options = options.Value;
        _handlers = handlers;
        _policyProvider = policyProvider;
        _logger = logger;
        _evaluator = evaluator;
        _contextFactory = contextFactory;
    }

    public new async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        if (requirements == null)
        {
            throw new ArgumentNullException(nameof(requirements));
        }

        var authContext = _contextFactory.CreateContext(requirements, user, resource);
        var handlers = await _handlers.GetHandlersAsync(authContext);
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(authContext);
            if (!_options.InvokeHandlersAfterFailure && authContext.HasFailed)
            {
                break;
            }
        }

        var result = _evaluator.Evaluate(authContext);
        if (result.Succeeded)
        {
            _logger.LogInformation($"Authorization is succeeded for {JsonConvert.SerializeObject(requirements)}");
            //_logger.UserAuthorizationSucceeded();
        }
        else
        {
            //var r = result.Failure.FailedRequirements.Select(requirement => new { Requirement = requirement.GetType() });
            var json = JsonConvert.SerializeObject(result.Failure.FailedRequirements);
            _logger.LogInformation($"Authorization is failed for {json}");
            //_logger.UserAuthorizationFailed();
        }
        return result;
    }

}