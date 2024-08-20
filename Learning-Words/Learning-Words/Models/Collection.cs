using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
