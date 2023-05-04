using System.Collections.Generic;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public class EntityList<TDto> : IEntityList<TDto>
        where TDto : class
    {
        public List<TDto> List { get; set; }

    }
}
