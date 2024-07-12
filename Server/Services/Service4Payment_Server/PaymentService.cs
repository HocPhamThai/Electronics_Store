using Stripe.Checkout;
using Stripe;

namespace Electronics_Store.Server.Services.Service4Payment_Server;

public class PaymentService: IPaymentService
{
    private readonly ICartService _cartService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IOrderService _orderService;
    const string secretKey = "whsec_cq1WH9CX9U1zxU9EpbBVvWOhfb6e5ysR";

    public PaymentService(ICartService cartService, IAuthenticationService authenticationService, IOrderService orderService)
    {
        _cartService = cartService;
        _authenticationService = authenticationService;
        _orderService = orderService;
        
        StripeConfiguration.ApiKey = "sk_test_51OHKgcDPPH5gGIhjA0lsVvNoWfVj50xhCn0E8m9NKgPdJ0svVnpZs0vjcnLt9o4Og1MsU9MYwFmmdMsluqvrq5zK00QpdGIzgo";
    }

    public async Task<ServiceResponder<bool>> CompleteOrder(HttpRequest purchaseRequest)
    {
        try {
            if (
                EventUtility.ConstructEvent(
                    await new StreamReader(purchaseRequest.Body).ReadToEndAsync() /*json as string*/,
                    purchaseRequest.Headers["Stripe-Signature"],
                    secretKey
                ) is var eStripe
                && eStripe.Type == Events.CheckoutSessionCompleted
            ) {
                Session? session = eStripe.Data.Object as Session;
                User? user = await _authenticationService.GetUserByEmail(session.CustomerEmail);
                await _orderService.PutOrder(user.userId);
            }
            return new ServiceResponder<bool> { Data = true, IsSuccess = true, Message = "Successfully Checked Out!"};
        } catch (StripeException e) {
            return new ServiceResponder<bool> { Data = false, IsSuccess = false, Message = e.Message };
        }
    }

    public async Task<Session> GenerateSession4Checkout()
    {
        List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();
        (await _cartService.GetUserCartProducts())
        .Data
        .ForEach(product => lineItems.Add(
            new SessionLineItemOptions
            {
                Quantity = product.quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmountDecimal = 100*product.price,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.name,
                        Images = new List<string> { product.imageUrl }
                    }
                }
            })
        );

        SessionCreateOptions options = new SessionCreateOptions
        {
            Mode = "payment",
            LineItems = lineItems,
            PaymentMethodTypes = new List<string> {"card"},
            CancelUrl = "https://localhost:5047/ShoppingCart",
            SuccessUrl = "https://localhost:5047/order-success",
            CustomerEmail = _authenticationService.GetUserEmail(),
            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                                        {
                                            AllowedCountries = new List<string> { "US" }
                                        }
        };
        return (new SessionService().Create(options)/*session*/);
    }
}