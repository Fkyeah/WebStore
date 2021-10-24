using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services.Identity
{
    public interface IRolesClient : IRoleStore<Role>
    {
        
    }
}
