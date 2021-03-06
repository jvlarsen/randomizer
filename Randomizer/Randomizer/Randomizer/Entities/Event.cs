﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Event
    {
        public string Name { get; set; }
        public Measure Measure { get; set; }
        public string SoundUrl { get; set; }
        public string RefereeSoundUrl { get; set; }

        public Event() { }

        public Event(string name, Measure measure, string soundUrl, string refereeSoundUrl)
        {
            Name = name;
            Measure = measure;
            SoundUrl = soundUrl;
            RefereeSoundUrl = refereeSoundUrl;
        }
    }
}
