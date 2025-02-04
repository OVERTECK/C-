using MauiApp1.Entities;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace MauiApp1;

public partial class ProductsView : ContentPage
{
	public ProductsView()
	{
		InitializeComponent();
    }

    public static List<Product> GetProducts()
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync("https://4cbcncpt-7166.euw.devtunnels.ms/products").Result;

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
            var response = httpClient.GetAsync("https://4cbcncpt-7166.euw.devtunnels.ms/categories").Result;

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
        var products = GetProducts();

        var imageSourceProducts = new List<object>();

        foreach (var product in products)
        {
            if (product.Image != null)
            {
                var stream = new MemoryStream(product.Image);

                var imageSource = ImageSource.FromStream(() => stream);

                var newProduct = new
                {
                    IdProduct = product.Idproduct,
                    Title = product.Title,
                    CategoriesId = product.CategoriesId,
                    Image = imageSource
                };

                imageSourceProducts.Add(newProduct);
            }
            else
            {

                var image = ImageSource.FromResource(product.TitlePath);

                var newProduct = new
                {
                    IdProduct = product.Idproduct,
                    Title = product.Title,
                    CategoriesId = product.CategoriesId,
                    Image = image
                };

                imageSourceProducts.Add(newProduct);
            }
        }

        collectionView.ItemsSource = imageSourceProducts;
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

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var selectedProducts = collectionView.SelectedItems.ToList();

        using (var httpClient = new HttpClient())
        {
            foreach (Product product in selectedProducts)
            {
                var url = "https://4cbcncpt-7166.euw.devtunnels.ms/products/" + product.Idproduct;

                await httpClient.DeleteAsync(url);
            }
        }
        collectionView.ItemsSource = GetProducts();
    }

    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {
        gridLayout.Span = Convert.ToInt16(page.Width / 200);
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReportLXML());
    }
}