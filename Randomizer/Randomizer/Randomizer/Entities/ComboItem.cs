using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class ComboItem
    {
        public string Presentation { get; set; }
        public string Value { get; set; }

        public ComboItem(string presentation, string value)
        {
            Presentation = presentation;
            Value = value;
        }

        public override string ToString()
        {
            return Presentation;
        }
    }
}
