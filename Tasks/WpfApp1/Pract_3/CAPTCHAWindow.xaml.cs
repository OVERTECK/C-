using System.Windows;

namespace Pract_3
{
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
            }
            
            DialogResult = true;
        }
    }
}
