using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string NameWord { get; set; } 
        public string Translation { get; set; }  
        public string? Transcription { get; set; }

        public bool? IsCorrect { get; set; } = null;

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } 
    }

}
