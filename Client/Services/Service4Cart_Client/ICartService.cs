namespace Electronics_Store.Client.Services.Service4Cart_Client;

public interface ICartService
{
    event Action changeOfCart;
    Task AddToCart(CartProduct cartProduct);
    Task RemoveFromCart(int productId, int productVarietyId);
    Task UpdateQuantity(CartProductResponder product);
    
    Task PersistCartProducts(bool isLocalCartEmpty);
    Task CountCartProducts();
    Task<List<CartProductResponder>> GetCartProductResponses();
}