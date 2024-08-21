using Learning_Words.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Learning_Words.Windows
{
    public partial class DataGridWindow : UserControl
    {
        public DataGridWindow()
        {
            InitializeComponent();
        }

        public DataGridWindow(IEnumerable<Word> words) : this()
        {
            DataContext = new DataGridViewModel(words);
        }
    }

    public class DataGridViewModel
    {
        public DataGridViewModel(IEnumerable<Word> words)
        {
            Words = new ObservableCollection<Word>(words);
        }

        public ObservableCollection<Word> Words { get; }
    }
}
