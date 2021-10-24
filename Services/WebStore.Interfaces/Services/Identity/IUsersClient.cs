using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services.Identity
{
    public interface IUsersClient :
        IUserStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserLoginStore<User>,
        IUserTwoFactorStore<User>,
        IUserClaimStore<User>
    {
        
    }
}
