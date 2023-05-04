using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MediatrEntityHandlerDemo.Domain;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    /// <summary>
    /// Updates <see cref="TEntity"/> based on the values in
    /// <see cref="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto">DTO.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public class EntityHandlerUpdate<TDto, TEntity>
        where TDto : class
        where TEntity : class, new()
    {
        public class Message : BaseMessage, IRequest<bool>
        {
            public readonly TDto Dto;

            public Message()
            {
                Dto = null;
            }

            public Message(string correlationId, TDto dto) : base(correlationId)
            {
                Dto = dto;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Handler : IRequestHandler<Message, bool>
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

            /// <summary>
            /// Update Entity Handler for Message with <see cref="TDto"/> with Transaction support; set Transaction in <see cref="Message"/>.
            /// </summary>
            /// <param name="request">Message to handle.</param>
            /// <param name="cancellationToken">Aysnc cancellation token.</param>
            /// <returns><see cref="bool"/> - true if successful, false if not.</returns>
            public async Task<bool> Handle(Message request, CancellationToken cancellationToken)
            {
                // Check if DTO is null and punch out if so.
                var dto = request.Dto;
                if (dto == null)
                {
                    return false;
                }

                // Map DTO over blank Entity.
                var e = new TEntity();
                e = _mapper.Map(dto, e);
                var keyParts = _context.GetKeyParts(e);

                // Get tracked Entity from Context using reflected primary key parts.
                e = await _context.Set<TEntity>().FindAsync(keyParts, cancellationToken);
                if (e == null)
                {
                    return false;
                }

                // Update tracked Entity with new values.
                e = _mapper.Map(dto, e);
                _context.Set<TEntity>().Update(e);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
