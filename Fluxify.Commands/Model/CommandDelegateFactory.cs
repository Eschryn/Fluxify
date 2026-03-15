// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Fluxify.Commands.TextCommand;

namespace Fluxify.Commands.Model;

public static class CommandDelegateFactory
{
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