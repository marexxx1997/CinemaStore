using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using CinemaStore.Models;
using CinemaStore.Data.Services;

namespace CinemaStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckoutsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMoviesService _service;

        public CheckoutsController(IConfiguration configuration, IMoviesService service)
        {
            _configuration = configuration;
            _service = service; 
        }
        [HttpGet("create-checkout-session")]
        public IActionResult GetCheckoutSession(double price, string movieName, int quantity = 1, string movieFormat="", int movieId=1, int showTimeId=1)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var currency = "usd";

            switch (movieFormat)
            {
                case "3D":
                    price += 2.00;
                    break;
                case "4D":
                    price += 4.00;
                    break;
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Bioskopska karta",
                        Description = "Naziv događaja: "+ movieName + " Broj kupljenih karata: "+quantity,
                    },
            UnitAmount = (long)(price * 100 / 1.87)
                },
                Quantity = quantity,
            },
        },
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Checkouts/success?showTimeId={showTimeId}&quantity={quantity}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Checkouts/cancel",
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success(int showTimeId, int quantity)
        {
            Console.WriteLine($">>> Success metoda pozvana sa showTimeId={showTimeId} i quantity={quantity}");

            await _service.DecreaseAvailableSeatsAsync(showTimeId, quantity);

            return View();
        }


        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
