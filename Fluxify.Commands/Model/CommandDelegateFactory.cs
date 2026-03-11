using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.Model;

[RequiresDynamicCode("Generates IL at runtime. Try to use CommandDelegate directly when runtime code generation is not possible")]
public static class CommandDelegateFactory
{
    private static readonly PropertyInfo ContextCommandTokenizerPropertyInfo = typeof(CommandContext).GetProperty(nameof(CommandContext.Tokenizer))!;
    private static readonly PropertyInfo ContextServicesPropertyInfo = typeof(CommandContext).GetProperty(nameof(CommandContext.Services))!;
    private static readonly MethodInfo? GetServiceMethodInfo = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService));

    public static CommandDelegate Create(Delegate handler)
    {
        if (handler is CommandDelegate commandDelegate)
        {
            return commandDelegate;
        }

        var ctxParam = Expression.Parameter(typeof(CommandContext), "ctx");
        //var handlerParam = Expression.Parameter(typeof(Delegate), "handler");
        var parameters = handler.Method.GetParameters()
            .Select(p => ProvideParameter(ctxParam, p))
            .OfType<Expression>();
        
        if (handler.Method.ReturnType != typeof(Task))
        {
            if (handler.Method.ReturnType != typeof(void))
            {
                throw new InvalidOperationException("Only Task and void supported");
            }

            var taskRun = typeof(Task).GetMethod(nameof(Task.Run), [typeof(Action)])!;
            
            return Expression.Lambda<CommandDelegate>(
                Expression.Call(taskRun, Expression.Lambda(
                    Expression.Invoke(Expression.Constant(handler), parameters)
                )), ctxParam).Compile();
        } 

        return Expression.Lambda<CommandDelegate>(
            Expression.Invoke(Expression.Constant(handler), parameters),
            [ctxParam]
        ).Compile();
    }

    static FrozenDictionary<Type, Func<ParameterExpression, Expression>> ParameterProviders { get; } =
        new Dictionary<Type, Func<ParameterExpression, Expression>>()
        {
            { typeof(CommandContext), ctx => ctx },
            { typeof(CommandTokenizer), ctx => Expression.MakeMemberAccess(ctx, ContextCommandTokenizerPropertyInfo) },
            { typeof(IServiceProvider), ctx => Expression.MakeMemberAccess(ctx, ContextServicesPropertyInfo) },
        }.ToFrozenDictionary();

    private static Expression? ProvideParameter(ParameterExpression ctxParam, ParameterInfo info)
    {
        return ParameterProviders.TryGetValue(info.ParameterType, out var provider)
            ? provider(ctxParam)
            : ProvideParameterDi(ctxParam, info);
    }

    private static Expression ProvideParameterDi(
        ParameterExpression ctxParam,
        ParameterInfo info)
    {
        var services = Expression.MakeMemberAccess(ctxParam, ContextServicesPropertyInfo);

        return Expression.Convert(Expression.Call(services, GetServiceMethodInfo!, Expression.Constant(info.ParameterType)), info.ParameterType);
    }
}