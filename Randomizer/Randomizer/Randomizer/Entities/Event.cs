using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Event
    {
        int EventNumber { get; set; }
        string Name { get; set; }
        int Sips { get; set; }
        string SoundUrl { get; set; }

        public Event() { }

        public Event(int number, string name, int sips, string soundUrl = "")
        {
            EventNumber = number;
            Name = name;
            Sips = sips;
            SoundUrl = soundUrl;
        }
    }
}
