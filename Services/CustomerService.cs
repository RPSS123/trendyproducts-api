using TrendyProducts.DTOs;
using TrendyProducts.Helpers;
using TrendyProducts.Models;
using TrendyProducts.Repositories.Interfaces;
using TrendyProducts.Services.Interfaces;

namespace TrendyProducts.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public int SaveCustomer(CustomerDTO dto)
        {
            var customer = new Customer
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                AddressLine = dto.AddressLine,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode
            };

            return _repo.Insert(customer);
        }
    }


}
