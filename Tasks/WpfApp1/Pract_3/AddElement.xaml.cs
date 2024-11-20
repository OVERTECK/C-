using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
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

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            var myConnection = new MySqlConnection(myConnectionString);

            myConnection.Open();

            var command = new MySqlCommand("SELECT title " +
                                           "FROM db_1.categories " +
                                           "ORDER BY title DESC", myConnection);

            var categList = new List<string> { };

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string categories_title = Convert.ToString(reader[0]);

                    categList.Add(categories_title);
                }
            }

            comboBox.ItemsSource = categList;
        }

        private void sendRequestBtn(object sender, RoutedEventArgs e)
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            var myConnection = new MySqlConnection(myConnectionString);

            try
            {
                string categoriesTitle = comboBox.Text.Trim();

                string productTitle = tb_2.Text.Trim();

                if (categoriesTitle == "")
                    throw new ArgumentException("Введите название категории!");

                if (productTitle == "")
                    throw new ArgumentException("Введите название продукта!");

                myConnection.Open();

                string sql = "INSERT INTO `db_1`.`product`(`title`, `categories_id`) " +
                    "VALUES (@title, (SELECT id FROM `db_1`.`categories` WHERE @categories_name = title))";

                var command = new MySqlCommand(sql, myConnection);

                command.Parameters.AddWithValue("@title", productTitle);
                command.Parameters.AddWithValue("@categories_name", categoriesTitle);

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
