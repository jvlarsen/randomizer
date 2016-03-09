using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
    public class DbFacade
    {
        private string connectionString;

        public DbFacade()
        { }

        public List<Event> GetEvents()
        {
            var events = new List<Event>();
            var sql = new RandomDbDataSetTableAdapters.EventsTableAdapter();
            var eventsTable = sql.GetData();
            
            foreach (var drinkEvent in eventsTable)
            {
                var soundUrl = string.IsNullOrEmpty(drinkEvent.SoundClipUrl) ? "" : drinkEvent.SoundClipUrl;
                events.Add(new Event(drinkEvent.EventNumber, drinkEvent.EventName, drinkEvent.NumberOfSips, soundUrl));    
            }
            return events;
        }

        public Participant GetOwnerFromPlayerName(string playerName)
        {
            var sql = new RandomDbDataSetTableAdapters.OwnersTableAdapter();
            var p = new Participant() { Name = "Faccio" };
            return p;
        }

        public List<Participant> GetOwners()
        {
            var owners = new List<Participant>();
            var sql = new RandomDbDataSetTableAdapters.OwnersTableAdapter();
            var ownersTable = sql.GetData();
            foreach (var owner in ownersTable)
            {
                owners.Add(new Participant() { Name = owner.Name }); //TODO: Make the SQL extract join Owners with Participants to get the P.Name
            }
        }

    }
}
