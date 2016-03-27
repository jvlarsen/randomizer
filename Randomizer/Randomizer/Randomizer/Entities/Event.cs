using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Event
    {
        public string Name { get; set; }
        public string Measure { get; set; }
        public string SoundUrl { get; set; }

        public Event() { }

        public Event(string name, string measure, string soundUrl)
        {
            Name = name;
            Measure = measure;
            SoundUrl = soundUrl;
        }
    }
}
