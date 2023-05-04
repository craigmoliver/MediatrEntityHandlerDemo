using AutoMapper;
using MediatrEntityHandlerDemo.Domain;
using MediatrEntityHandlerDemo.Domain.Data;
using MediatrEntityHandlerDemo.Domain.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MediatrEntityHandlerDemo.Handlers.Test
{
    public class HandlerTestFixture : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerTestFixture"/> class.
        /// </summary>
        public HandlerTestFixture()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            Mapper = mapperConfiguration.CreateMapper();
            Context = new DatabaseContext(DatabaseContext.GetDbOptionsBuilderForTests().Options);

            //var dataConfiguration = new DataConfiguration();
            //dataConfiguration.Seed(Context);
        }


        /// <summary>
        /// Gets or Sets a DbContext for the fixture.
        /// </summary>
        public DatabaseContext Context { get; private set; }

        /// <summary>
        /// Gets or Sets Mapper for your mapping pleasure.
        /// </summary>
        public IMapper Mapper { get; init; }

        /// <inheritdoc />
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
