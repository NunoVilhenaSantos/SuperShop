using Microsoft.AspNetCore.Identity;

namespace SuperShop.Web.Data.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }
    }
}