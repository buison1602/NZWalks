using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Create claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Tạo khóa đối xứng: là SECRET_KEY trong công thức tạo chữ ký.
            // Jwt:Key chỉ là một chuỗi string đơn thuần, ví dụ "my-secret-key-12345" trong appsettings.json.
            // Nhưng để ký JWT, thư viện cần một đối tượng dạng SecurityKey, chứ không phải một string.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Tạo chữ ký số
            // Đây là một đối tượng "đóng gói" gồm:
            //  - key: bí mật để ký.
            //  - algorithm: thuật toán dùng để ký(ví dụ: HMAC SHA256).
            // SigningCredentials là thông tin mô tả cách tạo chữ ký, không phải chữ ký.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // Tự động tạo token JWT
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
