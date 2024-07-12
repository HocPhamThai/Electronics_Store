namespace Electronics_Store.Server.Services.Service4Orders_Server;

public interface IOrderService
{
    Task<ServiceResponder<bool>> PutOrder(int userId);
    Task<ServiceResponder<DetailOrderResponder>> GetDetails(int orderId);
    Task<ServiceResponder<List<SummaryOrderResponder>>> GetOrders();
}