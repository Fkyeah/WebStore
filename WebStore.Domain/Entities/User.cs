using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities
{
    public class User : IdentityUser
    {
        public const string Administrator = "Administrator";
        public const string DefaultAdminPassword = "Admin_123";

    }
}
