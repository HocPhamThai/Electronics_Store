namespace Electronics_Store.Client.Services.Service4Addresses_Client;

public interface IAddressService
{
    Task<Address> GetAddress();
    Task<Address> AddOrUpdateAddress(Address address);
}