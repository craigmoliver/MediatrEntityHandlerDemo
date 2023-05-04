using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MediatrEntityHandlerDemo.Domain
{
    /// <summary>
    /// Helpers to help with DbContext and associated Entities.
    /// </summary>
    public static class ContextHelpers
    {
        /// <summary>
        /// Gets the Primary Key(s) of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="context">EF Context.</param>
        /// <param name="entity">Entity</param>
        /// <returns>Array of primary keys</returns>
        public static object[] GetKeyParts<T>(this DbContext context, T entity)
        {
            var entry = context.Entry(entity);
            var keyPropertiesByEntityType = new ConcurrentDictionary<Type, string[]>();
            var keyProperties = keyPropertiesByEntityType.GetOrAdd(
                typeof(T),
                t => entry.Metadata.FindPrimaryKey().Properties.Select(property => property.Name).ToArray());
            var keyParts = keyProperties
                .Select(propertyName => entry.Property(propertyName).CurrentValue)
                .ToArray();
            return keyParts;
        }
    }
}
