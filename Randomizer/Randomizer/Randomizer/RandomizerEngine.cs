using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
    public class RandomizerEngine
    {
        public Dictionary<Participant, Event> EventOutcome;
        private DbFacade dbFacade;

        public RandomizerEngine()
        {
            dbFacade = new DbFacade();
            EventOutcome = new Dictionary<Participant, Event>();
        }

        public void Randomize(string triggerPlayerName, string eventFired)
        {
            Participant winner = GetOwnerFromPlayerName(triggerPlayerName);
            List<Participant> owners = dbFacade.GetOwners();

            foreach (var loser in owners)
            {
                
            }
        }

        private Participant GetOwnerFromPlayerName(string triggerPlayerName)
        {
            dbFacade.GetOwnerFromPlayerName(triggerPlayerName);
            var p = new Participant(){
                Name = "Jesper"
            };
            return p;
        }


    }
}
