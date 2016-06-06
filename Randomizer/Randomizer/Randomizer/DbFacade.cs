using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Data;

namespace Randomizer
{
    public class DbFacade
    {
        private string connectionString;
        private SqlConnection conn;
        private static DbFacade dbFacade;
        public List<Measure> measures;
        public List<Participant> participants;

        private DbFacade()
        { 
            //Should make a manual insert of all Participants, Events and Measures.
            //Or make sure to always remember to do it manually in SSMS before putting the Randomizer to user

            connectionString = @"Data Source=JVLHOME\SQLEXPRESS;Initial Catalog=RandomDb;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            GetMeasures();
        }

        public static DbFacade GetInstance()
        {
            if (dbFacade == null)
                dbFacade = new DbFacade();

            return dbFacade;
        }

        public void OpenConn()
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        public void CloseConn()
        {
            conn.Close();
        }

        public SqlConnection GetConn()
        {
            return conn;
        }

        public List<Event> GetEvents()
        {
            var events = new List<Event>();
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Events";

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var currentMeasure = measures.First(x => x.Name == (string)reader["Measure"]);
                events.Add(new Event((string)reader["EventName"], currentMeasure, (string)reader["SoundClipUrl"]));
            }
            reader.Close();
            CloseConn();
            return events;
        }

