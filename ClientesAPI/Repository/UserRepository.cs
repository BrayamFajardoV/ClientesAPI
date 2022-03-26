using AutoMapper;
using ClientesAPI.Data;
using ClientesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        //protected IMapper _mapper;
        public UserRepository(ApplicationDbContext db /*, IMapper mapper*/)
        {
            //_mapper = mapper;
            _db = db;

        }


        public Task<string> Login(string userName, string password)
        {
            throw new NotImplementedException();
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
    }
}
