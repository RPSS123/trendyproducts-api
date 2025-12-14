using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace TrendyProducts.Helpers
{
    public class DbHelper
   {
        private readonly IConfiguration _configuration;
    private readonly string _connString;

    public DbHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _connString = _configuration.GetConnectionString("Default");
    }

    public IDbConnection GetConnection()
    {
        var conn = new MySqlConnection(_connString);
        conn.Open();
        return conn;
    }
}
}