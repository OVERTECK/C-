using Newtonsoft.Json;
using Entities;
using System.Net.Http.Json;

namespace Lib
{
    public class WebAPI
    {
        public async static Task<List<Product>> GetProducts()
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{Lib.BaseAddress.Current}/products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>();

                List<Category> categories = await GetCategories();

                if (products != null)
                {
                    foreach (var product in products)
                    {
                        var searchedCategory = categories.FirstOrDefault(c => c.Id == product.CategoriesId);

                        if (searchedCategory != null)
                            product.Categories = searchedCategory;
                        else
                            throw new ArgumentException();
                    }
                    return products;
                }
            }
            throw new HttpRequestException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async static Task<List<Category>> GetCategories()
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{Lib.BaseAddress.Current}/categories");

            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<Category>>();

                return categories ?? throw new HttpRequestException();
            }
            else
            {
                throw new HttpRequestException();
            }
        }
    }
}
