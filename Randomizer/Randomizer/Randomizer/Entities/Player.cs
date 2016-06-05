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
        public string Color;
        public int PlayerIndex;
        public Participant Owner { get; set; }

        public Player(string name, string color, int playerIndex)
        {
            Name = name;
            Color = color;
            PlayerIndex = playerIndex;
        }
    }
}
