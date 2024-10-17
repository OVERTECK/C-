using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Controls;
using System.Collections.Generic;

namespace ListBoxOutput
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void listBox_Initialized(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection();

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            List<Categories> categList = new List<Categories> { };

            try
            {
                myConnection.ConnectionString = myConnectionString;

                myConnection.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM db_1.categories", myConnection);

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
    }
}
