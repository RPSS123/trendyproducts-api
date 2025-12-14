using System;
using System.Collections.Generic;
using System.Data;
using TrendyProducts.Helpers;
using TrendyProducts.Models;
using TrendyProducts.Repositories.Interfaces;

namespace TrendyProducts.Repositories.ADONET
{
    public class CartRepository : ICartRepository
    {
        private readonly DbHelper _db;
        public CartRepository(DbHelper db) => _db = db;

        public int AddOrUpdate(int userId, int productId, int quantity)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO cart_items (user_id, product_id, quantity)
                VALUES (@userId, @productId, @qty)
                ON DUPLICATE KEY UPDATE quantity = quantity + @qty;
                SELECT LAST_INSERT_ID();";

            var pUser = cmd.CreateParameter();
            pUser.ParameterName = "@userId";
            pUser.Value = userId;
            cmd.Parameters.Add(pUser);

            var pProd = cmd.CreateParameter();
            pProd.ParameterName = "@productId";
            pProd.Value = productId;
            cmd.Parameters.Add(pProd);

            var pQty = cmd.CreateParameter();
            pQty.ParameterName = "@qty";
            pQty.Value = quantity;
            cmd.Parameters.Add(pQty);

            var id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }

        public IEnumerable<CartItem> GetCart(int userId)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                SELECT id, user_id, product_id, quantity
                FROM cart_items
                WHERE user_id = @userId";

            var pUser = cmd.CreateParameter();
            pUser.ParameterName = "@userId";
            pUser.Value = userId;
            cmd.Parameters.Add(pUser);

            using var reader = cmd.ExecuteReader();
            var list = new List<CartItem>();

            while (reader.Read())
            {
                list.Add(MapCartItem(reader));
            }

            return list;
        }

        private CartItem MapCartItem(IDataRecord r) =>
            new CartItem
            {
                Id = r.GetInt32(r.GetOrdinal("id")),
                UserId = r.GetInt32(r.GetOrdinal("user_id")),
                ProductId = r.GetInt32(r.GetOrdinal("product_id")),
                Quantity = r.GetInt32(r.GetOrdinal("quantity"))
            };
    }
}
