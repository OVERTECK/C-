using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Pract_3
{
    public partial class Input : Page
    {
        private static string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);

        public static Window owner;

        private static Input context = null;

        public Input()
        {
            InitializeComponent();
        }

        public static Input getContext()
        {
            if (context == null)
                context = new Input();

            return context;
         }

        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            string email = tbox_1.Text.Trim();
            string password = passBox.Password;
            
            if (email == "")
            {
                MessageBox.Show("Заполните поле \"Эл. Почта\"!");

                return;
            }

            if (password == "")
            {
                MessageBox.Show("Заполните поле \"Пароль\"");

                return;
            }

            Window captchaWindow = new CAPTCHAWindow();

            captchaWindow.Owner = owner;

            if (captchaWindow.ShowDialog() != true)
            {
                return;
            }

            myConnection.Open();

            var command = new MySqlCommand("SELECT COUNT(*) " +
                                           "FROM db_1.user " +
                                           "WHERE email = @email", myConnection);

            command.Parameters.AddWithValue("@email", email);

            var response = command.ExecuteScalar().ToString();

            if (response != "1")
            {
                MessageBox.Show("Аккаунт не найден.");

                return;
            }

            string hashPassword = Hash.createHash(password);

            command = new MySqlCommand("SELECT COUNT(*) = 1 FROM db_1.user WHERE email = @email AND password = @password", myConnection);

            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", hashPassword);

            response = command.ExecuteScalar().ToString();

            if (response == "0")
            {
                MessageBox.Show("Неверный пароль.");

                return;
            }

            myConnection.Close();

            var talbeWindow = new TableWindow();

            talbeWindow.Show();

            owner.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            var passwordString = passBox.Password;

            if (checkBox.IsChecked.Value)
            {
                passBox.Visibility = Visibility.Hidden;
                tbPassword.Visibility = Visibility.Visible;
                tbPassword.Text = passwordString;
            } else
            {
                tbPassword.Visibility = Visibility.Hidden;
                passBox.Visibility = Visibility.Visible;
            }
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            Registration.owner = owner;

            NavigationService.Navigate(Registration.getContext());
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            passBox.Password = tbPassword.Text;
        }
    }
}
