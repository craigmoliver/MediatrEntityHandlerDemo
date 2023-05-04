using AutoMapper;

namespace MediatrEntityHandlerDemo.Domain.Mappings
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            DtoToEntity();
            EntityToDto();
        }
    }
}
