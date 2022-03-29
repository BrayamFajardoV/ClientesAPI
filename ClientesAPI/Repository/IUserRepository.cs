using ClientesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Repository
{
    public interface IUserRepository
    {

        Task<string> Register(User user ,string password);
        Task<string> Login(string userName ,string password);
        Task<bool> UserExists(string userName);


    }
}
