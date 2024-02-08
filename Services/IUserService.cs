using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Models;

namespace LinkSystem.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string email);
        string HashedPassword(string password);
        Task<List<User>> GetUsers();
        Task<bool> MakeAmdin(string email);
        Task<bool> MakeManger(string email);
    }
}