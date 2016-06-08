using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;

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

        public Dictionary<string, MeasureName> Randomize(string triggerPlayerName, string eventNameFired, int matchId, int gameMinute)
        {
            Participant ownerOfTriggerPlayer = dbFacade.GetOwnerFromPlayerName(triggerPlayerName, matchId);
            Event eventFired = events.FirstOrDefault(x => x.Name.ToLower() == eventNameFired.ToLower());
            List<Participant> losers = dbFacade.GetParticipants(matchId);
            List<Participant> winners = new List<Participant>();
            var winnersIndex = losers.IndexOf(losers.First(x => x.Name == ownerOfTriggerPlayer.Name));
            if (eventFired.Measure.Name.Substring(0, 3).ToLower().Equals("own"))
            {
                winners = losers.Where(x => x.Name != ownerOfTriggerPlayer.Name).ToList();
                losers = new List<Participant>() { losers[winnersIndex] };      // Egen                   
            }
            else
            {
                winners.Add(losers.ElementAt(winnersIndex));
                losers.RemoveAt(winnersIndex); //Andre
            }
            Dictionary<string, MeasureName> randomizerOutcome = new Dictionary<string, MeasureName>();

            SoundPlayer soundPlayer;
            if (triggerPlayerName.Equals("Referee"))
            {
                soundPlayer = new SoundPlayer(eventFired.RefereeSoundUrl);
            }
            else
            {
                soundPlayer = new SoundPlayer(eventFired.SoundUrl);
            }
            soundPlayer.Play();

            var random = new Random();
            int outcomeIndex = 0;
            MeasureName outcomeMeasure = MeasureName.Lille;

            var smallRangeTop = eventFired.Measure.Small;
            var mediumRangeTop = eventFired.Measure.Medium + smallRangeTop;
            var largeRangeTop = eventFired.Measure.Large + mediumRangeTop;
            var walterRangeTop = eventFired.Measure.Walter + largeRangeTop;

            foreach (var loser in losers)            {
                
                    outcomeIndex = random.Next(1, 101);

                    if (1 <= outcomeIndex && outcomeIndex <= smallRangeTop)
                    {
                        outcomeMeasure = MeasureName.Lille;
                    }
                    else if (smallRangeTop < outcomeIndex && outcomeIndex <= mediumRangeTop)
                    {
                        outcomeMeasure = MeasureName.Memmel;
                    }
                    else if (mediumRangeTop < outcomeIndex && outcomeIndex <= largeRangeTop)
                    {
                        outcomeMeasure = MeasureName.Stor;
                    }
                    else if (largeRangeTop < outcomeIndex && outcomeIndex <= walterRangeTop)
                    {
                        outcomeMeasure = MeasureName.Walter;
                    }
                
                randomizerOutcome.Add(loser.Name, outcomeMeasure);
            }
            foreach (var winningParticipant in winners)
            {
                randomizerOutcome.Add(winningParticipant.Name, 0);
            }


            dbFacade.LogRandomizingOutcome(matchId, randomizerOutcome, gameMinute, eventFired);
            return randomizerOutcome;
        }

        public enum MeasureName
        {
            Lille = 1,
            Memmel = 3,
            Stor = 6,
            Walter = 11
        }

        public Dictionary<Player, string> DistributeTeams(List<Player> players, List<string> participants, int matchId)
        {
            var countParticipants = participants.Count;
            var playersAndOwners = new Dictionary<Player, string>();
            var random = new Random();
            var index = 0;
            var startIndex = random.Next(countParticipants);

            for (int j = 0; j < 21; j++)
            {
                var currentParticipant = participants.ElementAt((j+startIndex) % (participants.Count));
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

            dbFacade.SaveDistribution(playersAndOwners, matchId);
            return playersAndOwners;
        }

        private Dictionary<string, string> ShuffleParticipants(List<string> participants)
        {
            var shuffledList = new Dictionary<string, string>();

            return shuffledList;
        }

        public void UpdatePlayerName(string name, string radioButton, int matchId)
        {
            dbFacade.UpdatePlayerName(name, radioButton, matchId);
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames, int matchId)
        {
            dbFacade.SaveParticipants(participantNames, matchId);
        }

        public int SaveNewGame(string teamNames, DateTime gameDate)
        {
            return dbFacade.SaveNewGame(teamNames, gameDate);
        }

        public Dictionary<int, List<GraphPoint>> CalculateGraph(int matchId)
        {
            return dbFacade.CalculateGraph(matchId);
        }

        public void UndoLatestEvent()
        {
            dbFacade.UndoLatestEvent();
        }

        public List<SaveGame> GetSaveGames()
        {
            return dbFacade.GetSaveGames();
        }

        public List<Measure> GetMeasures()
        {
            return dbFacade.GetMeasures();
        }

        public List<Player> LoadPlayersFromSaveGame(SaveGame saveGame)
        {
            return dbFacade.LoadPlayersFromSaveGame(saveGame);
        }

        public Dictionary<string, Color> LoadParticipantsFromMatchId(int matchId)
        {
            return dbFacade.LoadParticipantsFromMatchId(matchId);
        }

        public Dictionary<Player, string> GetPlayersAndOwners(SaveGame saveGame)
        {
            return dbFacade.GetPlayersAndOwners(saveGame);
        }
    }
}
