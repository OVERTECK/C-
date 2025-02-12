using Entities;
using Newtonsoft.Json;
using System.Text;

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

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        var emailField = entryEmail.Text;
        var passwordField = entryPassword.Text;

        signalLabelEmail.IsVisible = string.IsNullOrWhiteSpace(emailField)
            ? true : false;

        signalLabelPassword.IsVisible = string.IsNullOrEmpty(passwordField)
            ? true : false;

        if (string.IsNullOrWhiteSpace(emailField) || string.IsNullOrEmpty(passwordField))
            return;

        var randomWord = Lib.RandomWord.GetWord(6);

        var result = await DisplayPromptAsync(title: "Введите символы ниже.", randomWord);

        if (result == null)
        {
            return;
        }

        if (result.ToLower() != randomWord.ToLower())
        {
            await DisplayAlert("Введенные данные не верные.", "Повторите попытку.", "Ок");

            return;
        }

        var user = new User
        {
            Email = emailField,
            Password = Hash.createHash(passwordField)
        };

        var userJson = JsonConvert.SerializeObject(user);

        using (HttpClient httpClient = new HttpClient())
        {
            var httpContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Lib.BaseAddress.Current}/login", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                await SecureStorage.SetAsync("token", token.Trim('"'));

                await Navigation.PushAsync(new ProductsView());
            }
            else
            {
                await DisplayAlert("Ошибка", await response.Content.ReadAsStringAsync(), "Ок");
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