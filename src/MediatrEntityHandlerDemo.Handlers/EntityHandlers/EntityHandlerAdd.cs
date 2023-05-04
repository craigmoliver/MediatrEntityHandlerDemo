using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatrEntityHandlerDemo.Domain;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    /// <summary>
    /// Adds <see cref="TEntity"/> based on the values in
    /// <see cref="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityHandlerAdd<TDto, TEntity>
        where TDto : class
        where TEntity : class
    {
        public class Message : BaseMessage, IRequest<bool>
        {
            public TDto Dto { get; set; }

            public Message()
            {
                Dto = null;
            }

            public Message(string correlationId, TDto dto) : base(correlationId)
            {
                Dto = dto;
            }
        }

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
            /// Add Entity Handler for Message with <see cref="TDto"/> with Transaction support; set Transaction in <see cref="Message"/>.
            /// </summary>
            /// <param name="request">Message to handle.</param>
            /// <param name="cancellationToken">Aysnc cancellation token.</param>
            /// <returns><see cref="bool"/> - true if successful, false if not.</returns>
            public async Task<bool> Handle(Message request, CancellationToken cancellationToken)
            {
                var dto = request.Dto;
                if (dto == null)
                {
                    return false;
                }

                var entity = _mapper.Map<TDto, TEntity>(dto);
                await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
