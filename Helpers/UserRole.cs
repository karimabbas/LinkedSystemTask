using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSystem.Helpers
{
    public static class UserRole
    {
        public const string Admin = "Admin";
        public const string Manger = "Manger";
        public const string User = "User";
    }

    public class CustomCalimTypes
    {
        public const string Permission = "LinkSystem.Permission";
    }

    public static class Permissions{
        public const string ProductCRUD="Product.CRUD";
        public const string SetUpRoles="SetUp.Roles";

    }
}