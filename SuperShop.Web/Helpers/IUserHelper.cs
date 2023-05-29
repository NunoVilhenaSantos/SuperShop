using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SuperShop.Web.Helpers
{
    public interface IUserHelper
    {
        Task<Data.Entity.User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(Data.Entity.User user, string password);

        Task CheckRoleAsync(string roleName);
    }
}
