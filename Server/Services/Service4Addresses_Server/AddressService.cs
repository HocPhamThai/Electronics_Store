namespace Electronics_Store.Server.Services.Service4Addresses_Server;

public class AddressService: IAddressService
{
    private readonly ElectronicsStoreDbContext _dbContext;
    private readonly IAuthenticationService _authenticationService;

    public AddressService(ElectronicsStoreDbContext dbContext, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
    }

    public async Task<ServiceResponder<Address>> GetAddress()
    {
        int userId = _authenticationService.GetUserId();
        return new ServiceResponder<Address>
        {
            Data = await _dbContext.Addresses
                .FirstOrDefaultAsync(a => a.userId == userId)
        };
    }

    public async Task<ServiceResponder<Address>> AddAddress(Address address)
    {
        ServiceResponder<Address> response = new ServiceResponder<Address>();
            address.userId = _authenticationService.GetUserId();
            _dbContext.Addresses.Add(address);
            response.IsSuccess = true;
            response.Data = address;
        await _dbContext.SaveChangesAsync();
        return response;
    }

    public async Task<ServiceResponder<Address>> UpdateAddress(Address address)
    {
        ServiceResponder<Address> response = new ServiceResponder<Address>();
        Address? databaseAddress = (await GetAddress()).Data;
        if (databaseAddress != null)
        {
            databaseAddress.country = address.country;
            databaseAddress.city = address.city;
            databaseAddress.district = address.district;
            databaseAddress.street = address.street;
            
            databaseAddress.zip = address.zip;
            
            databaseAddress.firstName = address.firstName;
            databaseAddress.lastName = address.lastName;
            
            response.Data = databaseAddress;
            response.IsSuccess = true;
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "Can't find the address to update";
        }
        return response;
    }

    public async Task<ServiceResponder<Address>> AddOrUpdateAddress(Address address)
    {
        Address? databaseAddress = (await GetAddress()).Data;
        if (databaseAddress != null)
            return await UpdateAddress(address);
        return await AddAddress(address);
    }
}