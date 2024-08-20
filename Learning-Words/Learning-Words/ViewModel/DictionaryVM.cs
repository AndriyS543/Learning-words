using Learning_Words.Models;
using Learning_Words.Repositories.ChapterRroviders;
using Learning_Words.Repositories.CollectionProviders;
using Learning_Words.Repositories.WordProviders;
using Learning_Words.Services;
using Learning_Words.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Learning_Words.ViewModel
{
    public class DictionaryVM : ViewModelBase
    {

        private readonly ICollectionProviders _collectionProviders;
        private readonly IChapterProviders _chapterProviders;
        private readonly IWordProviders _wordProviders;
        private readonly ICreator _creator;
        private readonly Collection _userCollection;

        // Properties
        public ObservableCollection<Chapter> Chapters { get; set; } = new ObservableCollection<Chapter>();

        private Chapter? _selectedChapter;
        public Chapter? SelectedChapter
        {
            get => _selectedChapter;
            set
            {
                if (_selectedChapter != value)
                {
                    _selectedChapter = value;
                    OnPropertyChanged(nameof(SelectedChapter));
                    UpdateChapterDetails().ConfigureAwait(false);
                }
            }
        }

        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged(nameof(InputText));
                    HandleTextChanged(_inputText);
                }
            }
        }

        private string _DbSearchinputText = string.Empty;
        public string DbSearchinputText
        {
            get => _DbSearchinputText;
            set
            {
                if (_DbSearchinputText != value)
                {
                    _DbSearchinputText = value;
                    OnPropertyChanged(nameof(DbSearchinputText));
                    HandleDbSearchinputTextChanged(_DbSearchinputText);
                }
            }
        }

        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        public ObservableCollection<Word> Words
        {
            get => words;
            set
            {
                words = value;
                OnPropertyChanged(nameof(Words));
            }
        }

        private Word? _selectedWord;
        public Word? SelectedWord
        {
            get => _selectedWord;
            set
            {
                if (_selectedWord != value)
                {
                    _selectedWord = value;
                    OnPropertyChanged(nameof(SelectedWord));
                }
            }
        }

        private ObservableCollection<Word> DBwords = new ObservableCollection<Word>();
        public ObservableCollection<Word> DBWords
        {
            get => DBwords;
            set
            {
                DBwords = value;
                OnPropertyChanged(nameof(DBWords));
            }
        }

        private Word? _DBselectedWord;
        public Word? DBSelectedWord
        {
            get => _DBselectedWord;
            set
            {
                if (_DBselectedWord != value)
                {
                    _DBselectedWord = value;
                    OnPropertyChanged(nameof(DBSelectedWord));
                }
            }
        }
        private ObservableCollection<Word> allWords = new ObservableCollection<Word>();
        private ObservableCollection<Word> allDBWords = new ObservableCollection<Word>();
        // Commands
        public ICommand AddWordCommand { get; }
        public ICommand DeleteWordCommand { get; }

        public ICommand RowDoubleClickCommand { get; }
        public DictionaryVM(Collection userCollection, ICollectionProviders collectionProviders, IChapterProviders chapterProviders, ICreator creator, IWordProviders wordProviders)
        {
            _chapterProviders = chapterProviders;
            _wordProviders = wordProviders;
            _creator = creator;
            _userCollection = userCollection;
            _collectionProviders = collectionProviders;
            // Initialize commands
            AddWordCommand = new AsyncRelayCommand(OnAddWordButtonClick);
            DeleteWordCommand = new AsyncRelayCommand(OnDeleteWordButtonClick);
            RowDoubleClickCommand = new RelayCommand(OnRowDoubleClick);

            // Initialize properties
            InitializeAsync().ConfigureAwait(false);
        }

        private async void OnRowDoubleClick(object param)
        {
            if (SelectedChapter != null && DBSelectedWord != null)
            {
                try
                {
                    var newWord = await _creator.CreateWordAsync(
                        DBSelectedWord.NameWord,
                        DBSelectedWord.Translation,
                        SelectedChapter,
                        DBSelectedWord.Transcription
                    );

                    // Вставка нового слова в початок списків
                    allWords.Insert(0, newWord);
                    Words.Insert(0, newWord);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while adding a word: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No chapter or word selected.");
            }
        }


        /// <summary>
        /// Handles the click event of the Delete Word button.
        /// </summary>
        private async Task OnDeleteWordButtonClick()
        {
            if (SelectedChapter != null && SelectedWord != null)
            {
                try
                {
                    await _wordProviders.DeleteAsync(SelectedWord);
                    allWords.Remove(SelectedWord);
                    Words.Remove(SelectedWord);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the word: {ex.Message}");
                }
            }
        }


        /// <summary>
        /// Handles changes to the input text for filtering words.
        /// </summary>
        private void HandleTextChanged(string inputText)
        {
            if (!string.IsNullOrWhiteSpace(inputText))
            {
                var filteredWords = allWords.Where(x => x.NameWord.StartsWith(inputText, StringComparison.OrdinalIgnoreCase) ||
                                                   x.Translation.StartsWith(inputText, StringComparison.OrdinalIgnoreCase)).ToList();

                Words = new ObservableCollection<Word>(filteredWords);
            }
            else
            {
                Words = new ObservableCollection<Word>(allWords.Reverse());
            }
        }

        private void HandleDbSearchinputTextChanged(string inputText)
        {
            if (!string.IsNullOrWhiteSpace(inputText))
            {
                var filteredWords = allDBWords.Where(x => x.NameWord.StartsWith(inputText, StringComparison.OrdinalIgnoreCase) ||
                                                   x.Translation.StartsWith(inputText, StringComparison.OrdinalIgnoreCase)).ToList();
                DBWords = new ObservableCollection<Word>(filteredWords);
            }
            else
            {
                DBWords = new ObservableCollection<Word>();
            }
        }

        /// <summary>
        /// Handles the click event of the Add Word button.
        /// </summary>
        private async Task OnAddWordButtonClick()
        {
            if (SelectedChapter != null)
            {
                try
                {
                    var word = await _creator.CreateWordAsync("", "", SelectedChapter, "");
                    allWords.Insert(0, word);
                    Words.Insert(0, word);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while adding a word: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No chapter selected.");
            }
        }

        /// <summary>
        /// Asynchronously initializes the view model.
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {


                if (_userCollection == null)
                {
                    throw new Exception("Collection not found.");
                }

                var chapters = await _chapterProviders.GetChaptersByCollectionAsync(_userCollection);

                Chapters.Clear();
                foreach (var chapter in chapters)
                {
                    Chapters.Add(chapter);
                }

                if (Chapters.Any())
                {
                    SelectedChapter = Chapters.First();
                }

                // Отримати всі колекції, починаючи з другої
                var allCollections = await _collectionProviders.GetAllAsync();
                var collectionsToProcess = allCollections.Skip(1).ToList(); // Пропустити першу колекцію

                // Зібрати всі слова з кожної колекції
                var allWords = new List<Word>();

                foreach (var collection in collectionsToProcess)
                {
                    var tempChapters = await _chapterProviders.GetChaptersByCollectionAsync(collection);
                    var words = tempChapters.SelectMany(chapter => chapter.Words).ToList();
                    allWords.AddRange(words);
                }

                // Перетворити список слів на ObservableCollection
                allDBWords = new ObservableCollection<Word>(allWords);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during initialization: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the details of the selected chapter.
        /// </summary>
        private async Task UpdateChapterDetails()
        {
            if (SelectedChapter == null)
            {
                Console.WriteLine("No chapter selected.");
                return;
            }

            try
            {
                var words = await _wordProviders.GetWordsByChapterAsync(SelectedChapter);

                if (words != null && words.Any())
                {
                    Words.Clear();
                    allWords = new ObservableCollection<Word>(words);
                    Words = new ObservableCollection<Word>(allWords.Reverse());
                }
                else
                {
                    Words.Clear();
                    Console.WriteLine("No words found for the selected chapter.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating chapter details: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the event of editing a row in the DataGrid.
        /// </summary>
        public async void HandleRowEdit(Word editedWord)
        {
            if (editedWord == null)
            {
                Console.WriteLine("Edited word is null.");
                return;
            }

            try
            {
                await _wordProviders.UpdateAsync(editedWord);
                Console.WriteLine($"Changes saved for word: {editedWord.NameWord}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            }
        }
    }
}
