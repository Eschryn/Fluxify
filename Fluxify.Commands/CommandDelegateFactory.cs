using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluxify.Commands;

[RequiresDynamicCode("Generates IL at runtime. Try to use CommandDelegate directly when runtime code generation is not possible")]
public static class CommandDelegateFactory
{
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
            { typeof(IServiceProvider), ctx => Expression.MakeMemberAccess(ctx, typeof(CommandContext).GetProperty(nameof(CommandContext.Services))!) },
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
        var services = Expression.MakeMemberAccess(ctxParam, typeof(CommandContext).GetProperty(nameof(CommandContext.Services))!);
        var func = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService));
        
        return Expression.Convert(Expression.Call(services, func!, Expression.Constant(info.ParameterType)), info.ParameterType);
    }
}