using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Pract_3
{
    /// <summary>
    /// Логика взаимодействия для CAPTCHAWindow.xaml
    /// </summary>
    public partial class CAPTCHAWindow : Window
    {
        private string randomWord = RandomWord.createRandomWord(8);
        public CAPTCHAWindow()
        {
            InitializeComponent();

            textblockCAPTCHA.Text = randomWord;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (answerTextBlock.Text == "")
            {
                MessageBox.Show("Заполните поле!");

                return;
            }

            if (answerTextBlock.Text.ToLower() != randomWord.ToLower())
            {
                MessageBox.Show("Текст не совпадает!");

                return;

            } else
            {
                DialogResult = true;
            }
        }
    }
}
