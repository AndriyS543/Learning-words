using Learning_Words.Models;

namespace Learning_Words.Services
{
    public interface ICreator
    {
        Task<Word> CreateWordAsync(string nameWord, string translate, Chapter chapter, string? transcription = null);
        Task<Chapter> CreateChapterAsync(string nameChapter, Collection collection, string? description = null);
    }
}
