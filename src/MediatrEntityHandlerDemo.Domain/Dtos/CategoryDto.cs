using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Domain.Dtos
{
    public class CategoryDto : IEntityDto
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<ProductDto> Products { get; set; }
    }
}
