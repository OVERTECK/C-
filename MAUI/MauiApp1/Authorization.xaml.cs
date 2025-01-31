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
        Navigation.PushAsync(new Regisration());
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProductsView());


        //var emailField = entryEmail.Text;
        //var passwordField = entryPassword.Text;

        //signalLabelEmail.IsVisible = string.IsNullOrEmpty(emailField)
        //    ? true : false;

        //signalLabelPassword.IsVisible = string.IsNullOrEmpty(passwordField)
        //    ? true : false;

        //if (signalLabelEmail.IsVisible || signalLabelPassword.IsVisible)
        //    return;

        //var user = new
        //{
        //    email = emailField,
        //    password = Hash.createHash(passwordField)
        //};

        //var userJson = JsonConvert.SerializeObject(user);

        //using (HttpClient httpClient = new HttpClient())
        //{
        //    var httpContent = new StringContent(userJson, Encoding.UTF8, "application/json");

        //    var response = await httpClient.PostAsync("https://localhost:7166/login", httpContent);

        //    if (response.IsSuccessStatusCode)
        //    {

        //        await Navigation.PushAsync(new ProductsView());
        //    }
        //    else
        //    {
        //        await DisplayAlert("Ошибка", "Почта или пароль не верные", "Ок");
        //    }
        //}
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