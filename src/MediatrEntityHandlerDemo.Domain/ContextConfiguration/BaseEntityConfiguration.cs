using MediatrEntityHandlerDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediatrEntityHandlerDemo.Domain.ContextConfiguration
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(t => t.DateCreatedUtc)
                .HasDefaultValueSql("GETUTCDATE()");
            builder
                .Property(t => t.DateUpdatedUtc)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}