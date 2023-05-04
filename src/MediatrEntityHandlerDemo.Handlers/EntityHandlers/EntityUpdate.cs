using MediatR;
using MediatrEntityHandlerDemo.Handlers;
using System.Threading;
using System.Threading.Tasks;
using MediatrEntityHandlerDemo.Domain;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public class EntityUpdate<TEntity>
        where TEntity : class, new()
    {
        public class Message : BaseMessage, IRequest<int>
        {
            public readonly TEntity Entity;
            public readonly bool IsTracked;

            public Message(string correlationId, TEntity entity) : base(correlationId)
            {
                Entity = entity;
            }
            public Message(string correlationId, TEntity entity, bool isTracked) : base(correlationId)
            {
                Entity = entity;
                IsTracked = isTracked;
            }
        }

        public class Handler : IRequestHandler<Message, int>
        {
            private readonly DatabaseContext _context;

            public Handler(DatabaseContext context)
            {
                _context = context;

            }

            public async Task<int> Handle(Message request, CancellationToken cancellationToken)
            {

                var entity = request.Entity;
                var keyParts = _context.GetKeyParts(entity);

                if (request.IsTracked)
                {
                    _context.Set<TEntity>().Update(request.Entity);
                }
                else
                {
                    // Get tracked Entity from Context using reflected primary key parts.
                    var trackedEntity = await _context.Set<TEntity>().FindAsync(keyParts, cancellationToken);

                    if (trackedEntity == null)
                    {
                        return 0;
                    }

                    // _context.Set<TEntity>().Update(request.Entity);
                    _context.Entry(trackedEntity).CurrentValues.SetValues(entity);

                }

                var updateCount = await _context.SaveChangesAsync(cancellationToken);
                return updateCount;
            }
        }

    }
}
