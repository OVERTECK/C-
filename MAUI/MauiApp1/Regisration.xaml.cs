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

    private void OnCounterClicked(object sender, EventArgs e)
    {

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