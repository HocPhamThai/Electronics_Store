using Blazored.LocalStorage;

namespace Electronics_Store.Client.Services.Service4Cart_Client;

public class CartService: ICartService
{
    public event Action? changeOfCart;
    
    private readonly IAuthenticationService _authenticationService;
    private readonly ILocalStorageService _storageService;
    private readonly HttpClient _httpClient;

    public CartService(ILocalStorageService localStorage, HttpClient httpClient, IAuthenticationService authenticationService)
    {
        _storageService = localStorage;
        _httpClient = httpClient;
        _authenticationService = authenticationService;
    }
    
    public async Task AddToCart(CartProduct cartProduct)
    {
        if (!await _authenticationService.IsAuthenticatedUser())
        {
            List<CartProduct>? cart = await _storageService.GetItemAsync<List<CartProduct>>("cart") ?? new List<CartProduct>();
            CartProduct? foundCartProduct = cart.Find(cart_product => cart_product.productId == cartProduct.productId
                                                     && cart_product.productVarietyId == cartProduct.productVarietyId);
            if (foundCartProduct == null)
                cart?.Add(cartProduct);
            else
                foundCartProduct.quantity += cartProduct.quantity;
            await _storageService.SetItemAsync("cart", cart);
            changeOfCart?.Invoke();
        }
        else await _httpClient.PostAsJsonAsync("api/cart/add", cartProduct);

        await CountCartProducts();
    }

    public async Task RemoveFromCart(int productId, int productVarietyId)
    {
        if (!await _authenticationService.IsAuthenticatedUser())
        {
            List<CartProduct>? cart = await _storageService.GetItemAsync<List<CartProduct>>("cart");
            CartProduct? cartProduct = cart?.Find(cart_product => cart_product.productId == productId
                                                                  && cart_product.productVarietyId == productVarietyId)
                                            ?? null ;
            if (cartProduct != null)
            {
                cart?.Remove(cartProduct);
                await _storageService.SetItemAsync("cart", cart);
                changeOfCart?.Invoke();
            }
        }
        else await _httpClient.DeleteAsync($"api/cart/{productId}/{productVarietyId}");
    }

    public async Task UpdateQuantity(CartProductResponder responder)
    {
        if (!await _authenticationService.IsAuthenticatedUser())
        {
            List<CartProduct> cart = await _storageService.GetItemAsync<List<CartProduct>>("cart");
            CartProduct? cartProduct = cart?.Find(cart_product => cart_product.productId == responder.productId
                                                                  && cart_product.productVarietyId ==
                                                                  responder.productVarietyId)
                                       ?? null;
            if (cartProduct != null)
            {
                cartProduct.quantity = responder.quantity;
                await _storageService.SetItemAsync("cart", cart);
            }
        }
        else await _httpClient.PutAsJsonAsync("api/cart/update-quantity", 
                                                 new CartProduct
                                                      {
                                                          productId = responder.productId,
                                                          quantity = responder.quantity,
                                                          productVarietyId = responder.productVarietyId
                                                      }
             );
        
    }

    public async Task PersistCartProducts(bool isLocalCartEmpty = false)
    {
        List<CartProduct>? localCart = await _storageService.GetItemAsync<List<CartProduct>>("cart");
        if (localCart != null)
        {
            await _httpClient.PostAsJsonAsync("api/cart", localCart);
            if (isLocalCartEmpty) await _storageService.RemoveItemAsync("cart");
        }
    }

    public async Task CountCartProducts()
    {
        int count;
        if (!await _authenticationService.IsAuthenticatedUser())
        {
            List<CartProduct>? cart = await _storageService.GetItemAsync<List<CartProduct>>("cart");
            count = cart?.Count ?? 0;
        }
        else
        {
            ServiceResponder<int>? result = await _httpClient.GetFromJsonAsync<ServiceResponder<int>>("api/cart/count");
            count = result?.Data ?? 0;
        }
        await _storageService.SetItemAsync<int>("Num_Of_CartProducts", count);
        changeOfCart.Invoke();
    }

    public async Task<List<CartProductResponder>> GetCartProductResponses()
    {
        if (await _authenticationService.IsAuthenticatedUser())
            return (await _httpClient.GetFromJsonAsync<ServiceResponder<List<CartProductResponder>>>("api/cart"))?/*a ServiceResponder*/
                    .Data ?? new List<CartProductResponder>();
        
        List<CartProduct>? cartProducts = await _storageService.GetItemAsync<List<CartProduct>>("cart");
        if (cartProducts != null)
            return (await (await _httpClient.PostAsJsonAsync("api/cart/products", cartProducts))/*an http response*/
                    .Content.ReadFromJsonAsync<ServiceResponder<List<CartProductResponder>>>())/*service response*/
                    ?.Data ?? new List<CartProductResponder>();
        
        return new List<CartProductResponder>();
    }
}