using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pract_3
{
    /// <summary>
    /// Логика взаимодействия для Input.xaml
    /// </summary>
    public partial class Input : Page
    {
        public Window owner;

        private static Input context = null;

        public Input()
        {
            InitializeComponent();
        }

        public static Input getContext()
        {
            if (context == null)
            {
                context = new Input();
            }

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
            
            if (email == "" || password == "")
            {
                MessageBox.Show("Заполните поля!");

                return;
            }

            var myConnection = new MySqlConnection();

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            myConnection.ConnectionString = myConnectionString;

            myConnection.Open();

            var command = new MySqlCommand("SELECT COUNT(*) FROM db_1.user WHERE email = @email", myConnection);

            command.Parameters.AddWithValue("@email", email);

            var response = command.ExecuteScalar().ToString();

            bool availability = response == "1";

            if (!availability)
            {
                MessageBox.Show("Аккаунт не найден.");

                return;
            }

            string hashPassword = Hash.createHash(password).Replace("-", "");

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
            Registration.owner = this.owner;

            NavigationService.Navigate(Registration.getContext());
        }
    }
}
