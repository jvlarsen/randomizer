using Randomizer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Randomizer
{
    public class DbFacade
    {
        private string connectionString;
        private SqlConnection conn;
        private static DbFacade dbFacade;

        private DbFacade()
        { 
            connectionString = @"Data Source=JVL\SQLEXPRESS;Initial Catalog=RandomDb;Integrated Security=True";
            conn = new SqlConnection(connectionString);
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
                events.Add(new Event((string)reader["EventName"], (string)reader["Measure"], (string)reader["SoundClipUrl"]));
            }
            reader.Close();
            CloseConn();
            return events;
        }

        public Participant GetOwnerFromPlayerName(string playerName)
        {
            //var sql = new RandomDbDataSetTableAdapters.OwnersTableAdapter();
            var p = new Participant() { Name = "Faccio" };
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

        public void SaveDistribution(Dictionary<string, string> playersAndOwners, string gameName)
        {

        }

    }
}
