using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning_Words.Utilities;
using System.Windows.Input;

namespace Learning_Words.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DictionaryCommand { get; set; }
        public ICommand TestsCommand { get; set; }
        public ICommand ChaptersCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Dictionary(object obj) => CurrentView = new DictionaryVM();
        private void Test(object obj) => CurrentView = new TestVM();

        private void Chapter(object obj) => CurrentView = new ChapterVM();
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            DictionaryCommand = new RelayCommand(Dictionary);
            TestsCommand = new RelayCommand(Test);
            ChaptersCommand = new RelayCommand(Chapter);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
