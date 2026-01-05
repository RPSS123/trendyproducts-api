using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using Dapper;

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
            var conn = new SqlConnection(_connString);
            conn.Open();
            return conn;
        }

        public T ExecuteScalar<T>(string sql, object param = null)
        {
            using var conn = GetConnection();
            return conn.ExecuteScalar<T>(sql, param);
        }

        public int Execute(string sql, object param = null)
        {
            using var conn = GetConnection();
            return conn.Execute(sql, param);
        }

        public IEnumerable<T> Query<T>(
    string sql,
    object param = null,
    CommandType commandType = CommandType.Text)
        {
            using var conn = GetConnection();
            return conn.Query<T>(sql, param, commandType: commandType);
        }

        public T QuerySingle<T>(
            string sql,
            object param = null,
            CommandType commandType = CommandType.Text)
        {
            using var conn = GetConnection();
            return conn.QuerySingle<T>(sql, param, commandType: commandType);
        }


    }
}