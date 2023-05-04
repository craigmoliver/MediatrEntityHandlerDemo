using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using MediatR;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using MediatrEntityHandlerDemo.Handlers.EntityHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace MediatrEntityHandlerDemo.Handlers
{
    /// <summary>
    /// For Dependency Injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Default service configuration for services in this assembly.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public static void ConfigureServicesHandlersDefault(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var mapper = sp.GetService<IMapper>();
            if (mapper == null)
            {
                throw new Exception(
                    $"AutoMapper configuration must be add before calling ServiceCollectionExtensions.ConfigureServicesHandlersDefault()");
            }

            var entityHandlers = GetEntityHandlers(mapper);

            // Add Services

            services.AddScoped(typeof(IEntitySaveCommand), typeof(EntitySaveCommand));
            services.AddScoped(typeof(IBaseMessage), typeof(BaseMessage));
            foreach (var entityHandler in entityHandlers)
            {
                services.AddScoped(entityHandler.Item1, entityHandler.Item2);
            }
        }

        private static List<Tuple<Type, Type>> GetEntityHandlers(IMapper mapper)
        {
            // Gather DTOs that implement IEntityDto
            var dtoTypes = Assembly.Load("MediatrEntityHandlerDemo.Domain.Entities").GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IEntityDto))).ToList();
            var typesToAddScoped = new List<Tuple<Type, Type>>();

            foreach (var dtoType in dtoTypes)
            {
                // Get all AutoMapper maps that map entity to dto
                var entityToDto = mapper.ConfigurationProvider.Internal().GetAllTypeMaps().Where(t => t.DestinationType == dtoType)
                    .ToList();
                foreach (var typeMap in entityToDto)
                {
                    var entityType = typeMap.SourceType;
                    if (!typeof(BaseEntity).IsAssignableFrom(entityType))
                    {
                        continue; // source not map BaseEntity move next
                    }

                    GetEntityHandlersGet(dtoType, entityType, typesToAddScoped);
                    GetEntityHandlersGetAll(dtoType, entityType, typesToAddScoped);
                }

                // Get all AutoMapper maps for this type
                var dtoToEntity = mapper.ConfigurationProvider.Internal().GetAllTypeMaps().Where(t => t.SourceType == dtoType)
                    .ToList();
                if (!dtoToEntity.Any())
                {
                    continue; // no maps found move next
                }

                // For each AutoMapper map type.
                foreach (var typeMap in dtoToEntity)
                {
                    var entityType = typeMap.DestinationType;
                    if (!typeof(BaseEntity).IsAssignableFrom(entityType))
                    {
                        continue; // destination not map BaseEntity move next
                    }

                    GetEntityHandlersUpdate(dtoType, entityType, typesToAddScoped);
                    GetEntityHandlersAdd(dtoType, entityType, typesToAddScoped);
                }
            }

            var entityTypes = HandlerDependencyInjectionHelper.GetAllEntityTypes("MediatrEntityHandlerDemo.Domain.Entities");

            foreach (var entityType in entityTypes)
            {
                AddEntityHandler(entityType, typeof(List<>).MakeGenericType(entityType),
                    typeof(EntityHandlerEntityGetAll<>.Message),
                    typeof(EntityHandlerEntityGetAll<>.Handler), typesToAddScoped);
                AddEntityHandler(entityType, entityType, typeof(EntityHandlerEntityGet<>.Message),
                    typeof(EntityHandlerEntityGet<>.Handler), typesToAddScoped);
                AddEntityHandler(entityType, typeof(int), typeof(EntityUpdate<>.Message),
                    typeof(EntityUpdate<>.Handler),
                    typesToAddScoped);
            }

            return typesToAddScoped;
        }

        private static void GetEntityHandlersAdd(Type dtoType, Type entityType,
            List<Tuple<Type, Type>> typesToAddScoped)
        {
            var messageTypeAdd = typeof(EntityHandlerAdd<,>.Message).MakeGenericType(dtoType, entityType);
            var handlerServiceTypeAdd = typeof(IRequestHandler<,>).MakeGenericType(messageTypeAdd, typeof(bool));
            var handlerTypeAdd = typeof(EntityHandlerAdd<,>.Handler).MakeGenericType(dtoType, entityType);
            typesToAddScoped.Add(
                new Tuple<Type, Type>(typeof(MediatR.IRequest<>).MakeGenericType(typeof(bool)), messageTypeAdd));
            typesToAddScoped.Add(new Tuple<Type, Type>(handlerServiceTypeAdd, handlerTypeAdd));
        }

        private static void GetEntityHandlersUpdate(Type dtoType, Type entityType,
            List<Tuple<Type, Type>> typesToAddScoped)
        {
            var messageTypeUpdate = typeof(EntityHandlerUpdate<,>.Message).MakeGenericType(dtoType, entityType);
            var handlerServiceTypeUpdate = typeof(IRequestHandler<,>).MakeGenericType(messageTypeUpdate, typeof(bool));
            var handlerTypeUpdate = typeof(EntityHandlerUpdate<,>.Handler).MakeGenericType(dtoType, entityType);
            typesToAddScoped.Add(new Tuple<Type, Type>(typeof(MediatR.IRequest<>).MakeGenericType(typeof(bool)),
                messageTypeUpdate));
            typesToAddScoped.Add(new Tuple<Type, Type>(handlerServiceTypeUpdate, handlerTypeUpdate));
        }

        private static void GetEntityHandlersGet(Type dtoType, Type entityType,
            List<Tuple<Type, Type>> typesToAddScoped)
        {
            var serviceTypeIRequestDto = typeof(MediatR.IRequest<>).MakeGenericType(dtoType);
            var messageTypeGet = typeof(EntityHandlerDtoGet<,>.Message).MakeGenericType(dtoType, entityType);
            var handlerServiceTypeGet = typeof(IRequestHandler<,>).MakeGenericType(messageTypeGet, dtoType);
            var handlerTypeGet = typeof(EntityHandlerDtoGet<,>.Handler).MakeGenericType(dtoType, entityType);
            typesToAddScoped.Add(new Tuple<Type, Type>(serviceTypeIRequestDto, messageTypeGet));
            typesToAddScoped.Add(new Tuple<Type, Type>(handlerServiceTypeGet, handlerTypeGet));
        }

        private static void GetEntityHandlersGetAll(Type dtoType, Type entityType,
            List<Tuple<Type, Type>> typesToAddScoped)
        {
            var dtoListType = typeof(EntityList<>).MakeGenericType(dtoType);
            var serviceTypeIRequestDto = typeof(MediatR.IRequest<>).MakeGenericType(dtoListType);
            var messageTypeGetAll = typeof(EntityHandlerDtoGetAll<,>.Message).MakeGenericType(dtoType, entityType);
            var handlerServiceTypeGet = typeof(IRequestHandler<,>).MakeGenericType(messageTypeGetAll, dtoListType);
            var handlerTypeGet = typeof(EntityHandlerDtoGetAll<,>.Handler).MakeGenericType(dtoType, entityType);
            typesToAddScoped.Add(new Tuple<Type, Type>(serviceTypeIRequestDto, messageTypeGetAll));
            typesToAddScoped.Add(new Tuple<Type, Type>(handlerServiceTypeGet, handlerTypeGet));
        }


        private static void AddEntityHandler(Type entityType, Type entityResponseType, Type typeOfHandlerMessage,
            Type typeOfHandlerHandle,
            List<Tuple<Type, Type>> typesToAddScoped)
        {
            HandlerDependencyInjectionHelper.ServiceTypeIRequestEntity(entityType, entityResponseType,
                typeOfHandlerMessage, typeOfHandlerHandle,
                out var messageInterfaceType, out var messageType, out var handlerInterfaceType, out var handlerType);
            //typesToAddScoped.Add(new Tuple<Type, Type>(messageInterfaceType, messageType));
            typesToAddScoped.Add(new Tuple<Type, Type>(handlerInterfaceType, handlerType));
        }
    }
}