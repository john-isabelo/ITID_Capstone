using Microsoft.AspNetCore.Mvc;
using WeeklyTask.Models.ViewModels;
using WeeklyTask.Models;
using Microsoft.AspNetCore.Http.Extensions;


namespace WeeklyTask.Infrastructure.Components
{

    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (HttpContext == null)
            {
                // return a default value or error message if HttpContext is null
                return Content("HttpContext is null");
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartVM;

            if (cart == null || cart.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = (int)cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVM);
        }

    }
}
