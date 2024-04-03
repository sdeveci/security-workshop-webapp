using BuildSecureNorthwind.DataAccess.Entities;

namespace BuildSecureNorthwind.Models
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
