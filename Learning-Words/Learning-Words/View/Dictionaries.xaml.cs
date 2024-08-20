using Learning_Words.Models;
using Learning_Words.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Learning_Words.View
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Dictionary : UserControl
    {
        public Dictionary()
        {
            InitializeComponent();

        }
        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            var viewModel = DataContext as DictionaryVM;
            if (viewModel != null)
            {
                var dataGrid = sender as DataGrid;
                var editedWord = dataGrid?.SelectedItem as Word;
                if (editedWord != null)
                {
                    viewModel.HandleRowEdit(editedWord);
                }
            }
        }
    }
}
