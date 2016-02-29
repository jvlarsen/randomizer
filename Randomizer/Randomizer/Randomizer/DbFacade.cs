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

    }
}
