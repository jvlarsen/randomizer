using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;

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
            connectionString = @"Data Source=JVL\SQLEXPRESS;Initial Catalog=RandomDb;Integrated Security=True";
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
            conn.Open();
        }

        public void CloseConn()
        {
            conn.Close();
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

        public List<Participant> GetOwners()
        {
            var owners = new List<Participant>();
            //var sql = new RandomDbDataSetTableAdapters.OwnersTableAdapter();
            //var ownersTable = sql.GetData();
            //foreach (var owner in ownersTable)
            //{
            //    owners.Add(new Participant() { Name = owner.Name }); //TODO: Make the SQL extract join Owners with Participants to get the P.Name
            //}
            return owners;
        }

        public List<Participant> GetParticipants()
        {
            OpenConn();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ParticipantId, Name FROM Participant";

            var participants = new List<Participant>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                participants.Add(new Participant((string)reader["Name"]));
            }
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
        }

        public void SaveParticipants(Dictionary<string, Color> participantNames)
        {

        }

    }
}
