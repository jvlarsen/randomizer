using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;

namespace Randomizer
{
    public class RandomizerEngine
    {
        public Dictionary<Participant, Event> EventOutcome;
        private DbFacade dbFacade;
        private List<Event> events;
        private List<Measure> measures;

        public RandomizerEngine()
        {
            dbFacade = DbFacade.GetInstance();
            events = dbFacade.GetEvents();
            EventOutcome = new Dictionary<Participant, Event>();
            measures = dbFacade.GetMeasures();
        }

        public Dictionary<string, string> Randomize(string triggerPlayerName, string eventNameFired, Dictionary<string, string> playersAndOwners)
        {
            Participant winner = dbFacade.GetOwnerFromPlayerName(triggerPlayerName);
            Event eventFired = events.FirstOrDefault(x => x.Name.ToLower() == eventNameFired.ToLower());
            List<Participant> owners = dbFacade.GetOwners();
            owners.Remove(winner);

            Dictionary<string, string> randomizerOutcome = new Dictionary<string, string>();

            var soundPlayer = new SoundPlayer(eventFired.SoundUrl);
            soundPlayer.Play();

            var random = new Random();
            int outcomeIndex = 0;
            var outcomeMeasure = "";

            var smallRangeTop = eventFired.Measure.Small;
            var mediumRangeTop = eventFired.Measure.Medium + smallRangeTop;
            var largeRangeTop = eventFired.Measure.Large + mediumRangeTop;
            var walterRangeTop = eventFired.Measure.Walter + largeRangeTop;

            foreach (var loser in owners)
            {
                outcomeIndex = random.Next(1, 101);

                    if (1 <= outcomeIndex && outcomeIndex <= smallRangeTop)
                    {
                        outcomeMeasure = "Small";
                    }
                    else if (smallRangeTop < outcomeIndex && outcomeIndex <= mediumRangeTop)
                    {
                        outcomeMeasure = "Memmel";
                    }
                    else if (mediumRangeTop < outcomeIndex && outcomeIndex <= largeRangeTop)
                    {
                        outcomeMeasure = "Large";
                    }
                    else if (largeRangeTop < outcomeIndex && outcomeIndex <= walterRangeTop)
                    {
                        outcomeMeasure = "Walter";
                    }
                    randomizerOutcome.Add(loser.Name, outcomeMeasure);
            }
            return randomizerOutcome;
        }

        public Dictionary<string, string> DistributeTeams(List<string> players, List<string> participants, string gameName)
        {
            var countParticipants = participants.Count;
            var playersAndOwners = new Dictionary<string, string>();
            var random = new Random();
            var index = 0;
            for (int j = 0; j < 22; j++)
            {
                var currentParticipant = participants.ElementAt(j % participants.Count);
                if (players.Count == 1)
                    index = 0;
                else
                { 
                    index = random.Next(players.Count - 1);
                }
                var selectedPlayer = players.ElementAt(index);
                playersAndOwners.Add(selectedPlayer, currentParticipant);
                players.Remove(selectedPlayer);
            }

            dbFacade.SaveDistribution(playersAndOwners, gameName);
            return playersAndOwners;
        }

        private Participant GetOwnerFromPlayerName(string triggerPlayerName)
        {
            dbFacade.GetOwnerFromPlayerName(triggerPlayerName);
            var p = new Participant("Jesper");

            return p;
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames)
        {
            dbFacade.SaveParticipants(participantNames);
        }

        public void SaveDistribution(Dictionary<string, string> playersAndOwners, string gameName)
        {
            dbFacade.SaveDistribution(playersAndOwners, gameName);
        }


    }
}
