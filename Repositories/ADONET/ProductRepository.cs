using System.Data;
using MySql.Data.MySqlClient;
using TrendyProducts.Repositories.Interfaces;
using System.Collections.Generic;
using System;
using TrendyProducts.Helpers;
using TrendyProducts.Models;

namespace TrendyProducts.Repositories.ADONET
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbHelper _db;
        public ProductRepository(DbHelper db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetProducts(string? category = null, int page = 1, int pageSize = 20, string? sort = null)
        {
            var list = new List<Product>();
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();

            var offset = (page - 1) * pageSize;
            cmd.CommandText = @"SELECT p.* FROM products p
                                JOIN categories c ON p.category_id = c.id
                                WHERE (@category IS NULL OR c.slug = @category)
                                ORDER BY CASE WHEN @sort = 'popularity' THEN p.sales_count END DESC, p.id
                                LIMIT @limit OFFSET @offset;";

            var pCategory = cmd.CreateParameter(); pCategory.ParameterName = "@category"; pCategory.Value = (object)category ?? DBNull.Value; cmd.Parameters.Add(pCategory);
            var pLimit = cmd.CreateParameter(); pLimit.ParameterName = "@limit"; pLimit.Value = pageSize; cmd.Parameters.Add(pLimit);
            var pOffset = cmd.CreateParameter(); pOffset.ParameterName = "@offset"; pOffset.Value = offset; cmd.Parameters.Add(pOffset);
            var pSort = cmd.CreateParameter(); pSort.ParameterName = "@sort"; pSort.Value = (object)sort ?? DBNull.Value; cmd.Parameters.Add(pSort);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public Product GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM products WHERE id = @id LIMIT 1;";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return Map(r);
            return null;
        }

        public Product GetBySlug(string slug)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM products WHERE slug = @slug LIMIT 1;";
            var p = cmd.CreateParameter(); p.ParameterName = "@slug"; p.Value = slug; cmd.Parameters.Add(p);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return Map(r);
            return null;
        }

        public IEnumerable<Product> GetTopSellingByCategory(int categoryId, int limit)
        {
            var list = new List<Product>();
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM products WHERE category_id = @cid ORDER BY sales_count DESC LIMIT @limit;";
            var pCid = cmd.CreateParameter(); pCid.ParameterName = "@cid"; pCid.Value = categoryId; cmd.Parameters.Add(pCid);
            var pLimit = cmd.CreateParameter(); pLimit.ParameterName = "@limit"; pLimit.Value = limit; cmd.Parameters.Add(pLimit);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public int Create(Product product)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO products (title,slug,description,brand,price,currency,image_url,category_id,stock,sales_count,rating)
                                VALUES (@title,@slug,@description,@brand,@price,@currency,@image_url,@category_id,@stock,@sales_count,@rating);
                                SELECT LAST_INSERT_ID();";

            AddParameters(cmd, product);
            var id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }

        public void Update(Product product)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE products SET title=@title,slug=@slug,description=@description,brand=@brand,price=@price,currency=@currency,image_url=@image_url,category_id=@category_id,stock=@stock,sales_count=@sales_count,rating=@rating WHERE id=@id;";
            var pid = cmd.CreateParameter(); pid.ParameterName = "@id"; pid.Value = product.Id; cmd.Parameters.Add(pid);
            AddParameters(cmd, product);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM products WHERE id = @id;";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            cmd.ExecuteNonQuery();
        }

        private Product Map(IDataRecord r)
        {
            return new Product
            {
                Id = Convert.ToInt32(r["id"]),
                Title = r["title"].ToString(),
                Slug = r["slug"].ToString(),
                Description = r["description"].ToString(),
                Brand = r["brand"].ToString(),
                Price = Convert.ToDecimal(r["price"]),
                Currency = r["currency"].ToString(),
                ImageUrl = r["image_url"].ToString(),
                CategoryId = Convert.ToInt32(r["category_id"]),
                Stock = Convert.ToInt32(r["stock"]),
                SalesCount = Convert.ToInt32(r["sales_count"]),
                Rating = Convert.ToDecimal(r["rating"])
            };
        }

        private void AddParameters(IDbCommand cmd, Product p)
        {
            void Add(string name, object val) { var pa = cmd.CreateParameter(); pa.ParameterName = name; pa.Value = val ?? DBNull.Value; cmd.Parameters.Add(pa); }

            Add("@title", p.Title);
            Add("@slug", p.Slug);
            Add("@description", p.Description);
            Add("@brand", p.Brand);
            Add("@price", p.Price);
            Add("@currency", p.Currency ?? "INR");
            Add("@image_url", p.ImageUrl);
            Add("@category_id", p.CategoryId);
            Add("@stock", p.Stock);
            Add("@sales_count", p.SalesCount);
            Add("@rating", p.Rating);
        }
        public IEnumerable<Product> SearchProducts(string term, int page, int pageSize)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
        SELECT id, title, slug, description, brand, price, currency,
               image_url, category_id, stock, sales_count, rating
        FROM products
        WHERE title       LIKE @term
           OR slug        LIKE @term
           OR description LIKE @term
        ORDER BY sales_count DESC
        LIMIT @limit OFFSET @offset;
    ";

            var termParam = cmd.CreateParameter();
            termParam.ParameterName = "@term";
            termParam.Value = "%" + term + "%";
            cmd.Parameters.Add(termParam);

            var limitParam = cmd.CreateParameter();
            limitParam.ParameterName = "@limit";
            limitParam.Value = pageSize;
            cmd.Parameters.Add(limitParam);

            var offsetParam = cmd.CreateParameter();
            offsetParam.ParameterName = "@offset";
            offsetParam.Value = (page - 1) * pageSize;
            cmd.Parameters.Add(offsetParam);

            using var reader = cmd.ExecuteReader();
            var list = new List<Product>();

            while (reader.Read())
            {
                list.Add(Map(reader));   // ya jo bhi tumhaara existing mapper hai
            }

            return list;
        }
    }
}