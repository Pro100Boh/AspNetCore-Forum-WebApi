using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Infrastructure
{
    public static class Role
    {
        public const string Admin = "Admin";

        public const string Moder = "Moder";

        public const string User = "User";

        public const string AdminOrModer = "Admin,Moder";
    }

    /*
    public enum Role
    {
        Admin = 1, Moder = 2, User = 3,
    }*/

    //$"{Role.Admin},{Role.Moder}"
}
