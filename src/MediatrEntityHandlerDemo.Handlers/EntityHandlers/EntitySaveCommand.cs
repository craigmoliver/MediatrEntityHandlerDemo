using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MediatrEntityHandlerDemo.Domain;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public class EntitySaveCommand : IEntitySaveCommand
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly DatabaseContext _databaseContext;

        public EntitySaveCommand(
            IMapper mapper,
            IMediator mediator,
            DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _databaseContext = databaseContext;
        }

        /// <inheritdoc />
        public async Task<sbyte> Save<TEntity, TDto>(string correlationId, TDto dto)
            where TEntity : BaseEntity, new()
            where TDto : class, IEntityDto
        {
            // Map DTO over blank Entity to get key parts
            var newEntity = new TEntity();
            newEntity = _mapper.Map(dto, newEntity);
            var keyParts = _databaseContext.GetKeyParts(newEntity);
            var existingEntity = await _databaseContext.Set<TEntity>().FindAsync(keyParts);

            if (existingEntity == null)
            {
                var addResult = await _mediator.Send(new EntityHandlerAdd<TDto, TEntity>.Message(correlationId, dto))
                    .ConfigureAwait(false);
                return (sbyte)(addResult ? 1 : -1);
            }

            // See CompareToEntity
            if (dto is IComparableEntityDto compareDto && compareDto.CompareToEntity(existingEntity))
            {
                // Entity not modified in database
                return 0;
            }

            var updateResult = await _mediator.Send(new EntityHandlerUpdate<TDto, TEntity>.Message(correlationId, dto))
                .ConfigureAwait(false);
            return (sbyte)(updateResult ? 2 : -1);

        }
    }
}