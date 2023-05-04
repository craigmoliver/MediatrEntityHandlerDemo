using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public interface IComparableEntityDto : IEntityDto
    {
        /// <summary>
        /// Allows for an equality comparison between a dto and an entity.
        /// Used when a dto contains fewer properties than the entity and need an equality check on synced properties
        /// </summary>
        /// <param name="entity">The entity being compared to</param>
        /// <returns>If the dto and entity have the same values for matching properties</returns>
        public bool CompareToEntity(BaseEntity entity);
    }
}
