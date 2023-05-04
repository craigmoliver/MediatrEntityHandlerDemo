using System.Collections.Generic;

namespace MediatrEntityHandlerDemo.Handlers.EntityHandlers
{
    public interface IEntityList<TDto>
        where TDto : class
    {
        List<TDto> List { get; set; }
    }
}
