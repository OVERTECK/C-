using Entities;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace MauiApp1;

public partial class Regisration : ContentPage
{
	public Regisration()
	{
		InitializeComponent();
	}

    private static Regisration context = null;

    public static Regisration GetContext()
    {
        if (context == null)
        {
            context = new Regisration();

            return context;
        }

        return context;
    }

    private void ClickedAuthorization(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        signalLoginAccess.IsVisible = false;
        signalLoginBusy.IsVisible = false;

        var loginField = entryLogin.Text;
        var emailField = entryEmail.Text;
        var passwordField1 = entryPassword1.Text;
        var passwordField2 = entryPassword2.Text;

        signalLabelLogin.IsVisible = string.IsNullOrWhiteSpace(loginField)
            ? true : false;

        signalLabelEmail.IsVisible = string.IsNullOrWhiteSpace(emailField)
            ? true : false;

        signalLabelPassword1.IsVisible = string.IsNullOrEmpty(passwordField1)
            ? true : false;

        signalLabelPassword2.IsVisible = string.IsNullOrEmpty(passwordField2)
            ? true : false;

        if (string.IsNullOrWhiteSpace(loginField) ||
            string.IsNullOrWhiteSpace(emailField) ||
            string.IsNullOrEmpty(passwordField1) ||
            string.IsNullOrEmpty(passwordField2))
            return;

        if (passwordField1 != passwordField2)
        {
            await DisplayAlert("Ошибка!", "Пароли не совпадают!", "Ок");
            
            return;
        }

        if (!Regex.Match(emailField, @".+@.+\..+").Success) {

            await DisplayAlert("Ошибка!", "Формат почты не верный.", "Ок");

            return;
        }

        var newUser = new User
        {
            Email = emailField,
            Login = loginField,
            Password = Hash.createHash(passwordField1)
        };

        var serializedUser = JsonConvert.SerializeObject(newUser);

        using (var httpClient = new HttpClient())
        {
            var stringContent = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Lib.BaseAddress.Current}/register", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                await SecureStorage.SetAsync("token", token.Trim('"'));

                Navigation.PushAsync(new ProductsView());
            }
            else
            {
                DisplayAlert("Ошибка!", response.Content.ReadAsStringAsync().Result, "Ок");
            }
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;

        if (checkBox.IsChecked)
        {
            entryPassword1.IsPassword = false;
            entryPassword2.IsPassword = false;
        }
        else
        {
            entryPassword1.IsPassword = true;
            entryPassword2.IsPassword = true;
        }

    }

    private async void entryLogin_TextChanged(object sender, TextChangedEventArgs e)
    {
        signalLabelLogin.IsVisible = false;

        var login = entryLogin.Text;

        if (string.IsNullOrWhiteSpace(login))
        {
            signalLabelLogin.IsVisible = true;
            signalLoginAccess.IsVisible = false;

            return;
        }

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"{Lib.BaseAddress.Current}/users/" + login);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                signalLoginBusy.IsVisible = false;
                signalLoginAccess.IsVisible = true;
            }
            else
            {
                signalLoginBusy.IsVisible = true;
                signalLoginAccess.IsVisible = false;
            }
        }
    }
}