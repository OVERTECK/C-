using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MauiApp1;

public partial class Authorization : ContentPage
{
	public Authorization()
	{
		InitializeComponent();
	}

    private static Authorization context = null;

    public static Authorization GetContext()
    {
        if (context == null)
        {
            context = new Authorization();

            return context;
        }

        return context;
    }

    private void ClickedRegistration(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Regisration());
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        var user = new
        {
            email = entryEmail.Text,
            password = Hash.createHash(entryPassword.Text)
        };

        var userJson = JsonSerializer.Serialize(user);

        using (HttpClient httpClient = new HttpClient())
        {
            var httpContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7166/login", httpContent);

            if (response.IsSuccessStatusCode)
            {

                await Navigation.PushAsync(new ProductsView());
            }
            else
            {
                await DisplayAlert("Ошибка", "Почта или пароль не верные", "Ок");
            }
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (checkBox.IsChecked)
        {
            entryPassword.IsPassword = false;
        }
        else
        {
            entryPassword.IsPassword = true;
        }
    }
}