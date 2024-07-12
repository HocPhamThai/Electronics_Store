namespace Electronics_Store.Server.Services.Service4Payment_Server;
using Stripe.Checkout;
public interface IPaymentService
{
    Task<ServiceResponder<bool>> CompleteOrder(HttpRequest request);
    Task<Session> GenerateSession4Checkout();
}