using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatrEntityHandlerDemo.Domain;
using MediatrEntityHandlerDemo.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public class EntityHandlerEntityGet<TEntity>
        where TEntity : class
    {
        public class Message : BaseMessage, IRequest<TEntity>
        {
            public readonly object[] EntityIds;
            public readonly Expression<Func<TEntity, bool>> Query;
            public readonly Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeProperties;
            public readonly bool AsNoTracking;


            public Message()
            {
                EntityIds = null;
            }

            public Message(string correlationId, object entityId) : base(correlationId)
            {
                EntityIds = new object[] { entityId };
            }

            public Message(string correlationId, object[] entityIds) : base(correlationId)
            {
                EntityIds = entityIds;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query) : base(correlationId)
            {
                Query = query;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query, bool asNoTracking) : base(
                correlationId)
            {
                Query = query;
                AsNoTracking = asNoTracking;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties) : base(
                correlationId)
            {
                Query = query;
                IncludeProperties = includeProperties;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties,
                bool asNoTracking) : base(
                correlationId)
            {
                Query = query;
                AsNoTracking = asNoTracking;
                IncludeProperties = includeProperties;
            }
        }

        public class Handler : IRequestHandler<Message, TEntity>
        {
            private readonly DatabaseContext _context;

            public Handler()
            {
            }

            public Handler(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<TEntity> Handle(Message request, CancellationToken cancellationToken)
            {
                if (request.Query != null)
                {
                    var query = _context.Set<TEntity>().AsQueryable();
                    if (request.AsNoTracking)
                    {
                        query = query.AsNoTracking();
                    }

                    query = query.Where(request.Query);

                    if (request.IncludeProperties != null)
                    {
                        query = request.IncludeProperties(query);
                    }

                    return query
                        .FirstOrDefault();
                }

                if (request.EntityIds == null) return null;
                var e = await _context.Set<TEntity>().FindAsync(request.EntityIds, cancellationToken);
                return e;
            }
        }
    }
}