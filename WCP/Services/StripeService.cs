using Stripe;
using Stripe.Checkout;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Services
{
    public class StripeService
    {
        private readonly IWcpDbContext _context;

        public StripeService(IWcpDbContext context)
        {
            _context = context;
        }

        public Session StartCheckoutSession(List<SessionLineItemOptions> lineItems, string sessionMode, string? customerId = null, string? customerEmail = null, Dictionary<string, string>? metaData = null)
        {
            var options = new SessionCreateOptions
            {
                Mode = sessionMode,
                LineItems = lineItems,
                AllowPromotionCodes = true,
                SuccessUrl = "https://example.com/success?session_id={CHECKOUT_SESSION_ID}",
            };

            if (sessionMode == "payment" && metaData is not null)
                options.PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = metaData,
                };

            else if (sessionMode == "subscription" && metaData is not null)
                options.SubscriptionData = new SessionSubscriptionDataOptions
                {
                    Metadata = metaData,
                };

            if (!string.IsNullOrEmpty(customerId))
                options.Customer = customerId;
            else if (!string.IsNullOrEmpty(customerEmail))
                options.CustomerEmail = customerEmail;

            var service = new SessionService();
            return service.Create(options);
        }

        public Customer CreateCustomer(User user)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
            };

            var customerService = new CustomerService();
            return customerService.Create(customerOptions);
        }

        public List<SessionLineItemOptions> GenerateLineItemsOld(WCPShared.Models.Entities.Subscription subscription)
        {
            var lineItems = new List<SessionLineItemOptions>();

            //if (subscription.Type == SubscriptionType.Small)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QIcSMHvTm5ojAP1fHGmabbx", Quantity = 1 });

            //if (subscription.Type == SubscriptionType.Medium)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QI82YHvTm5ojAP1RfnEQ6aY", Quantity = 1 });

            //if (subscription.Type == SubscriptionType.Large)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QIcSfHvTm5ojAP1bIm4bRat", Quantity = 1 });

            //var subscriptionIncluded = GetSubscriptionInfo(subscription.Type);

            //if (subscription.NumberOfVideos > subscriptionIncluded.NumberOfVideos)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QIcUKHvTm5ojAP1Jvpzjhlv", Quantity = subscription.NumberOfVideos - subscriptionIncluded.NumberOfVideos });

            //if (subscription.NumberOfBrands > subscriptionIncluded.NumberOfBrands)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QIcUZHvTm5ojAP1Nd5KbYkj", Quantity = subscription.NumberOfBrands - subscriptionIncluded.NumberOfBrands });

            //if (subscription.NumberOfUsers > subscriptionIncluded.NumberOfUsers)
            //    lineItems.Add(new SessionLineItemOptions { Price = "price_1QIcUoHvTm5ojAP1qdp3Fuks", Quantity = subscription.NumberOfUsers - subscriptionIncluded.NumberOfUsers });
            return lineItems;
        }
        
        public List<SessionLineItemOptions> GenerateLineItems(WCPShared.Models.Entities.Subscription subscription)
        {
            var lineItems = new List<SessionLineItemOptions>();

            if (subscription.Type == SubscriptionType.Small)
                lineItems.Add(new SessionLineItemOptions { Price = "price_1QjIx4RvgKwQz1aGXGC1dQtn", Quantity = 1 });

            if (subscription.Type == SubscriptionType.Medium)
                lineItems.Add(new SessionLineItemOptions { Price = "price_1QjIxIRvgKwQz1aGGX6AszdN", Quantity = 1 });

            if (subscription.Type == SubscriptionType.Large)
                lineItems.Add(new SessionLineItemOptions { Price = "price_1QjIxTRvgKwQz1aG0LN0p4Xy", Quantity = 1 });

            var subscriptionIncluded = GetSubscriptionInfo(subscription.Type);

            // Add extras
            if (subscription.Type != SubscriptionType.Large)
            {
                if (subscription.NumberOfBrands > subscriptionIncluded.NumberOfBrands)
                    lineItems.Add(new SessionLineItemOptions { Price = "price_1QjIwCRvgKwQz1aGP9UD21X9", Quantity = subscription.NumberOfBrands - subscriptionIncluded.NumberOfBrands });
            }

            if (subscription.NumberOfUsers > subscriptionIncluded.NumberOfUsers)
                lineItems.Add(new SessionLineItemOptions { Price = "price_1QjIvERvgKwQz1aGbanCgfU5", Quantity = subscription.NumberOfUsers - subscriptionIncluded.NumberOfUsers });

            return lineItems;
        }

        public Account CreateAccount(string email, string type, string country)
        {
            // Start stripe onboarding creators
            var accountOptions = new AccountCreateOptions
            {
                Type = "express", // or "standard"
                Country = country, // Adjust as needed
                BusinessType = type, // Use "individual" to avoid company info
                Email = email,
                Capabilities = new AccountCapabilitiesOptions
                {
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                },
                Settings = new AccountSettingsOptions 
                {
                    Payouts = new AccountSettingsPayoutsOptions
                    {
                        Schedule = new AccountSettingsPayoutsScheduleOptions
                        {
                            Interval = "monthly",
                            MonthlyAnchor = 1
                        }
                    }
                }
            };
            
            var accountService = new AccountService();
            return accountService.Create(accountOptions);
        }

        public AccountLink CreateAccountLink(string accountId)
        {
            var accountLinkOptions = new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = "https://wcp.dk/refresh",
                ReturnUrl = "https://wcp.dk.com/onboarding-complete",
                Type = "account_onboarding",
            };
            var accountLinkService = new AccountLinkService();
            return accountLinkService.Create(accountLinkOptions);
        }

        public WCPShared.Models.Entities.Subscription GetSubscriptionInfo(SubscriptionType type)
        {
            var subscription = new WCPShared.Models.Entities.Subscription();

            if (type == SubscriptionType.Small)
                subscription = new WCPShared.Models.Entities.Subscription
                {
                    Type = type,
                    NumberOfBrands = 1,
                    NumberOfUsers = 1,
                };

            if (type == SubscriptionType.Medium)
                subscription = new WCPShared.Models.Entities.Subscription
                {
                    Type = type,
                    NumberOfBrands = 1,
                    NumberOfUsers = 1,
                };

            if (type == SubscriptionType.Large)
                subscription = new WCPShared.Models.Entities.Subscription
                {
                    Type = type,
                    NumberOfBrands = -1,
                    NumberOfUsers = 1,
                };

            return subscription;
        }

        public Transfer Transfer(float amount, string accountId, string description, string currency)
        {
            var transfer = new TransferCreateOptions
            {
                Amount = (long) amount * 100, // Convert to cents
                Currency = currency,
                Description = description,
                Destination = accountId,
            };
            
            TransferService transferService = new TransferService();
            return transferService.Create(transfer);
        }

        public Transfer Transfer(PaymentIntent intent)
        {
            var transfer = new TransferCreateOptions
            {
                Amount = intent.TransferData.Amount, // Convert to cents
                Currency = intent.Currency,
                Description = "Creator salary",
                Destination = intent.TransferData.DestinationId,
                Metadata = intent.Metadata
            };

            TransferService transferService = new TransferService();
            return transferService.Create(transfer);
        }

        public PaymentIntent CreateCharge(float amount, string currency, string? creatorAccountId = null, string? customerId = null, float? creatorFee = null, int? orderId = null)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long) (amount * 100 * 1.25), // Incl. VAT
                Currency = currency,
            };

            if (!string.IsNullOrEmpty(customerId))
                options.Customer = customerId;

            if (orderId is not null)
                options.Metadata = new Dictionary<string, string>()
                {
                    { "orderId", orderId.Value.ToString() }
                };

            var service = new PaymentIntentService();
            return service.Create(options);
        }

        public async Task<Balance> GetBalance(string accountId)
        {
            BalanceService balanceService = new BalanceService();
            return await balanceService.GetAsync(new RequestOptions { StripeAccount = accountId });
        }

        public async Task<Payout> CreatePayout(string accountId, float amount, string currency)
        {
            var payoutService = new PayoutService();

            var payoutOptions = new PayoutCreateOptions
            {
                Amount = (long) amount * 100, // Amount is in cents
                Currency = currency,
                Method = "standard"
            };

            var requestOptions = new RequestOptions
            {
                StripeAccount = accountId,
            };

            return await payoutService.CreateAsync(payoutOptions, requestOptions);
        }

        public async Task<IEnumerable<Payout>> GetAccountPayouts(string accountId)
        {
            var payoutService = new PayoutService();

            var payoutListOptions = new PayoutListOptions
            {
                Limit = 100,
            };

            var requestOptions = new RequestOptions
            {
                StripeAccount = accountId
            };

            return await payoutService.ListAsync(payoutListOptions, requestOptions);
        }

        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods(string accountId)
        {
            var options = new PaymentMethodListOptions
            {
                Customer = accountId,
                Type = "card", // Fetch only card payment methods
            };

            var service = new PaymentMethodService();
            StripeList<PaymentMethod> paymentMethods = await service.ListAsync(options);
            return paymentMethods.Data;
        } 

        public PaymentIntent ConfirmPayment(string paymentIntent, string paymentMethod)
        {
            var options = new PaymentIntentConfirmOptions
            {
                PaymentMethod = paymentMethod,
                ReturnUrl = "https://www.example.com",
            };
            var service = new PaymentIntentService();
            return service.Confirm(paymentIntent, options);
        }

        public Account GetAccount(string accountId)
        {
            var accountService = new AccountService();
            return accountService.Get(accountId);
        }

        public async Task<IEnumerable<Transfer>> GetAccountTransfers(string accountId)
        {
            var transferOptions = new TransferListOptions
            {
                Limit = 100,
                Destination = accountId
            };

            var transferService = new TransferService();
            return await transferService.ListAsync(transferOptions);
        }

        public async Task<bool> CheckOnboardingStatus(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                return false;
            
            var service = new AccountService();
            var account = await service.GetAsync(accountId);
            var requirements = account.Requirements;

            if (requirements != null && requirements.CurrentlyDue.Count > 0)
                 return false;

            return true;
        }

        public async Task<List<Stripe.Subscription>> GetAllSubscriptions(string accountId)
        {
            var options = new SubscriptionListOptions
            {
                Customer = accountId,
            };

            var service = new SubscriptionService();
            return (await service.ListAsync(options)).ToList();
        }

        public async Task<Stripe.Subscription> GetSubscription(string subscriptionId)
        {
            var options = new SubscriptionGetOptions
            {
                Expand = new List<string> { "items.data.price.product" }
            };

            var service = new SubscriptionService();
            return await service.GetAsync(subscriptionId, options);
        }

        public async Task CancelAllSusbcriptions(string accountId)
        {
            var subscriptions = await GetAllSubscriptions(accountId);

            foreach (var subscription in subscriptions)
            {
                await CancelSubscriptionAtEndOfBillingCycle(subscription.Id);
            }
        }
 
        public async Task CancelSubscriptionAtEndOfBillingCycle(string subscriptionId)
        {
            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = true
            };

            var service = new SubscriptionService();
            await service.UpdateAsync(subscriptionId, options);
        }

        public async Task AddPaymentMethod(string customerId, PaymentMethodCreateOptions method)
        {
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.CreateAsync(method);

            await paymentMethodService.AttachAsync(paymentMethod.Id, new PaymentMethodAttachOptions
            {
                Customer = customerId,
            });
        }

        public async Task SetDefaultPaymentMethod(string customerId, string paymentMethodId)
        {
            var customerService = new CustomerService();
            await customerService.UpdateAsync(customerId, new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId,
                },
            });
        }
    }
}