using TrendyProducts.Helpers;
using TrendyProducts.Models;
using TrendyProducts.Repositories.Interfaces;


namespace TrendyProducts.Repositories.ADONET
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbHelper _db;

        public CustomerRepository(DbHelper db)
        {
            _db = db;
        }

        public int Insert(Customer customer)
        {
            string sql = @"
            INSERT INTO Customers
            (UserId, FullName, Email, Phone, AddressLine, City, State, Pincode)
            OUTPUT INSERTED.Id
            VALUES
            (@UserId, @FullName, @Email, @Phone, @AddressLine, @City, @State, @Pincode)";

            return _db.ExecuteScalar<int>(sql, customer);
        }
    }

}
