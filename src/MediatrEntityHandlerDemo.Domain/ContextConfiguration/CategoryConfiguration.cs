using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrEntityHandlerDemo.Domain.ContextConfiguration
{
    internal class CustomerDemographicConfiguration : BaseEntityConfiguration<CustomerDemographic>
    {
        public override void Configure(EntityTypeBuilder<CustomerDemographic> builder)
        {
            builder.HasKey(j => new {j.CustomerID, j.DemographicID});

            base.Configure(builder);
        }
    }
}