using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class EmployeeTerritoryDto : IEntityDto
    {
        public int EmployeeID { get; set; }

        public string TerritoryID { get; set; }

        public EmployeeDto Employee { get; set; }

        public TerritoryDto Territory { get; set; }
    }
}
