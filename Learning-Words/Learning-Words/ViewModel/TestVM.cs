using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Learning_Words.Models;
using Learning_Words.Repositories.ChapterRroviders;
using Learning_Words.Repositories.WordProviders;
using Learning_Words.Utilities;
using Learning_Words;

namespace Learning_Words.ViewModel
{
    public class TestVM : ViewModelBase
    {
        private readonly IChapterProviders _chapterProviders;
        private readonly IWordProviders _wordProviders;
        private readonly Collection _userCollection;

        private const string STR_WORD_COUNT = "Word Count: ";
        private const string STR_CHAPTER_COUNT = "Chapter Count: ";

        // Collections for Settings Test
        public ObservableCollection<Chapter> AllChapters { get; set; } = new ObservableCollection<Chapter>();
        public ObservableCollection<Chapter> UsedChapters { get; set; } = new ObservableCollection<Chapter>();


        // Collections for statistics 
        private ObservableCollection<Word> answerWords = new ObservableCollection<Word>();
        public ObservableCollection<Word> AnswerWords
        {
            get => answerWords;
            set
            {
                answerWords = value;
                OnPropertyChanged(nameof(AnswerWords));
            }
        }

        private ObservableCollection<TestResult> _testResults = new ObservableCollection<TestResult>();
        public ObservableCollection<TestResult> TestResults
        {
            get => _testResults;
            set
            {
                _testResults = value;
                OnPropertyChanged();
            }
        }

        // Test Static Variables
        private TestResult _selectedTestResult;
        public TestResult SelectedTestResult
        {
            get => _selectedTestResult;
            set
            {
                _selectedTestResult = value;
                OnPropertyChanged();
                if (SelectedTestResult != null)
                {
                    AnswerWords = new ObservableCollection<Word>(SelectedTestResult.Words);
                    UpdateLabelsStatic();
                }
            }
        }

        private Queue<Word> _wordsQueue = new Queue<Word>();
        private Random _random = new Random();

        // Commands
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand AnswerCommand { get; }

        public ICommand OpenDataGridCommand { get; }
        // Properties
        private Chapter? _selectedChapter;
        public Chapter? SelectedChapter
        {
            get => _selectedChapter;
            set
            {
                if (_selectedChapter != value)
                {
                    _selectedChapter = value;
                    OnPropertyChanged();
                    if (_selectedChapter != null)
                    {
                        UpdateChapterUsage(_selectedChapter);
                    }
                }
            }
        }

        private bool _isStartTest;
        public bool IsStartTest
        {
            get => _isStartTest;
            set
            {
                _isStartTest = value;
                OnPropertyChanged();
            }
        }

        private bool _isSettingsTest;
        public bool IsSettingsTest
        {
            get => _isSettingsTest;
            set
            {
                _isSettingsTest = value;
                OnPropertyChanged();
            }
        }

