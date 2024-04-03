using BuildSecureNorthwind.DataAccess.Entities;

namespace BuildSecureNorthwind.Models
{
    internal class ProductIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}