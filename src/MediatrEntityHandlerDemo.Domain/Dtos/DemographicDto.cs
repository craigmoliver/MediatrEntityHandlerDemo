using System.Collections.Generic;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public  class DemographicDto : IEntityDto
    {
        public string DemographicID { get; set; }
        public string Description { get; set; }

        public ICollection<CustomerDemographicDto> CustomerDemographics { get; set; }
    }
}
