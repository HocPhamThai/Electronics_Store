namespace Electronics_Store.Client.Services.Service4Order_Client;

public interface IOrderService
{
    Task<string>  PutOrder();
    Task<List<SummaryOrderResponder>> GetOrders();
    Task<DetailOrderResponder> GetDetail(int orderId);
}