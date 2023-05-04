using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrEntityHandlerDemo.Domain.ContextConfiguration
{
    internal class OrderDetailConfiguration : BaseEntityConfiguration<OrderDetail>
    {
        public override void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(j => new {j.OrderID, j.ProductID});

            base.Configure(builder);
        }
    }
}