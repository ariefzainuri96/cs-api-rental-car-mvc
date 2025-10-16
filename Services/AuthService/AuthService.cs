using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Data;
using cs_api_rental_car_mvc.Dtos;
using cs_api_rental_car_mvc.Dtos.Request;
using cs_api_rental_car_mvc.Dtos.Response;
using cs_api_rental_car_mvc.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cs_api_rental_car_mvc.Services.AuthService
{
    public class AuthService : IAuthService
    {

        private readonly RentalCarDbContext context;
        private readonly IConfiguration configuration;

        public AuthService(RentalCarDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<(HttpError?, LoginResponseDto)> LoginAsync(LoginRequestDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return (new HttpError("Invalid Email or Password!") { StatusCode = StatusCodes.Status404NotFound }, new LoginResponseDto());
            }

            var hashedPassword = new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.Password, request.Password);

            if (hashedPassword == PasswordVerificationResult.Failed)
            {
                return (new HttpError("Invalid Email or Password!") { StatusCode = StatusCodes.Status404NotFound }, new LoginResponseDto());
            }

            var token = CreateToken(user);

            return (null, new LoginResponseDto { Email = user.Email, Role = user.Role, Name = user.Name, Token = token });
        }

        public async Task<HttpError?> RegisterAsync(RegisterRequestDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return new HttpError("User Already Exist!") { StatusCode = StatusCodes.Status409Conflict };
            }

            var hashedPassword = new PasswordHasher<RegisterRequestDto>().HashPassword(request, request.Password);

            await context.Users.AddAsync(new UserEntity { Name = request.Name, Email = request.Email, Password = hashedPassword, Role = request.Role });
            await context.SaveChangesAsync();

            return null;
        }

        private string CreateToken(UserEntity user)
        {
            var claims = new List<Claim>{
            new Claim("name", user.Name),
            new Claim("email", user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer")!,
                audience: configuration.GetValue<string>("AppSettings:Audience")!,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}