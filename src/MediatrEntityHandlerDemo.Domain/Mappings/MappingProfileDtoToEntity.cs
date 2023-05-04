using MediatrEntityHandlerDemo.Domain.Dtos;
using MediatrEntityHandlerDemo.Domain.Entities;

namespace MediatrEntityHandlerDemo.Domain.Mappings
{
    public partial class MappingProfile
    {
        private void DtoToEntity()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<CustomerDemographicDto, CustomerDemographic>();
            CreateMap<DemographicDto, Demographic>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<EmployeeTerritoryDto, EmployeeTerritory>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<ProductDto, Product>();
            CreateMap<RegionDto, Region>();
            CreateMap<ShipperDto, Shipper>();
            CreateMap<SupplierDto, Supplier>();
            CreateMap<TerritoryDto, Territory>();
        }
    }
}
