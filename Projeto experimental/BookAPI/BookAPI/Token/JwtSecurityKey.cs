namespace BookAPI.Token;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtSecurityKey
{
	public static SymmetricSecurityKey Create(string secret)
	{
		return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
	}
}
