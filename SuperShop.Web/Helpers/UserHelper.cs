using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;
using System.Threading.Tasks;

namespace SuperShop.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;



        public UserHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }




        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);

            // throw new System.NotImplementedException();
        }





        public Task CheckRoleAsync(string roleName)
        {

            throw new System.NotImplementedException();
        }



        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);

            // throw new System.NotImplementedException();
        }




        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
            // throw new System.NotImplementedException();
        }





    }
}
