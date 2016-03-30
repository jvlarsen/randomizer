﻿using Randomizer.Entities;
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

        public Dictionary<string, string> DistributeTeams(List<string> players, List<string> participants)
        {
            var countParticipants = participants.Count;
            var playersAndOwners = new Dictionary<string, string>();
            var random = new Random();
            var index = 0;
            for (int j = 0; j < 22; j++)
            {
                var currentParticipant = participants.ElementAt(j % 7);
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

            //Call dbFacade here to store the playersAndOwners mapping

            return playersAndOwners;
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
