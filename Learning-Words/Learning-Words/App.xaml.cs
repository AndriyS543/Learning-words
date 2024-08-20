using Learning_Words.DatabaseInitializer;
using Learning_Words.DBContext;
using Learning_Words.Models;
using Learning_Words.Repositories.ChapterRroviders;
using Learning_Words.Repositories.CollectionProviders;
using Learning_Words.Repositories.WordProviders;
using Learning_Words.Services;
using Learning_Words.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Learning_Words
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_ST = "Data Source=mywords.db";

        private readonly IChapterProviders _chapterProviders;
        private readonly IWordProviders _wordProviders;
        private readonly ICollectionProviders _collectionProviders;
        private readonly ICreator _creator;
        private readonly DbInitializer _initializer;
        private Collection? _userCollection;
 
        private readonly VocabularyDbContextFactory _vocabularyDbContextFactory;
        public App()
        {
            _vocabularyDbContextFactory = new VocabularyDbContextFactory(CONNECTION_ST);
            _initializer = new DbInitializer(_vocabularyDbContextFactory);
            _collectionProviders = new DatabaseCollectionProvider(_vocabularyDbContextFactory);
            _chapterProviders = new DatabaseChapterProvider(_vocabularyDbContextFactory);
            _wordProviders = new DatabaseWordProvider(_vocabularyDbContextFactory);
            _creator = new Creator(_wordProviders, _chapterProviders);
        }
        protected async override void OnStartup(StartupEventArgs e)
        {
            using (VocabularyDbContext dbContext = _vocabularyDbContextFactory.CreateDbContext())
            {

                dbContext.Database.Migrate();
                await _initializer.InitializeAsync();
                _userCollection = await _collectionProviders.GetByIdAsync(1);

            }

            var navigationVM = new NavigationVM(_userCollection, _collectionProviders, _chapterProviders, _wordProviders, _creator);

            MainWindow = new MainWindow
            {
                DataContext = navigationVM
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
