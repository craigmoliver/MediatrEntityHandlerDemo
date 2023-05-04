using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Handlers;

public static class HandlerDependencyInjectionHelper
{
    private static readonly List<Type> ExcludedAbstractClasses = new()
    {
        typeof(BaseEntity)
    };

    public static Func<string, List<Type>> GetAllEntityTypes = assemblyName => Assembly.Load(assemblyName)
        .GetTypes()
        .Where(t => !ExcludedAbstractClasses.Contains(t))
        .Where(t => typeof(BaseEntity).IsAssignableFrom(t)).ToList();

    public static void ServiceTypeIRequestEntity(Type entityType, Type typeOfMessageResponse,
        Type typeOfHandlerMessage, Type typeOfHandlerHandle,
        out Type messageInterfaceType, out Type messageType, out Type handlerInterfaceType,
        out Type handlerType)
    {
        messageInterfaceType = typeof(MediatR.IRequest<>).MakeGenericType(typeOfMessageResponse);
        messageType = typeOfHandlerMessage.MakeGenericType(entityType);
        handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(messageType, typeOfMessageResponse);
        handlerType = typeOfHandlerHandle.MakeGenericType(entityType);
    }
}