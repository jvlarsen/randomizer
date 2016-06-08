using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string radioButton { get; set; }
        public Color radioColor { get; set; }

        public Player()
        { }

        public Player(string name, int playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
        }
    }
}
