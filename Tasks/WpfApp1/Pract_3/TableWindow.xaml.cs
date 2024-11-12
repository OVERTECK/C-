using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Xml.Linq;
using System.Windows.Controls;

namespace Pract_3
{
    public partial class TableWindow : Window
    {
        public TableWindow()
        {
            InitializeComponent();
        }

        static public void fillListBox(ListBox listBox)
        {
            var myConnection = new MySqlConnection();

            try
            {    
                string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

                myConnection.ConnectionString = myConnectionString;

                myConnection.Open();

                var command = new MySqlCommand("SELECT * FROM db_1.categories", myConnection);

                var categList = new List<Categories> { };

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader[0]);
                        string title = Convert.ToString(reader[1]);

                        categList.Add(new Categories() { id = id, title = title });
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

        private void listBox_Initialized(object sender, EventArgs e)
        {
            fillListBox(listBox);
        }

        private void addElementBtnClick(object sender, RoutedEventArgs e)
        {

            var addElementWindow = new AddElement(listBox);

            addElementWindow.Owner = this;

            addElementWindow.ShowDialog();
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            var myConnection = new MySqlConnection();

            try
            {
                string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

                myConnection.ConnectionString = myConnectionString;

                myConnection.Open();

                var selectedItems = listBox.SelectedItems;

                var answerDelete = MessageBox.Show("Вы действительно хотите удалить выделенные элементы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (answerDelete)
                {
                    case MessageBoxResult.Yes:
                        foreach (Categories item in selectedItems)
                        {
                            var command = new MySqlCommand($"DELETE FROM db_1.categories WHERE id = @indexSelected", myConnection);

                            command.Parameters.AddWithValue("@indexSelected", item.id);

                            command.ExecuteNonQuery();
                        }

                        fillListBox(listBox);

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
        }
    }
}
