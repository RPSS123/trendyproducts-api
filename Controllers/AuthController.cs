using Microsoft.AspNetCore.Mvc;
using TrendyProducts.DTOs;
using TrendyProducts.Helpers;
using System.Data;
using TrendyProducts.Repositories.ADONET;
using TrendyProducts.Utilities;

namespace TrendyProducts.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DbHelper _db;
    private readonly JwtService _jwt;
    public AuthController(DbHelper db, JwtService jwt)
    {
        _db = db; _jwt = jwt;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthDTOs.RegisterDto dto)
    {
        using var conn = _db.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(1) FROM users WHERE email=@email;";
        var pe = cmd.CreateParameter(); pe.ParameterName = "@email"; pe.Value = dto.Email; cmd.Parameters.Add(pe);
        var exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        if (exists) return BadRequest("Email already registered");

        cmd.CommandText = "INSERT INTO users (name,email,password_hash,role) VALUES(@name,@email,@hash,@role); SELECT LAST_INSERT_ID();";
        var pn = cmd.CreateParameter(); pn.ParameterName = "@name"; pn.Value = dto.Name; cmd.Parameters.Add(pn);
        var ph = cmd.CreateParameter(); ph.ParameterName = "@hash"; ph.Value = PasswordHasher.Hash(dto.Password); cmd.Parameters.Add(ph);
        var pr = cmd.CreateParameter(); pr.ParameterName = "@role"; pr.Value = "buyer"; cmd.Parameters.Add(pr);
        var id = Convert.ToInt32(cmd.ExecuteScalar());
        var token = _jwt.GenerateToken(id, "buyer");
        var userDto = new AuthDTOs.UserDto(id, dto.Name, dto.Email);
        return Ok(new AuthDTOs.AuthResponse(token, 120, userDto));
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthDTOs.LoginDto dto)
    {
        using var conn = _db.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, password_hash, role FROM users WHERE email=@email LIMIT 1;";
        var pe = cmd.CreateParameter(); pe.ParameterName = "@email"; pe.Value = dto.Email; cmd.Parameters.Add(pe);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return Unauthorized("Invalid credentials");
        var id = Convert.ToInt32(r["id"]);
        var hash = r["password_hash"].ToString();
        var role = r["role"].ToString();
        if (!PasswordHasher.Verify(dto.Password, hash)) return Unauthorized("Invalid credentials");
        var token = _jwt.GenerateToken(id, role);
        var userDto = new AuthDTOs.UserDto(id, dto.Name, dto.Email);
        return Ok(new AuthDTOs.AuthResponse(token, 120, userDto));
    }
}

