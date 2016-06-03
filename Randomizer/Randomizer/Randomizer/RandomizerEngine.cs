using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;
using System.Data.SqlClient;

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

        public Dictionary<string, int> Randomize(string triggerPlayerName, string eventNameFired, Dictionary<string, string> playersAndOwners, string gameName, int gameMinute)
        {
            Participant winner = dbFacade.GetOwnerFromPlayerName(triggerPlayerName, gameName);
            Event eventFired = events.FirstOrDefault(x => x.Name.ToLower() == eventNameFired.ToLower());
            List<Participant> owners = dbFacade.GetParticipants();
            var winnersIndex = owners.IndexOf(owners.First(x => x.Name == winner.Name));
            if (eventFired.Measure.Name.Substring(0, 3).ToLower().Equals("own"))
            {
                owners = new List<Participant>() { owners[winnersIndex] };
            }
            else
            {
                owners.RemoveAt(winnersIndex);
            }
            Dictionary<string, int> randomizerOutcome = new Dictionary<string, int>();

            var soundPlayer = new SoundPlayer(eventFired.SoundUrl);
            soundPlayer.Play();

            var random = new Random();
            int outcomeIndex = 0;
            var outcomeMeasure = 0;

            var smallRangeTop = eventFired.Measure.Small;
            var mediumRangeTop = eventFired.Measure.Medium + smallRangeTop;
            var largeRangeTop = eventFired.Measure.Large + mediumRangeTop;
            var walterRangeTop = eventFired.Measure.Walter + largeRangeTop;

            foreach (var loser in owners)
            {
                outcomeIndex = random.Next(1, 101);

                    if (1 <= outcomeIndex && outcomeIndex <= smallRangeTop)
                    {
                        outcomeMeasure = 1;
                    }
                    else if (smallRangeTop < outcomeIndex && outcomeIndex <= mediumRangeTop)
                    {
                        outcomeMeasure = 3;
                    }
                    else if (mediumRangeTop < outcomeIndex && outcomeIndex <= largeRangeTop)
                    {
                        outcomeMeasure = 6;
                    }
                    else if (largeRangeTop < outcomeIndex && outcomeIndex <= walterRangeTop)
                    {
                        outcomeMeasure = 11;
                    }
                    randomizerOutcome.Add(loser.Name, outcomeMeasure);
            }
            dbFacade.LogRandomizingOutcome(gameName, randomizerOutcome, gameMinute);
            return randomizerOutcome;
        }

        public Dictionary<string, string> DistributeTeams(List<string> players, List<string> participants, string gameName)
        {
            var countParticipants = participants.Count;
            var playersAndOwners = new Dictionary<string, string>();
            var random = new Random();
            var index = 0;

            for (int j = 0; j < 21; j++)
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

        private Dictionary<string, string> ShuffleParticipants(List<string> participants)
        {
            var shuffledList = new Dictionary<string, string>();

            return shuffledList;
        }

        private Participant GetOwnerFromPlayerName(string triggerPlayerName, string gameName)
        {
            dbFacade.GetOwnerFromPlayerName(triggerPlayerName, gameName);
            var p = new Participant("Jesper");

            return p;
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames)
        {
            dbFacade.SaveParticipants(participantNames);
        }

        public void SaveNewGame(string gameName, DateTime gameDate)
        {
            dbFacade.SaveNewGame(gameName, gameDate);
        }

        public Dictionary<int, Dictionary<int, int>> CalculateGraph(string matchId)
        {
            return dbFacade.CalculateGraph(matchId);
        }

        public void UndoLatestEvent()
        {
            dbFacade.UndoLatestEvent();
        }

    }
}
