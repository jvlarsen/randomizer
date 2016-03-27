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
        private List<Event> events;

        public RandomizerEngine()
        {
            dbFacade = DbFacade.GetInstance();
            events = dbFacade.GetEvents();
            EventOutcome = new Dictionary<Participant, Event>();
        }

        public void Randomize(string triggerPlayerName, string eventNameFired)
        {
            Participant winner = GetOwnerFromPlayerName(triggerPlayerName);
            Event eventFired = events.FirstOrDefault(x => x.Name == eventNameFired);
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
