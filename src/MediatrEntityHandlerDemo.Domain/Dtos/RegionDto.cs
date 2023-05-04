using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class RegionDto : IEntityDto
    {
        public int RegionID { get; set; }

        public string RegionDescription { get; set; }

        public ICollection<TerritoryDto> Territories { get; set; }
    }
}
