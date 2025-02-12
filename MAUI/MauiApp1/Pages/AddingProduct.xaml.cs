using DocumentFormat.OpenXml.Wordprocessing;
using Entities;
using Newtonsoft.Json;
using System.Text;
using Lib;

namespace MauiApp1;

public partial class AddingProduct : ContentPage
{
	public AddingProduct()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var resultPicker = await MediaPicker.Default.PickPhotoAsync();

		if (resultPicker != null)
		{
            labelNameImage.Text = resultPicker.FullPath;
        }
	}

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
		var selectedCategory = pickerAddingProduct.SelectedItem as Entities.Category;
		var productTitle = entryNameProduct.Text;
		var imagePath = labelNameImage.Text;

        if (selectedCategory == null)
		{
			signalLabelCateg.IsVisible = true;

			return;
        }
		else
		{
            signalLabelCateg.IsVisible = false;
        }

		if (string.IsNullOrWhiteSpace(productTitle))
		{
			signalLabelName.IsVisible = true;

			return;
        }
		else
		{
            signalLabelCateg.IsVisible = false;
        }

		var newProduct = new Product
        {
			Title = productTitle,
			CategoriesId = selectedCategory.Id
        };

        if (imagePath != null)
		{
			newProduct.Image = File.ReadAllBytes(imagePath);
		}
		
        var serializeProduct = JsonConvert.SerializeObject(newProduct);

		var httpClient = new HttpClient();

		var stringContent = new StringContent(serializeProduct, Encoding.UTF8, "application/json");

        var token = await SecureStorage.GetAsync("token");

        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        var response = await httpClient.PostAsync($"{Lib.BaseAddress.Current}/products", stringContent);

		if (response.IsSuccessStatusCode)

			//ProductsView.upda

			await Navigation.PopAsync();
		else
		{
			await DisplayAlert("Ошибка!", "Продукт не добавлен.", "Ок");
		}
    }

    private async void pickerAddingProduct_Loaded(object sender, EventArgs e)
    {
		var categories = await WebAPI.GetCategories();

        pickerAddingProduct.ItemsSource = categories;
    }
}