using Microsoft.AspNetCore.Mvc;

namespace BuildSecureNorthwind.DataAccess.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
