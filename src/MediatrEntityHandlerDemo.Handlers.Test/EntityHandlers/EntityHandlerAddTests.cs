using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;
using MediatrEntityHandlerDemo.Handlers.EntityHandlers;
using Xunit;

namespace MediatrEntityHandlerDemo.Handlers.Test.EntityHandlers
{
    [Collection("EntityHandlers")]
    public class EntityHandlerAddTests : IClassFixture<HandlerTestFixture>
    {
        private readonly HandlerTestFixture _fixture;

        public EntityHandlerAddTests(HandlerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EntityHandlerAdd_HappyPathAsync()
        {
            // Arrange
            
            var context = _fixture.Context;
            var customer = new Customer();

            var dto = _fixture.Mapper.Map<Customer, CustomerDto>(customer);
            var handler = new EntityHandlerAdd<CustomerDto, Customer>.Handler(context, _fixture.Mapper);
            var message = new EntityHandlerAdd<CustomerDto, Customer>.Message("abc123", dto);

            // Act
            var result = await handler.Handle(message, CancellationToken.None);
            var saved = await context.Customers.FindAsync(customer.CustomerID);

            // Assert
            result.Should().BeTrue();
            saved.Should().NotBeNull();
        }

        [Fact]
        public async Task EntityHandlerAdd_NullAsync()
        {
            // Arrange
            var context = _fixture.Context;
            var dto = _fixture.Mapper.Map<Order, OrderDto>(null);

            var handler = new EntityHandlerAdd<OrderDto, Order>.Handler(context, _fixture.Mapper);
            var message = new EntityHandlerAdd<OrderDto, Order>.Message("abc123", dto);

            // Act
            var result = await handler.Handle(message, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
