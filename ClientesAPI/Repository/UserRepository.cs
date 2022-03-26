using AutoMapper;
using ClientesAPI.Data;
using ClientesAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClientesAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            _db = db;

        }


        public async Task<string> Login(string userName, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync
                            (x=>x.UserName.ToLower().Equals(userName.ToLower()));

            if (user == null)
            {
                return "Nouser";


            }
            else if (!VerifyPasswordHash(password, user.PasswordHash,user.PasswordSalt))
            {
                return "wrong password";

            }
            else
            {
                return CreateToken(user);
            }
        }

        public async Task<int> Register(User user, string password)
        {
            try
            {

                if (await UserExists(user.UserName))
                {
                    return -1;
                }

                CreatePasswordHash(password, out byte[] PasswordHash, out byte[] PasswordSalt);

                user.PasswordHash = PasswordHash;
                user.PasswordSalt = PasswordSalt;

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return user.UserId;
            }
            catch (Exception)
            {
                return -500;
               
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await _db.Users.AnyAsync(x => x.UserName.ToLower().Equals(userName.ToLower()))) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /*metodo encargado de encriptar los password*/

        private void CreatePasswordHash
            (string password , out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var Hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = Hmac.Key;
                passwordHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        
        }

        public bool VerifyPasswordHash(string password, byte[] passworshash, byte[] paswordSalt) 
        {
            using (var Hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt))
            {
                var computedHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passworshash[i])
                    {
                        return false;

                    }               
                }
                return true;
            }
        
        }

        private string CreateToken(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
     
        }
    }
}
