using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities.Employees;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        private readonly string address = WebAPI.Employees;
        public EmployeesClient(HttpClient client) : base(client) { }

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(address);

        public Employee GetById(int id) => Get<Employee>($"{address}/{id}");

        public int Add(Employee Employee) => Post(address, Employee).Content.ReadAsAsync<int>().Result;

        public void Edit(Employee Employee) => Put(address, Employee);

        public bool Delete(int id) => Delete($"{address}/{id}").IsSuccessStatusCode;

        public void SaveChanges() { }
    }
}