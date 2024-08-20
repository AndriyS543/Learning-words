using Learning_Words.Models;
using Learning_Words.Repositories.WordProviders;
using Learning_Words.Repositories.ChapterRroviders;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Learning_Words.Services
{
    public class Creator:ICreator
    {
        private readonly IWordProviders _wordProvider;
        private readonly IChapterProviders _chapterProvider;

        public Creator(IWordProviders wordProvider, IChapterProviders chapterProvider)
        {
            _wordProvider = wordProvider;
            _chapterProvider = chapterProvider;
        }

        public async Task<Word> CreateWordAsync(string nameWord, string translate, Chapter chapter, string? transcription = null)
        {
            var word = new Word
            {
                NameWord = nameWord,
                Translation = translate,
                Transcription = transcription,
                ChapterId = chapter.Id
            };

            await _wordProvider.AddAsync(word);
            return word;
        }

        public async Task<Chapter> CreateChapterAsync(string nameChapter, Collection collection, string? description = null)
        {
            var chapter = new Chapter
            {
                NameChapter = nameChapter,
                Description = description,
                CollectionId = collection.Id
            };

            await _chapterProvider.AddAsync(chapter);
            return chapter;
        }
    }
}
