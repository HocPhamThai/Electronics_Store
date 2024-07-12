namespace Electronics_Store.Server.Services.Service4Cart_Server;

public interface ICartService
{
    Task<ServiceResponder<int>> CountCartProducts();
    
    Task<ServiceResponder<List<CartProductResponder>>> GetCartProductResponses(List<CartProduct> cartProducts);
    Task<ServiceResponder<List<CartProductResponder>>> PersistCartProducts(List<CartProduct> cartProducts);
    Task<ServiceResponder<List<CartProductResponder>>> GetUserCartProducts(int? userId = null);
    
    Task<ServiceResponder<bool>> Add2Cart(CartProduct cartProduct);
    Task<ServiceResponder<bool>> UpdateQuantity(CartProduct cartProduct);
    Task<ServiceResponder<bool>> RemoveCartProduct(int productId, int productVarietyId);
}