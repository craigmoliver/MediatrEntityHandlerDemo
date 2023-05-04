using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrEntityHandlerDemo.Domain.ContextConfiguration
{
    internal class EmployeeTerritoryConfiguration : BaseEntityConfiguration<EmployeeTerritory>
    {
        public override void Configure(EntityTypeBuilder<EmployeeTerritory> builder)
        {
            builder.HasKey(j => new {j.EmployeeID, j.TerritoryID});

            base.Configure(builder);
        }
    }
}