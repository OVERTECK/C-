using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace MauiApp1;

public partial class ReportLXML : ContentPage
{
	public ReportLXML()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var customTypes = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] {".xlsx"} },
                { DevicePlatform.Android, new[] {".xlsx"} }
            }
        );

        var resultPicker = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Выберите папку",
            FileTypes = customTypes
        });

        if (resultPicker == null)
            return;

        pathLabel.Text = resultPicker.FullPath;
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            var fullPath = pathLabel.Text;

            if (string.IsNullOrWhiteSpace(fullPath))
            {
                await DisplayAlert("Ошибка!", "Указажите папку для сохранения!", "Ок");

                return;
            }

            var wb = new XLWorkbook();

            var sh = wb.Worksheets.Add("Products");

            var searchedProducts = ProductsView.GetProducts();

            int index = 1;

            foreach (var product in searchedProducts)
            {
                sh.Cell(index, 1).SetValue(product.Idproduct);
                sh.Cell(index, 2).SetValue(product.Title);
                sh.Cell(index, 3).SetValue(product.Categories.Title);
                sh.Cell(index, 4).SetValue(product.TitlePath);

                index += 1;
            }

            wb.SaveAs(fullPath);

            await DisplayAlert("Статус", "Данные успешно экспортированны.", "Ок");

            Navigation.PopAsync();

        } catch (Exception ex)
        {
            await DisplayAlert("Статус", ex.Message, "Ок");
        }
    }
}