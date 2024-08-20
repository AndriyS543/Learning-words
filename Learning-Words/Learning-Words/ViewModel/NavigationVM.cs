using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning_Words.Utilities;
using System.Windows.Input;
using Learning_Words.Repositories.ChapterRroviders;
using Learning_Words.Repositories.CollectionProviders;
using Learning_Words.Repositories.WordProviders;
using Learning_Words.Services;
using Learning_Words.Models;

namespace Learning_Words.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private readonly ICollectionProviders _collectionProviders;
        private readonly IChapterProviders _chapterProviders;
        private readonly IWordProviders _wordProviders;
        private readonly ICreator _creator;
        private readonly Collection _userCollection;

        private object? _currentView;
        public object? CurrentView
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

        private void Chapter(object obj) => CurrentView = new ChapterVM(_userCollection, _chapterProviders, _creator);
        public NavigationVM(Collection userCollection, ICollectionProviders collectionProviders, IChapterProviders chapterProviders, IWordProviders wordProviders, ICreator creator)
        {
            _collectionProviders = collectionProviders;
            _chapterProviders = chapterProviders;
            _wordProviders = wordProviders;
            _creator = creator;
            _userCollection = userCollection;

            HomeCommand = new RelayCommand(Home);
            DictionaryCommand = new RelayCommand(Dictionary);
            TestsCommand = new RelayCommand(Test);
            ChaptersCommand = new RelayCommand(Chapter);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
