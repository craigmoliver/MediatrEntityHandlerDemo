using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using MediatrEntityHandlerDemo.Handlers.EntityHandlers;
using Xunit;

namespace MediatrEntityHandlerDemo.Handlers.Test.EntityHandlers
{
    [Collection("EntityHandlers")]
    public class EntityHandlerGetTests : IClassFixture<HandlerTestFixture>
    {
        private readonly HandlerTestFixture _fixture;

        public EntityHandlerGetTests(HandlerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EntityHandlerGet_HappyPathAsync()
        {
            // Arrange
            var context = _fixture.Context;

            var handler = new EntityHandlerDtoGet<EmployeeDto, Employee>.Handler(context, _fixture.Mapper);
            var message = new EntityHandlerDtoGet<EmployeeDto, Employee>.Message("abc123", i => i.EmployeeID == 3);

            // Act
            var dto = await handler.Handle(message, CancellationToken.None);

            // Assert
            dto.Should().NotBeNull();

            dto.EmployeeID.Should().Be(3);
        }

        [Fact]
        public async Task EntityHandlerGet_NotFoundAsync()
        {
            // Arrange
            var handler = new EntityHandlerDtoGet<EmployeeDto, Employee>.Handler(_fixture.Context, _fixture.Mapper);
            var message = new EntityHandlerDtoGet<EmployeeDto, Employee>.Message("abc123", x => x.EmployeeID == -4);

            // Act
            var dto = await handler.Handle(message, CancellationToken.None);

            // Assert
            dto.Should().BeNull();
        }
    }
}
