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

        private DbFacade()
        { 
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

        public Participant GetOwnerFromPlayerName(string playerName)
        {
            //var sql = new RandomDbDataSetTableAdapters.OwnersTableAdapter();
            var p = new Participant("Faccio");
            return p;
        }

        public List<Participant> GetParticipants()
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ParticipantId, Name FROM Participants";

            var participants = new List<Participant>();
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

        public void SaveDistribution(Dictionary<string, string> playersAndOwners, string gameName)
        {
            //TODO: Make the mapping here from playersAndOwners to find database records representing the entities Player and Participant and map them in the table Owners

            foreach (var player in playersAndOwners.Keys)
            {
                SavePlayer(player, gameName);
            }
            foreach (KeyValuePair<string, string> playerOwner in playersAndOwners)
            {
                OpenConn();
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MapPlayerToOwner";

                SqlParameter matchId = new SqlParameter("@MatchId", SqlDbType.VarChar);
                cmd.Parameters.Add(matchId);
                cmd.Parameters["@MatchId"].Value = gameName;

                SqlParameter playerName = new SqlParameter("@playerName", SqlDbType.VarChar);
                cmd.Parameters.Add(playerName);
                cmd.Parameters["@playerName"].Value = playerOwner.Key;

                SqlParameter ownerName = new SqlParameter("@ownerName", SqlDbType.VarChar);
                cmd.Parameters.Add(ownerName);
                cmd.Parameters["@ownerName"].Value = playerOwner.Value;

                cmd.ExecuteNonQuery();
            }
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames)
        {

        }

        public void SavePlayer(string playerName, string gameName)
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Players (Name, GameName) VALUES (\'" + playerName + "\', " + "\'" + gameName + "\')";
            cmd.ExecuteNonQuery();
            CloseConn();
        }

        public void SaveNewGame(string gameName, DateTime gameDate)
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Matches (Description, Created) VALUES (\'" + gameName + "\', \'" + String.Format("{0:yyyy-MM-dd}", gameDate) + "\')";
            cmd.ExecuteNonQuery();
            CloseConn();
        }

        public Dictionary<int, Dictionary<int, int>> CalculateGraph(string matchId)
        {
            var participantsAndTheirGraph = new Dictionary<int, Dictionary<int, int>>();
            OpenConn();
            var sProc = conn.CreateCommand();
            sProc.CommandType = CommandType.StoredProcedure;
            sProc.CommandText = "CalculateGraph";

            SqlParameter currentMatchId = new SqlParameter("@matchId", SqlDbType.VarChar);

            sProc.Parameters.Add(currentMatchId);

            sProc.Parameters["@matchId"].Value = matchId;

            SqlDataReader reader = sProc.ExecuteReader();

            while (reader.Read())
            {
                var participantId = (int)reader["ParticipantId"];
                if (!participantsAndTheirGraph.Keys.Contains(participantId))
                {
                    participantsAndTheirGraph.Add(participantId, new Dictionary<int, int>());
                    participantsAndTheirGraph[participantId].Add(0, 0);
                }
                participantsAndTheirGraph[participantId].Add((int)reader["GameMinute"], (int)reader["CurrentTotal"]);
            }
            CloseConn();
            return participantsAndTheirGraph;
        }

        public void LogRandomizingOutcome(string gameName, Dictionary<string, string> randomizerOutcome, int gameMinute)
        {
            //The randomizerOutcome contains <loserName, measures> and should be mapped to dbo.Graph 
            //as (MatchId, ParticipantId, GameMinute, Measure)
            var str = "";
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
    }
}
