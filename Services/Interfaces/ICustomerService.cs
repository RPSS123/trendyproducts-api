using TrendyProducts.DTOs;

namespace TrendyProducts.Services.Interfaces
{
    public interface ICustomerService
    {
        int SaveCustomer(CustomerDTO dto);
    }

}
