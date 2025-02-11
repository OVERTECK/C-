using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Entities;
using Lib;

namespace MauiApp1;

public partial class ProductsView : ContentPage
{
	public ProductsView()
	{
		InitializeComponent();
    }

    private async void categoriesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var selectedCategory = categoriesPicker.SelectedItem as Category;

            if (selectedCategory == null)
                return;

            if (selectedCategory.Id == -1)
            {
                collectionView.ItemsSource = await WebAPI.GetProducts();

                return;
            }

            var products = await WebAPI.GetProducts();

            var filteredProducts = products.Where(c => c.Categories.Id == selectedCategory.Id);

            collectionView.ItemsSource = filteredProducts;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "Ок");
        }

    }

    private async void categoriesPicker_Loaded(object sender, EventArgs e)
    {
        try
        {
            var listCategories = await WebAPI.GetCategories();

            listCategories.Insert(0, new Category { Title = "Все категории", Id = -1 });

            categoriesPicker.ItemsSource = listCategories;

            categoriesPicker.SelectedIndex = 0;

        } catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "Ок");
        }
    }

    private async void collectionView_Loaded(object sender, EventArgs e)
    {
        collectionView.ItemsSource = await WebAPI.GetProducts();
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textSearch = search.Text.ToLower();

        if (string.IsNullOrWhiteSpace(textSearch))
        {
            collectionView.ItemsSource = await WebAPI.GetProducts();

            return;
        }

        var getProducts = await WebAPI.GetProducts();

        var products = getProducts.Where(c => c.Title.ToLower().Contains(textSearch)).ToList();

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
                await httpClient.DeleteAsync($"{Lib.BaseAddress.Current}/products/" + product.Idproduct);
            }
        }

        collectionView.ItemsSource = await WebAPI.GetProducts();
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