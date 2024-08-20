using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TestDate { get; set; }
        public int CorrectAnswers { get; set; }
        public int Mistakes { get; set; }
        public List<Word> Words { get; set; } = new List<Word>();
    }

}
