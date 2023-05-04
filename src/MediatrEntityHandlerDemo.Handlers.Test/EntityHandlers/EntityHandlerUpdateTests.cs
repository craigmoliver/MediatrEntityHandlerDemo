using FluentAssertions;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using MediatrEntityHandlerDemo.Handlers.EntityHandlers;
using Microsoft.EntityFrameworkCore;

namespace MediatrEntityHandlerDemo.Handlers.Test.EntityHandlers
{
    [Collection("EntityHandlers")]
    public class EntityHandlerUpdateTests : IClassFixture<HandlerTestFixture>
    {
        private readonly HandlerTestFixture _fixture;

        public EntityHandlerUpdateTests(HandlerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EntityHandlerUpdate_HappyPathAsync()
        {
            // Arrange
            var context = _fixture.Context;
            

            var original = await context.Customers.FirstOrDefaultAsync(x => x.CustomerID == "ANATR");
            var dto = _fixture.Mapper.Map<Customer, CustomerDto>(original);
            var handler = new EntityHandlerUpdate<CustomerDto, Customer>.Handler(context, _fixture.Mapper);

            // Act
            dto.CompanyName = "Ajax Corp";
            var message = new EntityHandlerUpdate<CustomerDto, Customer>.Message("abc123", dto);
            var result = await handler.Handle(message, CancellationToken.None);
            var updated = await context.Customers.FindAsync("ANATR");

            // Assert
            result.Should().BeTrue();
            original.CompanyName.Should().Be("Ana Trujillo Emparedados y helados");
            updated.CompanyName.Should().Be("Ajax Corp");
        }

        [Fact]
        public async Task EntityHandlerUpdate_NullAsync()
        {
            // Arrange
            var context = _fixture.Context;
            var dto = _fixture.Mapper.Map<Domain.Entities.Customer, CustomerDto>(null);

            var handler = new EntityHandlerUpdate<CustomerDto, Customer>.Handler(context, _fixture.Mapper);
            var message = new EntityHandlerUpdate<CustomerDto, Customer>.Message("abc123", dto);

            // Act
            var result = await handler.Handle(message, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
