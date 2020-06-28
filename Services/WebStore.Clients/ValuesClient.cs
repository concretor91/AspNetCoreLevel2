using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebStore.Interfaces;
namespace WebStore.Clients.Services.Values
{
    public class ValuesClient : IValuesService
    {
        private readonly HttpClient client;

        public ValuesClient(HttpClient client)
        {
            this.client = client;
        }
        //protected sealed override string ServiceAddress { get; set; }
        public async Task<IEnumerable<string>> GetAsync()
        {
            var list = new List<string>();
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<string>>();
            }
            return list;
        }
        public string Get(int id)
        {
            var result = string.Empty;
            var response = client.GetAsync($"/get/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<string>().Result;
            }
            return result;
        }
        public async Task<string> GetAsync(int id)
        {
            var result = string.Empty;
            var response = await client.GetAsync($"/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<string>();
            }
            return result;
        }
        public Uri Post(string value)
        {
            var response = client.PostAsJsonAsync($"/post",
            value).Result;
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        public async Task<Uri> PostAsync(string value)
        {
            var response = await
            client.PostAsJsonAsync($"/post", value);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        public HttpStatusCode Put(int id, string value)
        {
            var response = client.PutAsJsonAsync($"/put/{id}",
            value).Result;
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await
            client.PutAsJsonAsync($"/put/{id}", value);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        public HttpStatusCode Delete(int id)
        {
            var response =
            client.DeleteAsync($"/delete/{id}").Result;
            return response.StatusCode;
        }
        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var response = await
            client.DeleteAsync($"/delete/{id}");
            return response.StatusCode;
        }
    }
}