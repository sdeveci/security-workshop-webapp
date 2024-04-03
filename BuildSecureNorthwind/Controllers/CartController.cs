using BuildSecureNorthwind.Contexts;
using BuildSecureNorthwind.DataAccess.Entities;
using BuildSecureNorthwind.Models;
using BuildSecureNorthwind.SessionManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildSecureNorthwind.Controllers
{
    public class CartController : Controller
    {
        private readonly SessionCartManager _cartManager;
        private readonly NorthwindContext _context;

        public CartController(SessionCartManager cartManager, NorthwindContext context)
        {
            _cartManager = cartManager;
            _context = context;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid productId)
        {
            var product = _context.Products.SingleOrDefault(t => t.Id == productId);

            if (product == null)
                throw new BusinessRuleException("Ürün sistemde bulunamadı");

            _cartManager.Add(new Models.CartItem
            {
                Product = product,
                Quantity = 1
            });

            return RedirectToAction("Index", "Product");
        }
    }
}
