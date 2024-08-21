using Learning_Words.Models;
using System.Collections.Generic;
using System.Windows;

namespace Learning_Words.Windows
{
    public partial class DataGridWindowContainer : Window
    {
        public DataGridWindowContainer(IEnumerable<Word> words)
        {
            InitializeComponent();
            DataGridUserControl.DataContext = new DataGridViewModel(words);
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
