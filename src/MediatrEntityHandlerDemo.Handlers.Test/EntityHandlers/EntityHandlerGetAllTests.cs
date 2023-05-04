using FluentAssertions;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using MediatrEntityHandlerDemo.Handlers.EntityHandlers;

namespace MediatrEntityHandlerDemo.Handlers.Test.EntityHandlers
{
    [Collection("EntityHandlers")]
    public class EntityHandlerGetAllTests : IClassFixture<HandlerTestFixture>
    {
        private readonly HandlerTestFixture _fixture;

        public EntityHandlerGetAllTests(HandlerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EntityHandlerGetAll_HappyPathAsync()
        {
            // Arrange
            var context = _fixture.Context;


            var handler = new EntityHandlerDtoGetAll<CustomerDto, Customer>.Handler(context, _fixture.Mapper);
            var message = new EntityHandlerDtoGetAll<CustomerDto, Customer>.Message("abc123");

            // Act
            var dtos = await handler.Handle(message, CancellationToken.None);

            // Assert
            dtos.Should().NotBeNull();
            dtos.List.Count.Should().Be(2);
        }

        [Fact]
        public async Task EntityHandlerGetAll_Where_HappyPathAsync()
        {
            // ARRANGE
            var context = _fixture.Context;
            await context.SaveChangesAsync();
            var handler = new EntityHandlerDtoGetAll<CustomerDto, Customer>.Handler(_fixture.Context, _fixture.Mapper);
            var message = new EntityHandlerDtoGetAll<CustomerDto, Customer>.Message("abc123", e => e.City == "Mexico");

            // ACT
            var dtos = await handler.Handle(message, CancellationToken.None);

            // ASSERT
            dtos.Should().NotBeNull();
            dtos.List.Count.Should().Be(1);
        }
    }
}
