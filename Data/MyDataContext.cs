using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkSystem.Data
{
    public class MyDataContext(DbContextOptions<MyDataContext> options) : IdentityDbContext<User>(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {

            const string ADMIN_USER_ID = "22e40406-8a9d-2d82-912c-5d6a640ee696";
            const string ADMIN_ROLE_ID = "b421e928-0613-9ebd-a64c-f10b6a706e73";

            builder.Entity<IdentityRole>(role =>
            {
                role.HasData(new IdentityRole()
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "admin",
                    NormalizedName = "ADMIN"

                }, new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "manger",
                    NormalizedName = "MANGER"
                }, new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "user",
                    NormalizedName = "USER"
                }

                );
            });

            builder.Entity<User>(user =>
            {
                var x = new User
                {
                    Id = ADMIN_USER_ID,
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                };
                PasswordHasher<User> p = new();
                user.HasData(new User()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    PasswordHash = p.HashPassword(x, "123456Aa*")
                });

            });

            builder.Entity<IdentityUserRole<string>>(i => i.HasData(new IdentityUserRole<string>()
            {
                UserId = ADMIN_USER_ID,
                RoleId = ADMIN_ROLE_ID
            }));

            base.OnModelCreating(builder);
        }

        public DbSet<Prodcut> Prodcuts { get; set; }
    }
}