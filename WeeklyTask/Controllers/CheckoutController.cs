using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;
using System.Security.Claims;
using WeeklyTask.Areas.Identity.Data;
using WeeklyTask.Infrastructure;
using WeeklyTask.Models;
using WeeklyTask.Models.Helpers;
using WeeklyTask.Models.ViewModels;

namespace WeeklyTask.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly FoodDbContext _context;

        public CheckoutController(FoodDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string TotalAmount { get; set; }


        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(Tuple.Create(cartVM, new Order()));
        }

        [HttpPost]
        public async Task<IActionResult> Processing(string stripeToken, string stripeEmail, string deliveryAddress, string customerPhone, string description, string guestEmail)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            decimal grandTotal = cart.Sum(x => x.Quantity * x.Price);

            var optionsCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = User.Identity.Name,
                Phone = customerPhone
            };

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(grandTotal * 100),
                Currency = "cad",
                PaymentMethodTypes = new List<string> { "card" },
                ReceiptEmail = stripeEmail
                //Description = "3% of your purchase goes toward our ocean cleanup effort!",
            };
            var services = new PaymentIntentService();
            services.Create(options);

            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionsCust);

            var optionsCharge = new ChargeCreateOptions
            {
                Amount = (long)(grandTotal * 100), // Convert to cents
                Currency = "CAD",
                Description = "Buying Food",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
            };

            var service = new ChargeService();
            Charge charge = service.Create(optionsCharge);

            if (charge.Status == "succeeded")
            {
                
                // Create a new Order object and save it to the database
                Order order = new Order
                {
                    Description = description,
                    DeliveryAddress = deliveryAddress,
                    CustomerPhone = customerPhone,
                    TotalAmount = grandTotal,
                    UserEmail = User.Identity.Name,
                    GuestEmail = guestEmail
                    //UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // get the current user's ID
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Reload the Order object from the database
                order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);

                string BalanceTransactionId = charge.BalanceTransactionId;
                ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                ViewBag.BalanceTxId = BalanceTransactionId;
                ViewBag.Customer = User.Identity.Name;
                

                var cartVM = new CartViewModel
                {
                    CartItems = cart,
                    GrandTotal = grandTotal
                };

                var orderVM = new Tuple<CartViewModel, Order>(cartVM, order);
                return View("Processing", orderVM);
            }

            return View("Error");
        }

        

    }
}
