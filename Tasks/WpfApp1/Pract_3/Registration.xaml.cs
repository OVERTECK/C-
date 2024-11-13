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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public static Window owner;

        private static Registration context = null;
        public Registration()
        {
            InitializeComponent();
        }

        public static Registration getContext()
        {
            if (context == null)
            {
                context = new Registration();
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
            string login = tbox_2.Text.Trim();
            string password = passBox.Password;
            string password_2 = passBox_2.Password;

            if (email == "" || password == "" || login == "" || password_2 == "")
            {
                MessageBox.Show("Заполните поля!");

                return;
            }

            if (password != password_2)
            {
                MessageBox.Show("Пароли не совпадают.");

                return;
            }

            string hashPassword = Hash.createHash(password).Replace("-", "");

            var myConnection = new MySqlConnection();

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            myConnection.ConnectionString = myConnectionString;

            myConnection.Open();

            var command = new MySqlCommand("SELECT COUNT(*) FROM db_1.user WHERE email = @email", myConnection);

            command.Parameters.AddWithValue("@email", email);

            var response = command.ExecuteScalar().ToString();

            bool availability = response == "1";

            if (availability)
            {
                MessageBox.Show("Почта занята.");

                return;
            }

            command = new MySqlCommand("INSERT INTO `db_1`.`user`(`email`, `login`, `password`) VALUES (@email, @login, @password)", myConnection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", hashPassword);

            command.ExecuteNonQuery();

            var talbeWindow = new TableWindow();

            talbeWindow.Show();

            owner.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox.IsChecked.Value)
            {
                passBox.Visibility = Visibility.Hidden;
                textboxPassword.Visibility = Visibility.Visible;
                textboxPassword.Text = passBox.Password;

                passBox_2.Visibility = Visibility.Hidden;
                textboxPassword_2.Visibility = Visibility.Visible;
                textboxPassword_2.Text = passBox_2.Password;
            }
            else
            {
                textboxPassword.Visibility = Visibility.Hidden;
                passBox.Visibility = Visibility.Visible;

                textboxPassword_2.Visibility = Visibility.Hidden;
                passBox_2.Visibility = Visibility.Visible;
            }
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Input.getContext());
        }

        private void textboxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            passBox.Password = textboxPassword.Text;
        }

        private void textboxPassword_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passBox_2.Password = textboxPassword_2.Text;
        }
    }
}
