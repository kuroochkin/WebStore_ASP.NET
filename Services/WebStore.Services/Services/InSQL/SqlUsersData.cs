using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlUsersData : IUsersData
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<SqlEmployeesData> _Logger;

        public SqlUsersData(WebStoreDB db, ILogger<SqlEmployeesData> Logger)
        {
            _db = db;
            _Logger = Logger;
        }
        public IEnumerable<User> GetUsers() => _db.Users.AsEnumerable();

        
    }
}
