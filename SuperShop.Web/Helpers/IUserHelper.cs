﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);


        Task<User> GetUserByIdAsync(string id);


        Task<IdentityResult> AddUserAsync(
            User user, string password);


        Task CheckRoleAsync(string roleName);
    }
}