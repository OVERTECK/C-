using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using MySql.Data.MySqlClient;

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

            string myConnectionString = "server=localhost;port=3306;database=db_1;uid=root;password=root";

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
