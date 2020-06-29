using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client) { }

        public IEnumerable<Section> GetSections() => Get<IEnumerable<Section>>($"sections");

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<Brand>>($"brands");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post("", Filter ?? new ProductFilter())
               .Content
               .ReadAsAsync<IEnumerable<ProductDTO>>()
               .Result;

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{id}");
    }
}