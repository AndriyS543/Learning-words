using Learning_Words.Models;
using Learning_Words.Repositories.ChapterRroviders;
using Learning_Words.Repositories.CollectionProviders;
using Learning_Words.Services;
using Learning_Words.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Learning_Words.ViewModel
{
    public class ChapterVM : ViewModelBase
    {
        private readonly IChapterProviders _chapterProviders;
        private readonly ICreator _creator;
        private Collection _userCollection;

        // String info
        private const string STR_NAME_CHAPTER = "Name chapter: ";
        private const string STR_WORD_COUNT = "Word Count: ";

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
                    UpdateChapterDetails();
                }
            }
        }

        private bool _isDescriptionChanged;
        public bool IsDescriptionChanged
        {
            get => _isDescriptionChanged;
            set
            {
                _isDescriptionChanged = value;
                OnPropertyChanged();
            }
        }

        private string _tbDescription = string.Empty;
        public string tbDescription
        {
            get => _tbDescription;
            set
            {
                _tbDescription = value;
                OnPropertyChanged();
                OnDescriptionTextChanged();
            }
        }

        private string _labelNameChapter = string.Empty;
        public string LabelNameChapter
        {
            get => _labelNameChapter;
            set
            {
                _labelNameChapter = value;
                OnPropertyChanged();
            }
        }

        private string _labelCountWord = string.Empty;
        public string LabelCountWord
        {
            get => _labelCountWord;
            set
            {
                _labelCountWord = value;
                OnPropertyChanged();
            }
        }

        private bool _isDeletedChapter;
        public bool IsDeletedChapter
        {
            get => _isDeletedChapter;
            set
            {
                _isDeletedChapter = value;
                OnPropertyChanged();
            }
        }

        private string _tbAddChapter = string.Empty;
        public string tbAddChapter
        {
            get => _tbAddChapter;
            set
            {
                if (_tbAddChapter != value)
                {
                    _tbAddChapter = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbRenameChapter = string.Empty;
        public string tbRenameChapter
        {
            get => _tbRenameChapter;
            set
            {
                if (_tbRenameChapter != value)
                {
                    _tbRenameChapter = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commands
        public ICommand AddChapterCommand { get; }
        public ICommand DeleteChapterCommand { get; }
        public ICommand RenameChapterCommand { get; }
        public ICommand YesDeleteCommand { get; }
        public ICommand NoDeleteCommand { get; }
        public ICommand YesChangeCommand { get; }
        public ICommand NoChangeCommand { get; }

        public ChapterVM(Collection userCollection, IChapterProviders chapterProviders, ICreator creator)
        {
           // _collectionProviders = collectionProviders;
            _chapterProviders = chapterProviders;
            _creator = creator;
            _userCollection = userCollection;

            YesDeleteCommand = new AsyncRelayCommand(OnYesDeleteButtonClickAsync);
            AddChapterCommand = new AsyncRelayCommand(OnAddChapterButtonClickAsync);
            DeleteChapterCommand = new RelayCommand(DeleteChapterButtonClick);
            RenameChapterCommand = new AsyncRelayCommand(OnRenameButtonClickAsync);
            NoDeleteCommand = new RelayCommand(NoDeleteButtonClick);
            NoChangeCommand = new RelayCommand(OnNoChangeButtonClick);
            YesChangeCommand = new AsyncRelayCommand(OnYesChangeButtonClickAsync);

            // Initialize properties
            InitializeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronous initialization of the view model.
        /// Loads the user collection and chapters from the database.
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {
                LabelCountWord = STR_WORD_COUNT;
                LabelNameChapter = STR_NAME_CHAPTER;

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
            }
            catch (Exception ex)
            {
                // Logging or exception handling
                Console.WriteLine($"An error occurred during initialization: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the details of the selected chapter.
        /// </summary>
        private void UpdateChapterDetails()
        {
            tbDescription = SelectedChapter?.Description ?? string.Empty;
            LabelNameChapter = STR_NAME_CHAPTER + (SelectedChapter?.NameChapter ?? string.Empty);
            LabelCountWord = STR_WORD_COUNT + (SelectedChapter?.Words.Count().ToString() ?? string.Empty);
        }

        /// <summary>
        /// Handles changes to the description text.
        /// </summary>
        public void OnDescriptionTextChanged()
        {
            if (SelectedChapter != null)
            {
                IsDescriptionChanged = SelectedChapter.Description != tbDescription;
            }
        }

        /// <summary>
        /// Handles the confirmation of changes to the chapter description.
        /// </summary>
        private async Task OnYesChangeButtonClickAsync()
        {
            if (SelectedChapter != null)
            {
                try
                {
                    var originalDescription = SelectedChapter.Description;
                    SelectedChapter.Description = tbDescription;
                    var updateResult = await _chapterProviders.UpdateAsync(SelectedChapter);

                    if (!updateResult)
                    {
                        SelectedChapter.Description = originalDescription;
                        Console.WriteLine("Failed to update the chapter.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the chapter description: {ex.Message}");
                }
            }
            else
            {
                tbDescription = string.Empty;
            }

            IsDescriptionChanged = false;
        }

        /// <summary>
        /// Handles the cancellation of changes to the chapter description.
        /// </summary>
        private void OnNoChangeButtonClick(object parameter)
        {
            tbDescription = SelectedChapter?.Description ?? string.Empty;
            IsDescriptionChanged = false;
        }

        /// <summary>
        /// Handles the addition of a new chapter.
        /// </summary>
        private async Task OnAddChapterButtonClickAsync()
        {
            if (!string.IsNullOrWhiteSpace(tbAddChapter) && _userCollection != null)
            {
                try
                {
                    var newChapter = await _creator.CreateChapterAsync(tbAddChapter, _userCollection, string.Empty);

                    if (newChapter != null)
                    {
                        Chapters.Add(newChapter);
                        SelectedChapter = newChapter;
                        tbAddChapter = string.Empty;
                        tbDescription = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while adding the chapter: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Handles the renaming of a chapter.
        /// </summary>
        private async Task OnRenameButtonClickAsync()
        {
            if (!string.IsNullOrWhiteSpace(tbRenameChapter))
            {
                try
                {
                    if (SelectedChapter == null)
                    {
                        Console.WriteLine("No chapter selected for renaming.");
                        tbRenameChapter = string.Empty;
                        return;
                    }

                    if (SelectedChapter.NameChapter == tbRenameChapter)
                    {
                        Console.WriteLine("The new name is the same as the current name.");
                        return;
                    }

                    var originalName = SelectedChapter.NameChapter;
                    SelectedChapter.NameChapter = tbRenameChapter;

                    var updateResult = await _chapterProviders.UpdateAsync(SelectedChapter);

                    if (!updateResult)
                    {
                        SelectedChapter.NameChapter = originalName;
                        Console.WriteLine("Chapter update failed.");
                    }
                    else
                    {
                        var updatedChapter = new Chapter
                        {
                            Id = SelectedChapter.Id,
                            NameChapter = tbRenameChapter,
                            Description = SelectedChapter.Description,
                            CollectionId = SelectedChapter.CollectionId,
                            Words = SelectedChapter.Words,
                        };

                        var index = Chapters.IndexOf(SelectedChapter);
                        if (index != -1)
                        {
                            Chapters[index] = updatedChapter;
                        }
                        else
                        {
                            Chapters.Add(updatedChapter);
                        }

                        SelectedChapter = updatedChapter;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while renaming the chapter: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("The new chapter name cannot be empty.");
            }

            tbRenameChapter = string.Empty;
        }

        /// <summary>
        /// Handles the deletion of a chapter.
        /// </summary>
        private void DeleteChapterButtonClick(object parameter)
        {
            IsDeletedChapter = !IsDeletedChapter;
        }

        /// <summary>
        /// Cancels the chapter deletion process.
        /// </summary>
        private void NoDeleteButtonClick(object parameter)
        {
            IsDeletedChapter = false;
        }

        /// <summary>
        /// Confirms the deletion of the selected chapter.
        /// </summary>
        private async Task OnYesDeleteButtonClickAsync()
        {
            if (SelectedChapter != null && _userCollection != null)
            {
                try
                {
                    if (await _chapterProviders.DeleteAsync(SelectedChapter))
                    {
                        Chapters.Remove(SelectedChapter);
                        if (Chapters.Any())
                        {
                            SelectedChapter = Chapters.First();
                        }
                    }
                    IsDeletedChapter = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the chapter: {ex.Message}");
                }
            }
        }
    }
}
