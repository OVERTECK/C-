using MauiApp1.Entities;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using Newtonsoft.Json;
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

    private void ClickedRegistration(object sender, EventArgs e)
    {
        Navigation.PushAsync(Regisration.GetContext());
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        var emailField = entryEmail.Text;
        var passwordField = entryPassword.Text;

        signalLabelEmail.IsVisible = string.IsNullOrWhiteSpace(emailField)
            ? true : false;

        signalLabelPassword.IsVisible = string.IsNullOrEmpty(passwordField)
            ? true : false;

        if (string.IsNullOrWhiteSpace(emailField) || string.IsNullOrEmpty(passwordField))
            return;

        var user = new User
        {
            Email = emailField,
            Password = Hash.createHash(passwordField)
        };

        var userJson = JsonConvert.SerializeObject(user);

        using (HttpClient httpClient = new HttpClient())
        {
            var httpContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("https://4cbcncpt-7166.euw.devtunnels.ms/login", httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Navigation.PushAsync(new ProductsView());
            }
            else
            {
                DisplayAlert("Œ¯Ë·Í‡", response.Content.ReadAsStringAsync().Result, "ŒÍ");
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