        public List<Measure> GetMeasures()
        {
            measures = new List<Measure>();
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Measure, Small, Medium, Large, Walter FROM Measures";

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                measures.Add(new Measure((string)reader["Measure"], (int)reader["Small"], (int)reader["Medium"], (int)reader["Large"], (int)reader["Walter"]));
            }
            CloseConn();
            return measures;
        }

        public Participant GetOwnerFromPlayerName(string playerName, int matchId)
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetOwnerFromPlayer";

            var matchParam = new SqlParameter("@matchId", SqlDbType.Int);
            matchParam.Value = matchId;
            cmd.Parameters.Add(matchParam);

            var playerParam = new SqlParameter("@playerName", SqlDbType.VarChar);
            playerParam.Value = playerName;
            cmd.Parameters.Add(playerParam);

            var reader = cmd.ExecuteReader();
            Participant p = null;

            if (reader.Read())
            {
                p = new Participant((string)reader["Name"]);
            }
            CloseConn();
            return p;
        }

        public List<Participant> GetParticipants()
        {
            participants = new List<Participant>();
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ParticipantId, Name FROM Participants";

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                participants.Add(new Participant((string)reader["Name"]));
            }
            CloseConn();

            return participants;
        }

        public Participant GetParticipantFromName(string participantName)
        {
            var participants = GetParticipants();
            return participants.First(x => x.Name == participantName);
        }

        public void SaveDistribution(Dictionary<string, string> playersAndOwners, int matchId)
        {
            for (int i = 0; i < playersAndOwners.Keys.Count; i++)
            {
                SavePlayer(playersAndOwners.ElementAt(i).Key, matchId, i);
            }
            OpenConn();
            foreach (KeyValuePair<string, string> playerOwner in playersAndOwners)
            {
                
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapPlayerToOwner";

                SqlParameter matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
                matchIdParam.Value = matchId;
                cmd.Parameters.Add(matchIdParam);

                SqlParameter playerName = new SqlParameter("@playerName", SqlDbType.VarChar);
                cmd.Parameters.Add(playerName);
                cmd.Parameters["@playerName"].Value = playerOwner.Key;

                SqlParameter ownerName = new SqlParameter("@ownerName", SqlDbType.VarChar);
                cmd.Parameters.Add(ownerName);
                cmd.Parameters["@ownerName"].Value = playerOwner.Value;

                cmd.ExecuteNonQuery();
            }
            CloseConn();
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames)
        {
            //TODO: Ville være fedt at kunne lave deltagerne generiske og mappe på participantId og deres positioner.
            //Vil bl.a. kræve at labels omdøbes og mappes til player1 -> labelDrinkOk1 osv.
            var buM = participantNames.ElementAt(0).Value.ToArgb();
            var newBum = Color.FromArgb(buM);

        }

        public void SavePlayer(string playerName, int matchId, int playerIndex)
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Players (Name, MatchId, PlayerIndex) VALUES (\'" + playerName + "\', " + matchId + ", " + playerIndex + ")";
            cmd.ExecuteNonQuery();
            CloseConn();
        }

        public int SaveNewGame(string teamNames, DateTime gameDate)
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Matches (TeamNames, Created) VALUES (\'" + teamNames + "\', \'" + String.Format("{0:yyyy-MM-dd}", gameDate) + "\')";
            cmd.ExecuteNonQuery();

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ISNULL(MatchId,1)) AS MatchId FROM Matches";
            var reader = cmd.ExecuteReader();
            reader.Read();
            var matchId = (int)reader["MatchId"];
            CloseConn();

            return matchId;
        }

        public List<SaveGame> GetSaveGames()
        {
            var saveGames = new List<SaveGame>();
            OpenConn();
            //var cmd = conn.CreateCommand();
            //cmd.CommandText = "SELECT MatchId, TeamNames, Created FROM Matches ORDER BY Created DESC";
            //var reader = cmd.ExecuteReader();

            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetOldGames";

            var reader = cmd.ExecuteReader();
            //M.MatchId, M.TeamNames, M.Created, MAX(EventNumber) AS LatestEvent
            while (reader.Read())
            {
                var matchId = (int)reader["MatchId"];
                var gameDate = (DateTime)reader["Created"];
                var matchItem = reader["TeamNames"] + " (" + gameDate.ToShortDateString() + ")";
                var latestEvent = (int)reader["LatestEvent"];
                var saveGame = new SaveGame(matchId);
                saveGame.TeamNames = matchItem;
                saveGame.GameDate = gameDate;
                saveGame.LatestEvent = latestEvent;
                saveGames.Add(saveGame);
            }
            CloseConn();
            return saveGames;
        }

        public List<Player> LoadPlayersFromSaveGame(SaveGame saveGame)
        {//Jeg skal ende med en liste af Players, der alle har deres Name, PlayerIndex og Participant sat.
            //Det kan hentes i én SP.


            var playersFromSaveGame = new List<Player>();
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetPlayersAndOwners";

            var matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
            matchIdParam.Value = saveGame.MatchId;
            cmd.Parameters.Add(matchIdParam);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var playerName = (string)reader["PlayerName"];
                var owner = new Participant((string)reader["Ownername"]);
                var playerToAdd = new Player(playerName, (int)reader["PlayerIndex"]);
                playerToAdd.Owner = owner;
                playersFromSaveGame.Add(playerToAdd);
            }
            CloseConn();
            return playersFromSaveGame;
        }

        //private Participant GetOwnerFromPlayerAndGame(SaveGame saveGame, string playerName)
        //{
        //    OpenConn();
        //    var cmd = conn.CreateCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "GetOwnerFromPlayerAndGame";

        //    var matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
        //    matchIdParam.Value = saveGame.MatchId;
        //    cmd.Parameters.Add(matchIdParam);

        //    var playerNameParam = new SqlParameter("@PlayerName", SqlDbType.VarChar);
        //    playerNameParam.Value = playerName;
        //    cmd.Parameters.Add(playerNameParam);

        //    var reader = cmd.ExecuteReader();

        //    Participant owner = null;
        //    while (reader.Read())
        //    {
        //        owner = new Participant((string)reader["Name"]);
        //        var red = (int)reader["Red"];
        //        var green = (int)reader["Green"];
        //        var blue = (int)reader["Blue"];
        //        owner.BackColor = Color.FromArgb(red, green, blue);
        //    }
        //    CloseConn();
        //    return owner;
        //}

        public Dictionary<string, string> GetPlayersAndOwners(SaveGame saveGame)
        {
            var playersAndOwners = new Dictionary<string, string>();
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetPlayersAndOwners";

            var matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
            matchIdParam.Value = saveGame.MatchId;
            cmd.Parameters.Add(matchIdParam);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                playersAndOwners.Add((string)reader["PlayerName"], (string)reader["OwnerName"]);
            }
            CloseConn();
            return playersAndOwners;
        }

        public Dictionary<int, List<GraphPoint>> CalculateGraph(int matchId)
        {
            var participantsAndTheirGraph = new Dictionary<int, List<GraphPoint>>();
            OpenConn();
            var sProc = conn.CreateCommand();
            sProc.CommandType = CommandType.StoredProcedure;
            sProc.CommandText = "CalculateGraph";

            var matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
            matchIdParam.Value = matchId;
            sProc.Parameters.Add(matchIdParam);            

            SqlDataReader reader = sProc.ExecuteReader();

            while (reader.Read())
            {
                var participantId = (int)reader["ParticipantId"];
                if (!participantsAndTheirGraph.Keys.Contains(participantId))
                {
                    participantsAndTheirGraph.Add(participantId, new List<GraphPoint>());
                    participantsAndTheirGraph[participantId].Add(new GraphPoint(0, 0, ""));
                }

                participantsAndTheirGraph[participantId].Add(new GraphPoint((int)reader["GameMinute"], (int)reader["CurrentTotal"], (string)reader["EventText"]));
                
            }
            CloseConn();
            return participantsAndTheirGraph;
        }

        public void LogRandomizingOutcome(int matchId, Dictionary<string, Randomizer.RandomizerEngine.MeasureName> randomizerOutcome, int gameMinute, Event eventFired)
        {
            //The randomizerOutcome contains <loserName, measures> and should be mapped to dbo.Graph 
            //as (MatchId, ParticipantId, GameMinute, Measure)
            OpenConn();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ISNULL(MAX(EventNumber),0) + 1 AS EventNumber FROM Graph";
            var reader = cmd.ExecuteReader();
            reader.Read();
            var eventNumber = (int)reader["EventNumber"];
            CloseConn();

            foreach (KeyValuePair<string, Randomizer.RandomizerEngine.MeasureName> outcome in randomizerOutcome)
            {
                OpenConn();
                var sProc = conn.CreateCommand();
                sProc.CommandType = CommandType.StoredProcedure;
                sProc.CommandText = "LogRandomizingOutcomeToGraph";

                var matchIdParam = new SqlParameter("@MatchId", SqlDbType.Int);
                matchIdParam.Value = matchId;
                sProc.Parameters.Add(matchIdParam);

                var loserParam = new SqlParameter("@ownerName", SqlDbType.VarChar);
                loserParam.Value = outcome.Key;
                sProc.Parameters.Add(loserParam);

                var zipsParam = new SqlParameter("@zips", SqlDbType.Int);
                zipsParam.Value = (int)outcome.Value;
                sProc.Parameters.Add(zipsParam);

                var timeParam = new SqlParameter("@Time", SqlDbType.Int);
                timeParam.Value = gameMinute;
                sProc.Parameters.Add(timeParam);

                var eventNumberParam = new SqlParameter("@EventNumber", SqlDbType.Int);
                eventNumberParam.Value = eventNumber;
                sProc.Parameters.Add(eventNumberParam);

                var eventTextParam = new SqlParameter("@EventText", SqlDbType.VarChar);
                eventTextParam.Value = eventFired.Name;
                sProc.Parameters.Add(eventTextParam);

                sProc.ExecuteNonQuery();
                CloseConn();
            }
        }

        public void UndoLatestEvent()
        {
            OpenConn();
            var sProc = conn.CreateCommand();
            sProc.CommandType = CommandType.StoredProcedure;
            sProc.CommandText = "UndoLatest";

            sProc.ExecuteNonQuery();
            CloseConn();
        }

        public enum Measures
        {
            Small = 1,
            Medium = 3,
            Large = 6,
            Walter = 11
        }
    }
}
