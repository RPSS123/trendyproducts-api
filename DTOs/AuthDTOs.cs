using System;
namespace TrendyProducts.DTOs
{
	public class AuthDTOs
    {
		
    public record LoginDto(string Name,string Email, string Password);
    public record RegisterDto(string Name, string Email, string Password);
    public record UserDto(int Id, string Name, string Email);
    public record AuthResponse(string Token, int ExpiresInMinutes, UserDto User
 );
    
}
}

