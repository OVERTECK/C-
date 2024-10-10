using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void datagrid_Initialized(object sender, EventArgs e)
        {
            MySqlConnection myConnection = new MySqlConnection();

            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            try
            {
                myConnection.ConnectionString = myConnectionString;

                myConnection.Open();

                MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter("SELECT * FROM db_1.categories", myConnection);

                DataTable dataTable = new DataTable();

                mySqlAdapter.Fill(dataTable);

                datagrid.ItemsSource = dataTable.AsDataView();
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
