using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pract_3
{
    public partial class AddElement : Window
    {
        private ListBox listBox;

        public AddElement(ListBox listBox)
        {
            InitializeComponent();

            this.listBox = listBox;
        }

        private void inputCategory_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (inputCategory.Text == "Введите название категории")
            {
                inputCategory.Text = "";

                inputCategory.Foreground = Brushes.Black;
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void inputCategory_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (inputCategory.Text == "")
            {
                inputCategory.Foreground = Brushes.Gray;

                inputCategory.Text = "Введите название категории";
            }
        }

        private void sendRequestBtn(object sender, RoutedEventArgs e)
        {
            var myConnection = new MySqlConnection();

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            try
            {
                string title = inputCategory.Text.Trim();

                if (title == "Введите название категории")
                    throw new ArgumentException("Введите название!");

                myConnection.ConnectionString = myConnectionString;

                myConnection.Open();

                string sql = "INSERT INTO `db_1`.`categories`(`Title`) VALUES (@title);";

                var command = new MySqlCommand(sql, myConnection);

                command.Parameters.AddWithValue("@title", title);

                command.ExecuteNonQuery();

                TableWindow.fillListBox(this.listBox);

                myConnection.Close();

                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
