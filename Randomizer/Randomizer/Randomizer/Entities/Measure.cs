using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Measure
    {
        public string Name { get; set; }
        public int Small { get; set; }
        public int Medium { get; set; }
        public int Large { get; set; }
        public int Walter { get; set; }
        
        public Measure(string name, int small, int medium, int large, int walter)
        {
            this.Name = name;
            this.Small = small;
            this.Medium = medium;
            this.Large = large;
            this.Walter = walter;
        }
    }
}
