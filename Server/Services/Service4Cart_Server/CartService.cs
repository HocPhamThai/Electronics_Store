using System.Security.Claims;

namespace Electronics_Store.Server.Services.Service4Cart_Server;

public class CartService: ICartService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ElectronicsStoreDbContext _dbContext;

    public CartService(ElectronicsStoreDbContext dbContext, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
    }
    
    public async Task<ServiceResponder<int>> CountCartProducts()
        => new()
        {
            Data = (await _dbContext.CartProducts.Where(cartProduct => cartProduct.userId == _authenticationService.GetUserId()).ToListAsync()).Count
        };
    
    public async Task<ServiceResponder<List<CartProductResponder>>> GetCartProductResponses(List<CartProduct> cartProducts)
    {
        ServiceResponder<List<CartProductResponder>> result = new ServiceResponder<List<CartProductResponder>>() {Data = new List<CartProductResponder>()};
        foreach (CartProduct item in cartProducts)
        {
            Product? product = await initProduct(item);
            if (product != null)
            {
                ProductVariant? productVariant = await initProductVariant(item);
                if (productVariant != null)
                {
                    CartProductResponder responder = initCartProductResponder(product, productVariant, item);
                    result.Data.Add(responder);
                }
            }
        }
        return result;
    }

    public async Task<ServiceResponder<List<CartProductResponder>>> PersistCartProducts(List<CartProduct> cartProducts)
    {
        foreach (CartProduct cartProduct in cartProducts)
            cartProduct.userId = _authenticationService.GetUserId();
        _dbContext.CartProducts.AddRange(cartProducts);
        await _dbContext.SaveChangesAsync();
        return await GetUserCartProducts();
    }

    public async Task<ServiceResponder<List<CartProductResponder>>> GetUserCartProducts(int? userId = null)
    {
        userId ??= _authenticationService.GetUserId();
        return await GetCartProductResponses(
            await _dbContext.CartProducts.Where(cartProduct => cartProduct.userId == userId).ToListAsync()
        );
    }

    public async Task<ServiceResponder<bool>> Add2Cart(CartProduct cartProduct)
    {
        cartProduct.userId = _authenticationService.GetUserId();
        string add2CartMessage;
        CartProduct? sameItem = await _dbContext
                                .CartProducts
                                .FirstOrDefaultAsync(cp => cp.productId == cartProduct.productId
                                                                     && cp.productVarietyId == cartProduct.productVarietyId && cp.userId == cartProduct.userId
                                                                    );
        if (sameItem != null)
        {
            sameItem.quantity += cartProduct.quantity;
            add2CartMessage = "Existing Item, Increased Quantity by 1!";
        }
        else
        {
            _dbContext.CartProducts.Add(cartProduct);
            add2CartMessage = "Successfully Add a New Item to Cart";
        }

        await _dbContext.SaveChangesAsync();
        return new ServiceResponder<bool> { Data = true, Message = add2CartMessage};
    }

    public async Task<ServiceResponder<bool>> UpdateQuantity(CartProduct cartProduct)
    {
        CartProduct? dbCartItem =
            await _dbContext
            .CartProducts
            .FirstOrDefaultAsync(cp => cp.productId == cartProduct.productId
                                                 && cp.productVarietyId == cartProduct.productVarietyId && cp.userId == _authenticationService.GetUserId()
            );
        bool isSuccess = false;
        string productName = string.Empty;
        if (dbCartItem != null)
        {
            isSuccess = true;
            dbCartItem.quantity = cartProduct.quantity;
            await _dbContext.SaveChangesAsync();
            productName = (await _dbContext.Products.FirstOrDefaultAsync(p=> p.ProductId == dbCartItem.productId))?.ProductName ?? string.Empty;
        }

        return new ServiceResponder<bool>
        {
            Data = isSuccess,
            IsSuccess = isSuccess,
            Message = isSuccess? $"Successfully Update Quantity of {productName}"
                    : "Product doesn't exist!"
        };
    }

    public async Task<ServiceResponder<bool>> RemoveCartProduct(int productId, int productVarietyId)
    {
        CartProduct? dbCartItem =
            await _dbContext
            .CartProducts
            .FirstOrDefaultAsync(cartProduct => cartProduct.productId == productId
                                                && cartProduct.productVarietyId == productVarietyId && cartProduct.userId == _authenticationService.GetUserId());
        bool isSuccess = false;
        string productName = string.Empty;
        if (dbCartItem != null)
        {
            isSuccess = true;
            productName = (await _dbContext.Products.FirstOrDefaultAsync(p=> p.ProductId == dbCartItem.productId))?.ProductName ?? string.Empty;
            _dbContext.CartProducts.Remove(dbCartItem);
            await _dbContext.SaveChangesAsync();
        }

        return new ServiceResponder<bool>
        {
            Data = isSuccess,
            IsSuccess = isSuccess,
            Message = isSuccess ? $"Successfully Remove {productName}"
                : "Product doesn't exist!"
        };
    }

    private async Task<Product?> initProduct(CartProduct item)
        => await _dbContext.Products
            .Where(product => product.ProductId == item.productId)
            .FirstOrDefaultAsync();
    
    private CartProductResponder initCartProductResponder(Product product, ProductVariant variant, CartProduct cartProduct)
        => new CartProductResponder()
            {
                productId = product.ProductId,
                name = product.ProductName,
                imageUrl = product.ProductImageUrl,
                price = variant.price,
                productVariety = variant.productVariety.Name,
                productVarietyId = variant.productVarietyId,
                quantity = cartProduct.quantity
                // new CartProductResponder()
                // {
                //     productId = Product.ProductId,
                //     name = Product.ProductName,
                //     imageUrl = Product.ProductImageUrl,
                //     price = productVariant.price,
                //     productVariety = productVariant.productVariety.Name,
                //     productVarietyId = productVariant.productVarietyId,
                //     quantity = item.quantity
                // };
            };

    private async Task<ProductVariant?> initProductVariant(CartProduct item)
        =>  await _dbContext.ProductVariants
            .Where(variant => variant.productId == item.productId
                              && variant.productVarietyId == item.productVarietyId)
            .Include(variant => variant.productVariety)
            .FirstOrDefaultAsync();
}


