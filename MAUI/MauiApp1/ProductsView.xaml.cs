using MauiApp1.Entities;
using Newtonsoft.Json;

namespace MauiApp1;

public partial class ProductsView : ContentPage
{
	public ProductsView()
	{
		InitializeComponent();

        GetProducts();

        GetCategories();
    }

    public static List<Product> GetProducts()
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync("https://localhost:7166/products/").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                var products = JsonConvert.DeserializeObject<List<Product>>(jsonString);

                var categories = GetCategories();

                foreach (var product in products)
                {
                    product.Categories = categories.FirstOrDefault(c => c.Id == product.CategoriesId);
                }

                return products;
            }
            else
            {
                throw new Exception("Данные не получены.");
            }
        }
    }

    public static List<Category> GetCategories()
    {
        using (var httpClient = new HttpClient())
        {
            var response = httpClient.GetAsync("https://localhost:7166/categories").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                var listCategories = JsonConvert.DeserializeObject<List<Category>>(jsonString);

                return listCategories;
            }
            else
            {
                throw new Exception("Данные не получены.");
            }
        }
    }

    private void categoriesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCategory = categoriesPicker.SelectedItem.ToString();

        if (selectedCategory == null)
            return;

        if (selectedCategory == "Все категории")
        {
            collectionView.ItemsSource = GetProducts();

            return;
        }

        var listProducts = GetProducts().Where(c => c.Categories.Title == selectedCategory);

        collectionView.ItemsSource = listProducts;
    }

    private void categoriesPicker_Loaded(object sender, EventArgs e)
    {
        var listCategories = GetCategories();

        listCategories.Insert(0, new Category { Title = "Все категории" });

        categoriesPicker.ItemsSource = listCategories.Select(c => c.Title).ToList();

        categoriesPicker.SelectedIndex = 0;
    }

    private void collectionView_Loaded(object sender, EventArgs e)
    {
        collectionView.ItemsSource = GetProducts();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textSearch = search.Text.ToLower();

        if (string.IsNullOrWhiteSpace(textSearch))
        {
            collectionView.ItemsSource = GetProducts();

            return;
        }

        var products = GetProducts().Where(c => c.Title.ToLower().Contains(textSearch)).ToList();

        collectionView.ItemsSource = products;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddingProduct());
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        var selectedProducts = collectionView.SelectedItems.ToList();

        using (var httpClient = new HttpClient())
        {
            foreach (Product product in selectedProducts)
            {
                var url = "https://localhost:7166/products/" + product.Idproduct;

                httpClient.DeleteAsync(url);

                Thread.Sleep(100);
            }
        }
        collectionView.ItemsSource = GetProducts();
    }
}