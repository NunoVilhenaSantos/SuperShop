using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Models;

namespace SuperShop.Web.Helpers;

public class UserHelper : IUserHelper
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;


    public UserHelper(
        UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<IdentityResult> AddUserAsync(User user,
        string password)
    {
        return await _userManager.CreateAsync(user, password);
    }


    public Task CheckRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
            false);
    }

    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }


    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }


    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }
}