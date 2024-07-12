

namespace Electronics_Store.Server.Services.Service4Products_Server;

public class ProductsService : IProductsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ElectronicsStoreDbContext _dbContext;

    public ProductsService(IHttpContextAccessor httpContextAccessor, ElectronicsStoreDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }
    
    public async Task<ServiceResponder<List<Product>>> GetProductsAsync() => new()
    {
        Data = await _dbContext
            .Products
            .Where(product => product.isViewable && !product.isDeleted)
            .Include(product => product.ProductVariants.Where(variant => variant.isViewable && !variant.isDeleted))
            .Include(product => product.Pictures)
            .ToListAsync()
    };

    public async Task<ServiceResponder<List<Product>>> GetProductsAsAdmin() => new()
    {
        Data = await _dbContext.Products
                .Where(product => !product.isDeleted)
                .Include(product => product.ProductVariants.Where(variant => !variant.isDeleted))
                .ThenInclude(variant => variant.productVariety)
                .Include(product => product.Pictures)
                .ToListAsync()
    };

    public async Task<ServiceResponder<Product>> GetProductByIdAsync(int productId)
    {
        Product? foundDbProduct;

        if (!_httpContextAccessor.HttpContext.User.IsInRole("Administrator"))
            foundDbProduct = await _dbContext
                .Products
                .Include(p => p.ProductVariants.Where(variant => !variant.isDeleted && variant.isViewable))
                .ThenInclude(variant => variant.productVariety)
                .Include(product => product.Pictures)
                .FirstOrDefaultAsync(p => p.ProductId == productId && !p.isDeleted && p.isViewable);
        else
            foundDbProduct = await _dbContext
                .Products
                .Include(p => p.ProductVariants.Where(v => !v.isDeleted))
                .ThenInclude(v => v.productVariety)
                .Include(product => product.Pictures)
                .FirstOrDefaultAsync(p => p.ProductId == productId && !p.isDeleted);
        
        if (foundDbProduct != null)
        {
            return new ServiceResponder<Product>()
            {
                Data = foundDbProduct
            };
        }
        else
        {
            return new ServiceResponder<Product>()
            {
                IsSuccess = false,
                Message = "Searching Failed. Error: We Can't Find That Product."
            };
        }
    }
    
    public async Task<ServiceResponder<List<string>>> GetSearchSuggestionsAsync(string search)
    {
        List<string> searchContainedWords = new List<string>();

        (await FindProductsBySearchText(search)).ForEach(product =>
        {
            if (product.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase))
                searchContainedWords.Add(product.ProductName);
                    
            if (product is { ProductDetail: not null })
                foreach (string word in product.ProductDetail
                             .Split() /* = array of words not normalized*/
                             .Select(s => s.Trim
                                 ( /*array of punctuations in the product description,
                                    used to normalize*/
                                     product.ProductDetail
                                         .Where(char.IsPunctuation)
                                         .Distinct()
                                         .ToArray()
                                 ) /* = array of words normalized*/
                             )
                        )
                        if (word.Contains(search, StringComparison.OrdinalIgnoreCase) && !searchContainedWords.Contains(word))
                            searchContainedWords.Add(word);
        });

        return new ServiceResponder<List<string>> { Data = searchContainedWords };
    }

    private bool IsContainedText(Product product, string text)
    {
        return product.ProductName.ToUpper().Contains(text.ToUpper())
            || product.ProductDetail.ToUpper().Contains(text.ToUpper());
    }

    public async Task<ServiceResponder<ProductsDTO>> GetProductsIncludeText(string text, int currentPageIndex)
    {
        const double numOfProductsPerPage = 2.0;
        int numOfPages = Convert.ToInt32(Math.Ceiling((await FindProductsBySearchText(text)).Count / numOfProductsPerPage));
        List<Product> products = await _dbContext.Products
            .Where(product => (product.ProductName.ToUpper().Contains(text.ToUpper())
                              || product.ProductDetail.ToUpper().Contains(text.ToUpper())
                              )
                              && product.isViewable
                              && !product.isDeleted)
            .Include(product => product.ProductVariants)
            .Include(product => product.Pictures)
            .Skip((currentPageIndex - 1) * Convert.ToInt32(numOfProductsPerPage))
            .Take(Convert.ToInt32(numOfProductsPerPage))
            .ToListAsync();
        return new ServiceResponder<ProductsDTO>()
        {
            Data = new ProductsDTO()
                    {
                        products = products,
                        currentPageIndex = currentPageIndex,
                        numOfPages = numOfPages
                    }
        };
    }

    private async Task<List<Product>> FindProductsBySearchText(string text) =>
        await _dbContext
            .Products
            .Where(product => (product.ProductName.ToUpper().Contains(text.ToUpper())
                              || product.ProductDetail.ToUpper().Contains(text.ToUpper())
                              )
                              && product.isViewable
                              && !product.isDeleted)
            .Include(product => product.ProductVariants)
            .ToListAsync();

    
    // public async Task<ServiceResponder<List<Product>>> SortProductsByPrice(List<Product> products, int num)
    // {
    //     return new ServiceResponder<List<Product>>
    //     {
    //         Data = (num >= 0 ) ? /*ascending*/
    //             await _dbContext
    //             .Products
    //             .Where(product => product.isViewable && !product.isDeleted
    //                               && products.Exists(paramProduct => paramProduct.ProductId == product.ProductId))
    //             .Include(product => product.ProductVariants
    //                                                   .Where(variant => variant.isViewable && !variant.isDeleted)
    //                                                   .OrderBy(variant => variant.price))
    //             .Include(product => product.Pictures)
    //             .ToListAsync()
    //             : await _dbContext/*Descending*/
    //                 .Products
    //                 .Where(product => product.isViewable && !product.isDeleted
    //                                                      && products.Exists(paramProduct => paramProduct.ProductId == product.ProductId))
    //                 .Include(product => product.ProductVariants
    //                     .Where(variant => variant.isViewable && !variant.isDeleted)
    //                     .OrderByDescending(variant => variant.price))
    //                 .Include(product => product.Pictures)
    //                 .ToListAsync()
    //     };
    // }
    //
    // public async Task<ServiceResponder<List<Product>>> SortProductsByAlphabet(List<Product> products, int num)
    // {
    //     return new ServiceResponder<List<Product>>
    //     {
    //         Data = (num >= 0 ) ? /*ascending*/
    //             await _dbContext
    //                 .Products
    //                 .Where(product => product.isViewable && !product.isDeleted
    //                                                      && products.Exists(paramProduct => paramProduct.ProductId == product.ProductId))
    //                 .Include(product => product.ProductVariants
    //                     .Where(variant => variant.isViewable && !variant.isDeleted)
    //                     .OrderBy(variant => variant.product.ProductName))
    //                 .Include(product => product.Pictures)
    //                 .ToListAsync()
    //             : await _dbContext/*Descending*/
    //                 .Products
    //                 .Where(product => product.isViewable && !product.isDeleted
    //                                                      && products.Exists(paramProduct => paramProduct.ProductId == product.ProductId))
    //                 .Include(product => product.ProductVariants
    //                     .Where(variant => variant.isViewable && !variant.isDeleted)
    //                     .OrderByDescending(variant => variant.product.ProductName))
    //                 .Include(product => product.Pictures)
    //                 .ToListAsync()
    //     };
    // }

    public async Task<ServiceResponder<List<Product>>> GetProductsByCategoryUrl(string categoryUrl, int num = 0, string key = "price")
    {
        List<Product> Data = new();
        if (key.ToLower().Equals("price") && num >= 0)
            Data =
                await _dbContext.Products
                    .Where(product => product.ProductCategory.CategoryAccessUrl.ToUpper().Equals(categoryUrl.ToUpper())
                                      && product.isViewable
                                      && !product.isDeleted)
                    .Include(product =>
                        product.ProductVariants
                            .Where(variant => variant.isViewable && !variant.isDeleted)
                            .OrderBy(variant => variant.price))
                    .Include(p => p.Pictures)
                    .ToListAsync();
        else if(key.ToLower().Equals("price") && num < 0)
            Data =
                await _dbContext.Products
                    .Where(product => product.ProductCategory.CategoryAccessUrl.ToUpper().Equals(categoryUrl.ToUpper())
                                      && product.isViewable
                                      && !product.isDeleted)
                    .Include(product =>
                        product.ProductVariants
                            .Where(variant => variant.isViewable && !variant.isDeleted)
                            .OrderByDescending(variant => variant.price))
                    .Include(p => p.Pictures)
                    .ToListAsync();
        else if(!key.ToLower().Equals("price") && num >= 0)
            Data =
                await _dbContext.Products
                    .Where(product => product.ProductCategory.CategoryAccessUrl.ToUpper().Equals(categoryUrl.ToUpper())
                                      && product.isViewable
                                      && !product.isDeleted)
                    .Include(product =>
                        product.ProductVariants
                            .Where(variant => variant.isViewable && !variant.isDeleted)
                            .OrderBy(variant => variant.product.ProductName))
                    .Include(p => p.Pictures)
                    .ToListAsync();
        else
            Data =
                await _dbContext.Products
                    .Where(product => product.ProductCategory.CategoryAccessUrl.ToUpper().Equals(categoryUrl.ToUpper())
                                      && product.isViewable
                                      && !product.isDeleted)
                    .Include(product =>
                        product.ProductVariants
                            .Where(variant => variant.isViewable && !variant.isDeleted)
                            .OrderByDescending(variant => variant.product.ProductName))
                    .Include(p => p.Pictures)
                    .ToListAsync();
        
        return new ServiceResponder<List<Product>>{Data = Data};
    }

    public async Task<ServiceResponder<Product>> UpdateProduct(Product product)
    {
        Product? foundDatabaseProduct = await _dbContext
                                              .Products
                                              .Include(p => p.Pictures)
                                              .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
        if (foundDatabaseProduct != null)
        {
            foundDatabaseProduct.isViewable = product.isViewable;
            foundDatabaseProduct.ProductName = product.ProductName;
            foundDatabaseProduct.IsTopProduct = product.IsTopProduct;
            foundDatabaseProduct.ProductDetail = product.ProductDetail;
            foundDatabaseProduct.ProductImageUrl = product.ProductImageUrl;
            foundDatabaseProduct.ProductCategoryId = product.ProductCategoryId;
            
            _dbContext.Pictures.RemoveRange(foundDatabaseProduct.Pictures);
            foundDatabaseProduct.Pictures = product.Pictures;
            
            foreach (ProductVariant pv in product.ProductVariants)
            {
                ProductVariant? foundDbVariant = await _dbContext
                                                .ProductVariants
                                                .SingleOrDefaultAsync(variant => variant.productId == pv.productId &&
                                                                                 variant.productVarietyId == pv.productVarietyId);
                if (foundDbVariant != null)
                {
                    foundDbVariant.price = pv.price;
                    foundDbVariant.isDeleted = pv.isDeleted;
                    foundDbVariant.isViewable = pv.isViewable;
                    foundDbVariant.initialPrice = pv.initialPrice;
                    foundDbVariant.productVarietyId = pv.productVarietyId;
                }
                else
                {
                    pv.productVariety = null;
                    _dbContext.ProductVariants.Add(pv);
                }
            }
            await _dbContext.SaveChangesAsync();
            return new ServiceResponder<Product>{Data = product, Message = "Successfully Delete The Product with Given Id", IsSuccess = true};
        }
        return new ServiceResponder<Product>{Message = "Couldn't Find Product with Given Id", IsSuccess = false};
    }

    public async Task<ServiceResponder<bool>> DeleteProductById(int productId)
    {
        Product? foundDatabaseProduct = await _dbContext.Products.FindAsync(productId);
        if (foundDatabaseProduct != null)
        {
            foundDatabaseProduct.isDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new ServiceResponder<bool>{Data = true, Message = "Successfully Delete The Product with Given Id", IsSuccess = true};
        }
        return new ServiceResponder<bool>{Data = false, Message = "Couldn't Find Product with Given Id", IsSuccess = false};
    }

    public async Task<ServiceResponder<Product>> CreateProduct(Product product)
    {
        product.ProductVariants.ForEach(variant => variant.productVariety = null);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return new ServiceResponder<Product> { Data = product };
    }

    public async Task<ServiceResponder<List<Product>>> GetTopProducts() => new()
    {
        Data = await _dbContext.Products
            .Where(product => product.IsTopProduct && !product.isDeleted && product.isViewable)
            .Include(product => product.ProductVariants.Where(variant => !variant.isDeleted && variant.isViewable))
            .Include(product => product.Pictures)
            .ToListAsync()
    };
    
}