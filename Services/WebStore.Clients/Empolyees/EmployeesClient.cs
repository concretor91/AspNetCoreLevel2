using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain.Entities.Employees;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client) { }

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>("");

        public Employee GetById(int id) => Get<Employee>($"{id}");

        public int Add(Employee Employee) => Post("", Employee).Content.ReadAsAsync<int>().Result;

        public void Edit(Employee Employee) => Put("", Employee);

        public bool Delete(int id) => Delete($"{id}").IsSuccessStatusCode;

        public void SaveChanges() { }
    }
}