using BuildSecureNorthwind.SessionManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace BuildSecureNorthwind.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly SessionCartManager _cartManager;
        public CartSummaryViewComponent(SessionCartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public ViewViewComponentResult Invoke()
        {
            var model = new CartSummaryViewModel
            {
                TotalPrice=_cartManager.TotalPrice,
                TotalQuantity=_cartManager.TotalQuantity
            };

            return View(model);
        }
    }
}
