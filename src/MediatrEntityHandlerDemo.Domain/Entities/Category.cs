using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MediatrEntityHandlerDemo.Domain.Entities
{
    /// <summary>
    /// The category.
    /// </summary>
    [Table("Categories")]
    public partial class Category : BaseEntity
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        /// <summary>
        /// Gets the picture display.
        /// </summary>
        [NotMapped]
        public byte[] PictureDisplay
        {
            get
            {
                return Picture.Skip(78).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}
