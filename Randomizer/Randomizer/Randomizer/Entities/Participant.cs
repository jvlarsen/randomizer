using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Participant
    {
        public string Name { get; set; }

        public Participant(string name)
        {
            this.Name = name;
        }
    }
}
