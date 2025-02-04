using DocumentFormat.OpenXml.Wordprocessing;
using MauiApp1.Entities;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Text;
using Microsoft.Maui.Media;

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

    private void Button_Clicked_1(object sender, EventArgs e)
    {
		var categoriesTitle = pickerAddingProduct.SelectedItem;
		var productTitle = entryNameProduct.Text;
		var imagePath = labelNameImage.Text;

        if (categoriesTitle == null)
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

		byte[] bytesImage = null;

		if (string.IsNullOrEmpty(imagePath))
		{
			imagePath = "no_images.png";
		} 
		else
		{
			bytesImage = File.ReadAllBytes(imagePath);
			imagePath = null;
        }

		var categories = ProductsView.GetCategories();

		var categoryId = categories.FirstOrDefault(c => c.Title == categoriesTitle.ToString()).Id;

		var newUser = new Product
		{
			Title = productTitle,
			CategoriesId = categoryId,
			Image = bytesImage,
			TitlePath = imagePath
		};

		var serializeUser = JsonConvert.SerializeObject(newUser);

		using (var httpClient = new HttpClient())
		{
			var stringContent = new StringContent(serializeUser, Encoding.UTF8, "application/json");

			var response = httpClient.PostAsync("https://4cbcncpt-7166.euw.devtunnels.ms/products", stringContent).Result;

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Продукт не добавлен");
			}
		}

		Navigation.PopAsync();
    }

    private void pickerAddingProduct_Loaded(object sender, EventArgs e)
    {
		pickerAddingProduct.ItemsSource = ProductsView.GetCategories().Select(c => c.Title).ToList();
    }
}