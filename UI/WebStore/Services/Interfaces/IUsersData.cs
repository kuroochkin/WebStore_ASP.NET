using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Interfaces
{
    public interface IUsersData
    {
        IEnumerable<User> GetUsers();
    }
}
