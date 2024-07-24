using Application.Services.AuthenticatorService;
using Application.Services.AuthService;
using Application.Services.UserService;
using Core.Application.Pipelines.Authorizations;
using Core.Application.Pipelines.Cachings;
using Core.Application.Pipelines.Loggings;
using Core.Application.Pipelines.Transactions;
using Core.Application.Pipelines.Validations;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Serilogs;
using Core.CrossCuttingConcerns.Serilogs.Loggers;
using Core.Mailing;
using Core.Mailing.MailKitImplementations;
using Core.Security;
using Core.Security.JWT;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class ApplicationServiceRegistiration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, TokenOptions tokenOptions)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSecurityServices<int, int, int>(tokenOptions);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IAuthenticatorService, AuthenticatorManager>();

        services.AddSingleton<LoggerServiceBase, FileLogger>();
        //services.AddSingleton<LoggerServiceBase, MsSqlLogger>();
        services.AddSingleton<IMailService, MailKitMailService>();
        return services;
    }

    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(type) && !type.Equals(t)).ToList();

        foreach (var item in types)
            if (addWithLifeCycle is null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);

        return services;
    }
}