using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using WCPShared.Interfaces;
using WCPShared.Services.StaticHelpers;

namespace WCPFrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : Controller
    {
        private readonly IWcpDbContext _context;
        private readonly IConfiguration _configuration;

        public StripeController(IWcpDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = Secrets.GetStripeWebhookKey(_configuration);
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var checkout = stripeEvent.Data.Object as Session;

                    CustomerService customerService = new CustomerService();
                    Customer customer = customerService.Get(checkout.CustomerId);

                    var org = await _context.Organizations.Include(x => x.Subscription)
                        .SingleOrDefaultAsync(x => x.StripeAccountId == checkout.CustomerId);

                    if (org is null)
                    {
                        var user = (await _context.Users.Include(x => x.Organization)
                            .ThenInclude(x => x.Subscription)
                            .SingleOrDefaultAsync(x => x.Email == customer.Email));

                        if (user is not null)
                        {
                            org = user.Organization;

                            if (string.IsNullOrEmpty(org.StripeAccountId) || org.StripeAccountId != checkout.CustomerId)
                                org.StripeAccountId = checkout.CustomerId;
                        }   
                    }

                    if (org is not null)
                    {
                        var service = new SessionLineItemService();
                        StripeList<LineItem> lineItems = service.List(checkout.Id);

                        // Check the product ID of WCP subscription in line items
                        if (lineItems.Any(x => x.Price.ProductId == "prod_RASyvFiFqfKwLD"))
                        {
                            org.Subscription.LastPaid = DateTime.Now;
                            org.IsActive = true;
                            await _context.SaveChangesAsync();
                            Console.WriteLine("A new subscription was successful for {0}.", org.Name);
                        }
                    }
                }
                else if (stripeEvent.Type == EventTypes.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    Console.WriteLine("A subcription was cancelled for {0}.", subscription.CustomerId);
                    // Then define and call a method to handle the successful payment intent.

                    var org = await _context.Organizations.SingleOrDefaultAsync(x => x.StripeAccountId == subscription.CustomerId);

                    if (org is not null)
                    {
                        org.IsActive = false;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("A new subscription was successful for {0}.", subscription.Customer.Email);
                    }

                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
