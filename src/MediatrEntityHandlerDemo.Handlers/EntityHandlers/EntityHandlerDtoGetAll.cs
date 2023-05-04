using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MediatrEntityHandlerDemo.Domain;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    /// <summary>
    /// Gets a <see cref="List{TDto}"/> based on the Where clause of <see cref="Expression{Func{TEntity, bool}}"/>
    /// or all of <see cref="TEntity" />.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityHandlerDtoGetAll<TDto, TEntity>
        where TDto : class, IEntityDto
        where TEntity : BaseEntity
    {
        public class Message : BaseMessage, IRequest<EntityList<TDto>>
        {
            public readonly Expression<Func<TEntity, bool>> WhereExpression = null;
            public readonly Expression<Func<TEntity, TDto>> SelectExpression = null;

            public Message()
            {
            }

            public Message(string correlationId) : base(correlationId)
            {
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> whereExpression) : base(correlationId)
            {
                WhereExpression = whereExpression;
            }

            public Message(string correlationId, Expression<Func<TEntity, TDto>> selectExpression) : base(correlationId)
            {
                SelectExpression = selectExpression;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TDto>> selectExpression) : base(correlationId)
            {
                WhereExpression = whereExpression;
                SelectExpression = selectExpression;
            }
        }
        public class Handler : IRequestHandler<Message, EntityList<TDto>>
        {
            private readonly DatabaseContext _context;
            private readonly IMapper _mapper;

            public Handler()
            {

            }

            public Handler(DatabaseContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<EntityList<TDto>> Handle(Message request, CancellationToken cancellationToken)
            {
                var query = _context.Set<TEntity>().AsQueryable();
                if (request.WhereExpression != null)
                {
                    query = query.Where(request.WhereExpression);
                }

                if (query == null)
                {
                    return null;
                }

                if (request.SelectExpression != null)
                {
                    var dtos = query.Select(request.SelectExpression);

                    return new EntityList<TDto>
                    {
                        List = dtos?.ToList()
                    };
                }


                var entities = await query.ToListAsync(cancellationToken);
                if (entities == null)
                {
                    return null;
                }

                var entityList = new EntityList<TDto>
                {
                    List = _mapper.Map<List<TEntity>, List<TDto>>(entities)
                };

                return entityList;
            }
        }
    }
}
