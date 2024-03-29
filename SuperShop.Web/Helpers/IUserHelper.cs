﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers;

public interface IUserHelper
{
    Task<User> GetUserByEmailAsync(string email);


    Task<User> GetUserByIdAsync(string id);


    Task<IdentityResult> AddUserAsync(User user, string password);


    Task CheckRoleAsync(string roleName);


    Task<SignInResult> LoginAsync(LoginViewModel model);


    Task LogOutAsync();


    Task<IdentityResult> UpdateUserAsync(User user);


    Task<IdentityResult> ChangePasswordAsync(
        User user, string oldPassword, string newPassword);


    Task AddUserToRoleAsync(User user, string roleName);


    Task RemoveUserFromRoleAsync(User user, string roleName);


    Task<bool> IsUserInRoleAsync(User user, string roleName);


    Task<SignInResult> ValidatePasswordAsync(User user, string password);


    // Task<string> GenerateEmailConfirmationTokenAsync(User user);


    // Task<IdentityResult> ConfirmEmailAsync(User user, string token);
}