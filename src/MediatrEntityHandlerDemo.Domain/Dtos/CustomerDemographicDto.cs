using System.ComponentModel.DataAnnotations;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class CustomerDemographicDto : IEntityDto
    {
        [Key]
        public string CustomerID { get; set; }
        [Key]
        public string DemographicID { get; set; }

        public CustomerDto CustomerDto { get; set; }

        public DemographicDto DemographicDto { get; set; }
    }
}
