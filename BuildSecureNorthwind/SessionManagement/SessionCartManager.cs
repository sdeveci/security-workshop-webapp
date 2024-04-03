using BuildSecureNorthwind.Models;
using System.Text.Json;

namespace BuildSecureNorthwind.SessionManagement
{
    public class SessionCartManager
    {
        private readonly List<CartItem> _cart;

        private readonly ISession _session;
        public SessionCartManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;

            _cart = new List<CartItem>();

            if (_session.Keys.Contains("CART"))
            {
                var stringData = _session.GetString("CART");
                _cart = JsonSerializer.Deserialize<List<CartItem>>(stringData);
            }
        }

        public void Add(CartItem cartItem)
        {
            var addedCartItem = _cart.SingleOrDefault(t => t.Product.Id == cartItem.Product.Id);

            if (addedCartItem is null)
                _cart.Add(cartItem);
            else
                addedCartItem.Quantity += cartItem.Quantity;

            var stringCart = JsonSerializer.Serialize(_cart);

            _session.SetString("CART", stringCart);
        }

        public decimal TotalPrice => _cart.Sum(t => t.Quantity * t.Product.UnitPrice);

        public decimal TotalQuantity => _cart.Sum(t => t.Quantity);

    }
}
