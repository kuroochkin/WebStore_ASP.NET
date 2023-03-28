using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData // клиент прикидывается службой
    {
        public EmployeesClient(HttpClient Client) : base(Client, WebApiAddresses.Employees)
        {
        }

        public int Add(Employee employee)
        {
            var response = Post(Address, employee);
            var added_employee = response.Content.ReadFromJsonAsync<Employee>().Result;

            if (added_employee is null)
                return -1;

            var id = added_employee.Id;
            employee.Id = id;

            return id;
        }

        public bool Delete(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return success;
        }

        public bool Edit(Employee employee)
        {
            var response = Put(Address, employee);
            var success = response.EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<bool>()
                .Result;

            return success;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = Get<IEnumerable<Employee>>(Address);
            return employees!;
        }

        public Employee? GetById(int id)
        {
            var result = Get<Employee>($"{Address}/{id}");
            return result;
        }
    }
}
