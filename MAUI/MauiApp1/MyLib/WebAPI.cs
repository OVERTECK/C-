using Newtonsoft.Json;
using Entities;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Lib
{
    public class WebAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async static Task<List<Product>> GetProducts()
        {
            var httpClient = new HttpClient();

            var token = await SecureStorage.GetAsync("token");

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await httpClient.GetAsync($"{Lib.BaseAddress.Current}/products");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Ошибка запроса.");

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            if (products == null)
                throw new ArgumentException("Ошибка конвертации продуктов.");

            List<Category> categories = await GetCategories();

            // Связываем поле Categories с классом Category
            foreach (var product in products)
            {
                var searchedCategory = categories.FirstOrDefault(c => c.Id == product.CategoriesId);

                if (searchedCategory != null)
                    product.Categories = searchedCategory;
                else
                    throw new ArgumentException($"Категория, с id: {product.CategoriesId}, не найдена.");
            }

            // Преобразуем массив байтов или путь к изображение в ItemSource
            foreach (var product in products)
            {
                if (product.Image != null)
                {
                    var stream = new MemoryStream(product.Image);

                    var imageSource = ImageSource.FromStream(() => stream);

                    product.ImageSource = imageSource;
                }
                else if (product.TitlePath != null)
                {
                    var imageSource = ImageSource.FromFile(product.TitlePath);

                    product.ImageSource = imageSource;
                }
                else
                {
                    var imageSource = ImageSource.FromFile("no_images.png");

                    product.ImageSource = imageSource;
                }
            }

            return products;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async static Task<List<Category>> GetCategories()
        {
            using var httpClient = new HttpClient();

            var token = await SecureStorage.GetAsync("token");

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await httpClient.GetAsync($"{Lib.BaseAddress.Current}/categories");

            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<Category>>();

                return categories ?? throw new HttpRequestException("Ошибка преобразования категорий.");
            }
            else
            {
                throw new HttpRequestException();
            }
        }
    }
}
