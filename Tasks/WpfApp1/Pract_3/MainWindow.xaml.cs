using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _context = null;

        public MainWindow()
        {
            InitializeComponent();

            Input.owner = this;

            frame.Content = Input.getContext();
        }

        public static MainWindow getContext()
        {
            if (_context == null)
            {
                _context = new MainWindow();

                return _context;
            }

            return _context;
        }
    }
}
