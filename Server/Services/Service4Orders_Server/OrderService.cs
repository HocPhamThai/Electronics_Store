namespace Electronics_Store.Server.Services.Service4Orders_Server;

public class OrderService: IOrderService
{
    private readonly ElectronicsStoreDbContext _dbContext;
    private readonly ICartService _cartService;
    private readonly IAuthenticationService _authenticationService;

    public OrderService(ElectronicsStoreDbContext dbContext, ICartService cartService, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _cartService = cartService;
        _authenticationService = authenticationService;
    }

    public async Task<ServiceResponder<bool>> PutOrder(int userId)
    {
        List<CartProductResponder>? cartProducts = (await _cartService.GetUserCartProducts(userId)).Data;
        List<OrderProduct> orderProducts = new List<OrderProduct>();
        decimal totalPrice = 0;
        cartProducts.ForEach(cartProduct => totalPrice += (cartProduct.price * cartProduct.quantity));
        cartProducts.ForEach(cartProduct => orderProducts.Add(new OrderProduct
                                                                {
                                                                    productId = cartProduct.productId,
                                                                    productVarietyId = cartProduct.productVarietyId,
                                                                    quantity = cartProduct.quantity,
                                                                    orderProductPrice = cartProduct.price * cartProduct.quantity
                                                                }
                                                            )
        );
        _dbContext.Orders.Add(
            new Order
            {
                userId = userId,
                orderDate = DateTime.Now,
                orderPrice = totalPrice,
                orderProducts = orderProducts
            }
        );
        _dbContext.CartProducts.RemoveRange(_dbContext.CartProducts.Where(cp => cp.userId == userId));
        await _dbContext.SaveChangesAsync();
        return new ServiceResponder<bool> { Data = true };
    }
    
    public async Task<ServiceResponder<DetailOrderResponder>> GetDetails(int orderId)
    {
        Order? order = await _dbContext.Orders
            .Include(o => o.orderProducts)
            .ThenInclude(op => op.product)
            .Include(o => o.orderProducts)
            .ThenInclude(op => op.productVariety)
            .Where(o => o.userId == _authenticationService.GetUserId() && o.orderId == orderId)
            .OrderByDescending(o => o.orderDate)
            .FirstOrDefaultAsync();
        if (order != null)
        {
            DetailOrderResponder orderDetailsResponse = new DetailOrderResponder
            {
                totalPrice = order.orderPrice,
                orderDate = order.orderDate,
                products = new List<DetailOrderProductResponder>()
            };
            order.orderProducts.ForEach(item =>
                orderDetailsResponse.products.Add(new DetailOrderProductResponder {
                    quantity = item.quantity,
                    totalPrice = item.orderProductPrice,
                    
                    productId = item.productId,
                    productName = item.product.ProductName,
                    productVariety = item.productVariety.Name,
                    productImageUrl = item.product.ProductImageUrl
                })
            );
            return new ServiceResponder<DetailOrderResponder> { IsSuccess = true, Data = orderDetailsResponse };
        }
        return new ServiceResponder<DetailOrderResponder> { IsSuccess = false, Message = "There's No Order With Specified Id" };
    }

    public async Task<ServiceResponder<List<SummaryOrderResponder>>> GetOrders()
    {
        ServiceResponder<List<SummaryOrderResponder>> summariesResponse = new ServiceResponder<List<SummaryOrderResponder>>();
        List<SummaryOrderResponder> summaries = new List<SummaryOrderResponder>();
        (await _dbContext.Orders
                .Include(order => order.orderProducts)
                .ThenInclude(orderProduct => orderProduct.product)
                .Where(order => order.userId == _authenticationService.GetUserId())
                .OrderByDescending(order => order.orderDate)
                .ToListAsync()) /*orders match query sort by desc date*/
        .ForEach(order => summaries.Add (
            new SummaryOrderResponder
                {
                    id = order.orderId,
                    orderDate = order.orderDate,
                    totalPrice = order.orderPrice,
                    product = order.orderProducts.Count > 1 ?
                                ($"{order.orderProducts.First().product.ProductName} and"
                                 + $" {order.orderProducts.Count - 1} more...")
                              : order.orderProducts.First().product.ProductName,
                    productImageUrl = order.orderProducts.First().product.ProductImageUrl
                }
            )
        );
        summariesResponse.Data = summaries;
        return summariesResponse;
    }
}