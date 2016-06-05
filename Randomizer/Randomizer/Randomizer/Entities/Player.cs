using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public int PlayerIndex;
        public Participant Owner { get; set; }

        public Player(string name, int playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
        }
    }
}
