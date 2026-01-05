using System.Collections.Generic;
using System.Data;
using TrendyProducts.Helpers;
using TrendyProducts.Models;
using TrendyProducts.Repositories.Interfaces;

namespace TrendKart.API.Repositories.ADONET
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbHelper _db;
        public CategoryRepository(DbHelper db) { _db = db; }

        public IEnumerable<Category> GetAll()
        {
            var list = new List<Category>();
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM categories ORDER BY name;";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Category
                {
                    Id = Convert.ToInt32(r["id"]),
                    Name = r["name"].ToString(),
                    Slug = r["slug"].ToString(),
                    Description = r["description"].ToString(),
                    ImageUrl = r["image_url"].ToString(),
                    ParentId = r["parent_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["parent_id"])
                });
            }
            return list;
        }

        public Category GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM categories WHERE id=@id;";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return new Category { Id = Convert.ToInt32(r["id"]), Name = r["name"].ToString(), Slug = r["slug"].ToString(), Description = r["description"].ToString(), ImageUrl = r["image_url"].ToString() };
            return null;
        }

        public Category GetBySlug(string slug)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM categories WHERE slug=@slug;";
            var p = cmd.CreateParameter(); p.ParameterName = "@slug"; p.Value = slug; cmd.Parameters.Add(p);
            using var r = cmd.ExecuteReader();
            if (r.Read()) return new Category { Id = Convert.ToInt32(r["id"]), Name = r["name"].ToString(), Slug = r["slug"].ToString(), Description = r["description"].ToString(), ImageUrl = r["image_url"].ToString() };
            return null;
        }

        public int Create(Category category)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO categories (name,slug,description,parent_id) VALUES(@name,@slug,@desc,@pid); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = category.Name; cmd.Parameters.Add(p1);
            var p2 = cmd.CreateParameter(); p2.ParameterName = "@slug"; p2.Value = category.Slug; cmd.Parameters.Add(p2);
            var p3 = cmd.CreateParameter(); p3.ParameterName = "@desc"; p3.Value = category.Description ?? (object)DBNull.Value; cmd.Parameters.Add(p3);
            var p4 = cmd.CreateParameter(); p4.ParameterName = "@pid"; p4.Value = category.ParentId ?? (object)DBNull.Value; cmd.Parameters.Add(p4);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(Category category)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE categories SET name=@name,slug=@slug,description=@desc,parent_id=@pid WHERE id=@id;";
            var pid = cmd.CreateParameter(); pid.ParameterName = "@id"; pid.Value = category.Id; cmd.Parameters.Add(pid);
            var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = category.Name; cmd.Parameters.Add(p1);
            var p2 = cmd.CreateParameter(); p2.ParameterName = "@slug"; p2.Value = category.Slug; cmd.Parameters.Add(p2);
            var p3 = cmd.CreateParameter(); p3.ParameterName = "@desc"; p3.Value = category.Description ?? (object)DBNull.Value; cmd.Parameters.Add(p3);
            var p4 = cmd.CreateParameter(); p4.ParameterName = "@pid"; p4.Value = category.ParentId ?? (object)DBNull.Value; cmd.Parameters.Add(p4);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM categories WHERE id=@id;";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            cmd.ExecuteNonQuery();
        }
    }
}