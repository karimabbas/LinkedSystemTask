using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using LinkSystem.Data;
using LinkSystem.Helpers;
using LinkSystem.Models;
using LinkSystem.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkSystem.Repository
{
    public class UserRepository(MyDataContext myDataContext, UserManager<User> userManager) : IUserService
    {
        private readonly MyDataContext _myDataContext = myDataContext;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<User> GetUser(string email)
        {
            return await _myDataContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _myDataContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<bool> MakeAmdin(string email)
        {
            var user = await GetUser(email);
            var Exist_Role = await _myDataContext.UserRoles.FirstOrDefaultAsync(x => x.UserId.ToString() == user.Id.ToString());

            if (Exist_Role is not null)
            {
                var prviousRole = _myDataContext.Roles.Where(x => x.Id == Exist_Role.RoleId).Select(e => e.Name).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(user, prviousRole);
                await _userManager.AddToRoleAsync(user, UserRole.Admin);
                _myDataContext.SaveChanges();
                return true;

            }
            return false;

        }

        public async Task<bool> MakeManger(string email)
        {
            var user = await GetUser(email);
            var Exist_Role = await _myDataContext.UserRoles.FirstOrDefaultAsync(x => x.UserId.ToString() == user.Id.ToString());

            if (Exist_Role is not null)
            {
                var prviousRole = _myDataContext.Roles.Where(x => x.Id == Exist_Role.RoleId).Select(e => e.Name).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(user, prviousRole);
                await _userManager.AddToRoleAsync(user, UserRole.Manger);
                _myDataContext.SaveChanges();
                return true;
            }
            return false;

        }



        public string HashedPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: password,
                   salt: salt,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 256 / 8
               )).ToString();
        }


    }
}