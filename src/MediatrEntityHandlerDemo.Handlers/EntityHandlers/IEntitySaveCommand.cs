using System.Threading.Tasks;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public interface IEntitySaveCommand
    {
        /// <returns>
        /// 1 => Added,
        /// 2 => Updated,
        /// 0 => Not Modified
        /// -1 => Error
        /// </returns>
        Task<sbyte> Save<TEntity, TDto>(string correlationId, TDto dto)
            where TEntity : BaseEntity, new()
            where TDto : class, IEntityDto;
    }
}
