using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Player
    {
        string name { get; set; }

        Participant owner { get; set; }
    }
}
