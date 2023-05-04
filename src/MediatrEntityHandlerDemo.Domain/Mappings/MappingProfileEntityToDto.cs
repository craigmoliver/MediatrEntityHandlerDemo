using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Domain.Mappings
{
    public partial class MappingProfile
    {
        private void EntityToDto()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDemographic, CustomerDemographicDto>();
            CreateMap<Demographic, DemographicDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeTerritory, EmployeeTerritoryDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Region, RegionDto>();
            CreateMap<Shipper, ShipperDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Territory, TerritoryDto>();
        }
    }
}