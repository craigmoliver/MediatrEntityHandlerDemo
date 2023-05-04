using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrEntityHandlerDemo.Domain.ContextConfiguration
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(j => new {j.CategoryID});

            base.Configure(builder);
        }
    }
}