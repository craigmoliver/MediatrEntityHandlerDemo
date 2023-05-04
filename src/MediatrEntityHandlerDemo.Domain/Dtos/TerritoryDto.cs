using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class TerritoryDto : IEntityDto
    {
        public string TerritoryID { get; set; }

        public string TerritoryDescription { get; set; }

        public int RegionID { get; set; }

        public RegionDto Region { get; set; }

        public ICollection<EmployeeTerritoryDto> EmployeeTerritories { get; set; }
    }
}
