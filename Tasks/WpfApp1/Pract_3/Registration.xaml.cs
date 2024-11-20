using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Pract_3
{
    public partial class Registration : Page
    {
        public static Window owner;

        private static string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);

        private static Registration context = null;

        public Registration()
        {
            InitializeComponent();
        }

        public static Registration getContext()
        {
            if (context == null)
                context = new Registration();

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

            if (email == "")
            {
                MessageBox.Show("Заполните поле \"Эл. Почта\"!");

                return;
            }

            if (login == "")
            {
                MessageBox.Show("Заполните поле \"Логин\"");

                return;
            }

            if (password == "")
            {
                MessageBox.Show("Заполните поле \"Пароль\"");

                return;
            }

            if (password_2 == "")
            {
                MessageBox.Show("Заполните поле \"Повторно пароль\"");

                return;
            }

            if (password != password_2)
            {
                MessageBox.Show("Пароли не совпадают.");

                return;
            }

            Window captchawindow = new CAPTCHAWindow();

            captchawindow.Owner = owner;

            if (captchawindow.ShowDialog() != true)
            {
                return;
            }

            string hashPassword = Hash.createHash(password);

            myConnection.Open();

            var command = new MySqlCommand("SELECT COUNT(*) FROM db_1.user WHERE email = @email", myConnection);

            command.Parameters.AddWithValue("@email", email);

            var response = command.ExecuteScalar().ToString();

            if (response == "1")
            {
                MessageBox.Show("Почта занята.");

                return;
            }

            command = new MySqlCommand("INSERT INTO `db_1`.`user`(`email`, `login`, `password`) VALUES (@email, @login, @password)", myConnection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", hashPassword);

            command.ExecuteNonQuery();

            myConnection.Close();

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
