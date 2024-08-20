using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string NameChapter { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = false;
        public int CollectionId { get; set; } 
        public Collection? Collection { get; set; }

        public List<Word> Words { get; set; } = [];
    }
}
