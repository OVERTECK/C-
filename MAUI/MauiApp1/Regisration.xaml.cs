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
        var emailField = entryEmail.Text;
        var passwordField1 = entryPassword1.Text;
        var passwordField2 = entryPassword2.Text;

        signalLabelEmail.IsVisible = string.IsNullOrEmpty(emailField)
            ? true : false;

        signalLabelPassword1.IsVisible = string.IsNullOrEmpty(passwordField1)
            ? true : false;

        signalLabelPassword2.IsVisible = string.IsNullOrEmpty(passwordField2)
            ? true : false;

        if (signalLabelEmail.IsVisible ||
            signalLabelPassword1.IsVisible ||
            signalLabelPassword2.IsVisible)
            return;

        if (passwordField1 != passwordField2)
        {
            await DisplayAlert("Ошибка!", "Пароли не совпадают!", "Ок");
            
            return;
        }

        await Navigation.PushAsync(new ProductsView());
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
}