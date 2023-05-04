using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MediatrEntityHandlerDemo.Domain;
using MediatrEntityHandlerDemo.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    /// <summary>
    /// Gets <see cref="TDto" /> based on the primary key(s) of
    /// <see cref="TEntity" />.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityHandlerDtoGet<TDto, TEntity>
        where TDto : class
        where TEntity : class
    {
        public class Message : BaseMessage, IRequest<TDto>
        {
            public readonly Expression<Func<TEntity, bool>> Where;
            public readonly Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeProperties;
            public readonly bool AsNoTracking;

            public Message()
            {
                Where = null;
                IncludeProperties = null;
                AsNoTracking = false;
            }

            public Message(string correlationId, bool asNoTracking, Expression<Func<TEntity, bool>> where)
                : base(correlationId)
            {
                Where = where;
                IncludeProperties = null;
                AsNoTracking = asNoTracking;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> where,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
                : base(correlationId)
            {
                Where = where;
                IncludeProperties = includeProperties;
                AsNoTracking = false;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> where, bool asNoTracking,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
                : base(correlationId)
            {
                Where = where;
                IncludeProperties = includeProperties;
                AsNoTracking = asNoTracking;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> where)
                : base(correlationId)
            {
                Where = where;
                IncludeProperties = null;
                AsNoTracking = false;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> where, bool asNoTracking)
                : base(correlationId)
            {
                Where = where;
                IncludeProperties = null;
                AsNoTracking = asNoTracking;
            }
        }

        public class Handler : IRequestHandler<Message, TDto>
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

            public async Task<TDto> Handle(Message request, CancellationToken cancellationToken)
            {
                var query = _context.Set<TEntity>().Where(request.Where).AsQueryable();

                if (request.IncludeProperties != null)
                {
                    query = request.IncludeProperties(query);
                }

                if (request.AsNoTracking)
                {
                    query = query.AsNoTracking();
                }

                var entity = await query.SingleOrDefaultAsync(cancellationToken);
                if (entity != null)
                {
                    return _mapper.Map<TDto>(entity);
                }

                return null;
            }
        }
    }
}