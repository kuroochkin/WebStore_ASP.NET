using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient
    {
        public EmployeesClient(HttpClient Client) : base(Client, "api/employees")
        {
        }
    }
}
