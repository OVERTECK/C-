using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Data.Common;

namespace Pract_3
{
    public partial class TableWindow : Window
    {
        private static string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);

        public TableWindow()
        {
            InitializeComponent();

            fillListBox(listBox);
        }

        public static void fillListBox(ListBox listBox)
        {
            try
            {   
                myConnection.Open();

                var command = new MySqlCommand("SELECT categories.title, product.title " + 
                                                "FROM db_1.product " +
                                                "INNER JOIN db_1.categories " +
                                                "ON product.categories_id = categories.id", myConnection);

                var categList = new List<Product> { };

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categories_title = Convert.ToString(reader[0]);

                        string title = Convert.ToString(reader[1]);
                        
                        categList.Add(new Product() { title = title, categories_title = categories_title});
                    }
                }

                listBox.ItemsSource = categList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        }

        private void addElementBtnClick(object sender, RoutedEventArgs e)
        {
            var addElementWindow = new AddElement(listBox);

            addElementWindow.Owner = this;

            addElementWindow.ShowDialog();
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();

                var selectedItems = listBox.SelectedItems;

                var answerDelete = MessageBox.Show("Вы действительно хотите удалить выделенные элементы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (answerDelete)
                {
                    case MessageBoxResult.Yes:
                        foreach (Product item in selectedItems)
                        {
                            var command = new MySqlCommand($"DELETE FROM db_1.product WHERE title = @title", myConnection);

                            command.Parameters.AddWithValue("@title", item.title);

                            command.ExecuteNonQuery();
                        }

                        break;

                    case MessageBoxResult.No:
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
            }

            fillListBox(listBox);
        }

        private void ComboBox_Initialized(object sender, EventArgs e)
        {
            try
            {
                comboBoxFilter.SelectedIndex = 0;

                myConnection.Open();

                var command = new MySqlCommand("SELECT title " +
                                               "FROM db_1.categories " +
                                               "ORDER BY title DESC", myConnection);

                var categList = new List<String> { "Не фильтровать" };

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categories_title = Convert.ToString(reader[0]);

                        categList.Add(categories_title);
                    }
                }

                comboBoxFilter.ItemsSource = categList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        } 

        private void comboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectString = comboBoxFilter.SelectedValue.ToString();

            if (listBox is null)
                return;

            if (comboBoxFilter.SelectedIndex == 0)
            {
                fillListBox(listBox);

                return;
            }

            try
            {
                myConnection.Open();

                var command = new MySqlCommand("SELECT categories.title, product.title " +
                                               "FROM db_1.product " +
                                               "INNER JOIN db_1.categories " +
                                               "ON product.categories_id = categories.id " +
                                               "WHERE categories.title = @selectString", myConnection);

                command.Parameters.AddWithValue("@selectString", selectString);

                var categList = new List<Product> { };

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categories_title = Convert.ToString(reader[0]);

                        string title = Convert.ToString(reader[1]);

                        categList.Add(new Product() { title = title, categories_title = categories_title });
                    }
                }

                listBox.ItemsSource = categList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}