        private bool _isAnswerWordSelected;
        public bool IsAnswerWordSelected
        {
            get => _isAnswerWordSelected;
            set
            {
                if (_isAnswerWordSelected != value)
                {
                    _isAnswerWordSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAnswerTranslationSelected;
        public bool IsAnswerTranslationSelected
        {
            get => _isAnswerTranslationSelected;
            set
            {
                if (_isAnswerTranslationSelected != value)
                {
                    _isAnswerTranslationSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private Word _currentWord;
        private List<Word> _currentOptions = new List<Word>();

        private string _currentWordName;
        public string CurrentWordName
        {
            get => _currentWordName;
            set
            {
                _currentWordName = value;
                OnPropertyChanged();
            }
        }

        private string _currentTranscription;
        public string CurrentTranscription
        {
            get => _currentTranscription;
            set
            {
                _currentTranscription = value;
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

        private string _labelCountChapter = string.Empty;
        public string LabelCountChapter
        {
            get => _labelCountChapter;
            set
            {
                _labelCountChapter = value;
                OnPropertyChanged();
            }
        }

        private string _answer1;
        public string Answer1
        {
            get => _answer1;
            set
            {
                _answer1 = value;
                OnPropertyChanged();
            }
        }

        private string _answer2;
        public string Answer2
        {
            get => _answer2;
            set
            {
                _answer2 = value;
                OnPropertyChanged();
            }
        }

        private string _answer3;
        public string Answer3
        {
            get => _answer3;
            set
            {
                _answer3 = value;
                OnPropertyChanged();
            }
        }

        private string _answer4;
        public string Answer4
        {
            get => _answer4;
            set
            {
                _answer4 = value;
                OnPropertyChanged();
            }
        }

        private string _labelCorrectAnswers;
        public string LabelCorrectAnswers
        {
            get => _labelCorrectAnswers;
            set
            {
                _labelCorrectAnswers = value;
                OnPropertyChanged();
            }
        }

        private string _labelMistake;
        public string LabelMistake
        {
            get => _labelMistake;
            set
            {
                _labelMistake = value;
                OnPropertyChanged();
            }
        }

        private string _labelPercentage;
        public string LabelPercentage
        {
            get => _labelPercentage;
            set
            {
                _labelPercentage = value;
                OnPropertyChanged();
            }
        }

        public TestVM(Collection userCollection, IChapterProviders chapterProviders, IWordProviders wordProviders)
        {
            _chapterProviders = chapterProviders;
            _wordProviders = wordProviders;
            _userCollection = userCollection;

            StopCommand = new RelayCommand(StopTest);
            StartCommand = new RelayCommand(StartTest);
            AnswerCommand = new RelayCommand(CheckAnswer);
            OpenDataGridCommand = new RelayCommand(OpenDataGridWindow);


            InitializeAsync().ConfigureAwait(false);
        }

        private void OpenDataGridWindow(object parameter)
        {
            var dataGridWindow = new Learning_Words.Windows.DataGridWindowContainer(SelectedTestResult.Words);
            dataGridWindow.ShowDialog();
        }

        #region Initialization and Setup

        /// <summary>
        /// Asynchronously initializes the view model.
        /// </summary>
        private async Task InitializeAsync()
        {
            var chapters = await _chapterProviders.GetChaptersByCollectionAsync(_userCollection);

            foreach (var chapter in chapters)
            {
                if (chapter.Words.Count >= 4)
                {
                    AllChapters.Add(chapter);
                }
            }

            IsStartTest = false;
            IsSettingsTest = true;
            IsAnswerTranslationSelected = true;
            UpdateLabelsSetting();
        }

        private void UpdateLabelsSetting()
        {
            LabelCountChapter = STR_CHAPTER_COUNT + UsedChapters.Count;
            LabelCountWord = STR_WORD_COUNT + UsedChapters.Sum(chapter => chapter.Words.Count);
        }

        private void UpdateChapterUsage(Chapter chapter)
        {
            if (chapter == null) return;

            var isUsed = !chapter.IsUsed;
            chapter.IsUsed = isUsed;

            if (isUsed)
            {
                if (!UsedChapters.Contains(chapter))
                {
                    UsedChapters.Add(chapter);
                    AllChapters.Remove(chapter);
                    AllChapters.Insert(0, chapter);
                }
            }
            else
            {
                if (UsedChapters.Contains(chapter))
                {
                    UsedChapters.Remove(chapter);
                    AllChapters.Remove(chapter);
                    AllChapters.Add(chapter);
                }
            }

            UpdateLabelsSetting();
            SelectedChapter = null;
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Starts the test by shuffling words and enqueuing them.
        /// </summary>
        private void StopTest(object param)
        {
            IsSettingsTest = true;
            IsStartTest = false;
            TestResults.Clear();
            AnswerWords.Clear();
        }

        private void StartTest(object param)
        {
            _wordsQueue.Clear();
            ShuffleAndEnqueueWords();

            if (_wordsQueue.Count > 0)
            {
                var testResult = new TestResult
                {
                    Id = 0,
                    Name = "current test",
                    TestDate = DateTime.Now,
                    CorrectAnswers = 0,
                    Mistakes = 0,
                    Words = new List<Word>(AnswerWords)
                };
                TestResults.Add(testResult);
                SelectedTestResult = testResult;
                IsStartTest = true;
                IsSettingsTest = false;
                UpdateLabelsStatic();
                GenerateQuestion();
            }
        }
        private void ShuffleAndEnqueueWords()
        {
            var allWords = UsedChapters.SelectMany(chapter => chapter.Words).ToList();
            allWords = allWords.OrderBy(_ => _random.Next()).ToList();

            foreach (var word in allWords)
            {
                _wordsQueue.Enqueue(word);
            }
        }

        private void GenerateQuestion()
        {
            if (_wordsQueue.Count == 0)
            {
                EndTest();
                return;
            }

            _currentWord = _wordsQueue.Dequeue();
            var allWordsInCurrentChapter = UsedChapters
                .Where(ch => ch.Id == _currentWord.ChapterId)
                .SelectMany(ch => ch.Words)
                .ToList();

            _currentOptions = allWordsInCurrentChapter
                .OrderBy(_ => _random.Next())
                .Take(4)
                .ToList();

            if (!_currentOptions.Contains(_currentWord))
            {
                _currentOptions[_random.Next(4)] = _currentWord;
            }

            if (IsAnswerWordSelected)
            {
                SetQuestionWithWord();
            }
            else if (IsAnswerTranslationSelected)
            {
                SetQuestionWithTranslation();
            }
        }

        private void SetQuestionWithWord()
        {
            CurrentWordName = _currentWord.NameWord;
            CurrentTranscription = _currentWord.Transcription ?? "";
            SetAnswerOptions(_currentOptions.Select(w => w.Translation).ToList());
        }

        private void SetQuestionWithTranslation()
        {
            CurrentWordName = _currentWord.Translation;
            CurrentTranscription = "";
            SetAnswerOptions(_currentOptions.Select(w => w.NameWord).ToList());
        }

        private void SetAnswerOptions(List<string> options)
        {
            Answer1 = options.ElementAtOrDefault(0);
            Answer2 = options.ElementAtOrDefault(1);
            Answer3 = options.ElementAtOrDefault(2);
            Answer4 = options.ElementAtOrDefault(3);
        }

        private void EndTest()
        {
            var testResult = new TestResult
            {
                Id = TestResults.Count,
                Name = $"Test {TestResults.Count}",
                TestDate = DateTime.Now,
                CorrectAnswers = SelectedTestResult.CorrectAnswers,
                Mistakes = SelectedTestResult.Mistakes,
                Words = new List<Word>(AnswerWords)
            };

            TestResults.Add(testResult);
            ResetTest();
            GenerateQuestion();
        }

        private void ResetTest()
        {
            SelectedTestResult.CorrectAnswers = 0;
            SelectedTestResult.Mistakes = 0;
            SelectedTestResult.Words.Clear();
            AnswerWords.Clear();
            UpdateLabelsStatic();
            ShuffleAndEnqueueWords();
        }

        private void UpdateLabelsStatic()
        {
            if (SelectedTestResult == null) return;

            LabelCorrectAnswers = $"Correct Answers: {SelectedTestResult.CorrectAnswers}";
            LabelMistake = $"Mistakes: {SelectedTestResult.Mistakes}";

            int totalAnswers = SelectedTestResult.CorrectAnswers + SelectedTestResult.Mistakes;
            LabelPercentage = totalAnswers > 0
                ? $"Accuracy: {(double)SelectedTestResult.CorrectAnswers / totalAnswers * 100:F2}%"
                : "Accuracy: 0.00%";
        }

        /// <summary>
        /// Checks the given answer and generates the next question.
        /// </summary>
        private void CheckAnswer(object parameter)
        {
            if (SelectedTestResult?.Id != 0)
            {
                SelectedTestResult = TestResults.First(x => x.Id == 0);
            }

            if (parameter is string answer)
            {
                bool isCorrect = answer == (IsAnswerWordSelected ? _currentWord.Translation : _currentWord.NameWord);
                UpdateTestResult(isCorrect);
                GenerateQuestion();
            }
        }

        private void UpdateTestResult(bool isCorrect)
        {
            if (SelectedTestResult == null) return;

            if (isCorrect)
            {
                SelectedTestResult.CorrectAnswers++;
                _currentWord.IsCorrect = true;
            }
            else
            {
                SelectedTestResult.Mistakes++;
                _currentWord.IsCorrect = false;
            }

            AnswerWords.Insert(0, _currentWord);
            SelectedTestResult.Words = AnswerWords.ToList();
            UpdateLabelsStatic();
        }

        #endregion
    }
}
