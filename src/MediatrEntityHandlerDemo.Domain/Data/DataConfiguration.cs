using MediatrEntityHandlerDemo.Domain.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediatrEntityHandlerDemo.Domain.Data
{
    public class DataConfiguration
    {
        public void Seed(DatabaseContext context)
        {
            SeedData<Category>(context);
            SeedData<Customer>(context);
            SeedData<CustomerDemographic>(context);
            SeedData<Demographic>(context);
            SeedData<Employee>(context);
            SeedData<EmployeeTerritory>(context);
            SeedData<Order>(context);
            SeedData<OrderDetail>(context);
            SeedData<Product>(context);
            SeedData<Region>(context);
            SeedData<Shipper>(context);
            SeedData<Supplier>(context);
            SeedData<Territory>(context);

            context.SaveChanges();
        }

        private void SeedData<T>(DatabaseContext context)
            where T : class
        {
            var type = typeof(T);
            var ns = typeof(DataConfiguration).Assembly.FullName.Split(',')[0];
            var dataNS = $"{ns}.Data";
            var dataFile = $"{dataNS}.{type.Name.ToLower()}.json";
            var json = GetResource(dataFile);

            var listType = typeof(IEnumerable<>).MakeGenericType(type);

            var entities = JsonConvert.DeserializeObject(json, listType);

            var dbset = context.Set<T>();

            dbset.AddRange((IEnumerable<T>)entities);
        }

        private string GetResource(string resourceName)
        {
            var result = string.Empty;
            var assembly = typeof(DataConfiguration).Assembly;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1")))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
