using System;
using System.Collections.Generic;
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
    public class EntityHandlerEntityGetAll<TEntity>
        where TEntity : class
    {
        public class Message : BaseMessage, IRequest<List<TEntity>>
        {
            public readonly Expression<Func<TEntity, bool>> Query;
            public readonly Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> IncludeProperties;
            public readonly bool AsNoTracking;
            // https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
            public readonly bool AsSplitQuery;

            public Message()
            {
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

            public Message(string correlationId, Expression<Func<TEntity, bool>> query, bool asNoTracking,
                bool asSplitQuery) : base(
                correlationId)
            {
                Query = query;
                AsNoTracking = asNoTracking;
                AsSplitQuery = asSplitQuery;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties) : base(
                correlationId)
            {
                Query = query;
                IncludeProperties = includeProperties;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties, bool asNoTracking) :
                base(correlationId)
            {
                Query = query;
                IncludeProperties = includeProperties;
                AsNoTracking = asNoTracking;
            }

            public Message(string correlationId, Expression<Func<TEntity, bool>> query,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties, bool asNoTracking,
                bool asSplitQuery) :
                base(correlationId)
            {
                Query = query;
                IncludeProperties = includeProperties;
                AsNoTracking = asNoTracking;
                AsSplitQuery = asSplitQuery;
            }
        }

        public class Handler : IRequestHandler<Message, List<TEntity>>
        {
            private readonly DatabaseContext _context;

            public Handler()
            {
            }

            public Handler(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<List<TEntity>> Handle(Message request, CancellationToken cancellationToken)
            {
                if (request.Query == null) return null;

                var query = _context.Set<TEntity>().AsQueryable();
                if (request.AsNoTracking)
                {
                    query = query.AsNoTracking();
                }

                query = query.Where(request.Query);

                if (request.IncludeProperties != null)
                {
                    query = request.IncludeProperties(query);
                    if (request.AsSplitQuery)
                    {
                        query = query.AsSplitQuery();
                    }
                }

                var entities = await query.ToListAsync(cancellationToken);

                return entities;
            }
        }
    }
}