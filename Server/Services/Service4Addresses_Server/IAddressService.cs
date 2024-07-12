namespace Electronics_Store.Server.Services.Service4Addresses_Server;

public interface IAddressService
{
    Task<ServiceResponder<Address>> GetAddress();
    Task<ServiceResponder<Address>> AddAddress(Address address);
    Task<ServiceResponder<Address>> UpdateAddress(Address address);
    
    Task<ServiceResponder<Address>> AddOrUpdateAddress(Address address);
}