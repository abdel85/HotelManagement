using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMangement.Classes
{
    public class Hotel
    {
        private int id;
        private string name;
        private int roomNb;
        private SqlCommand command;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int RoomNb { get => roomNb; set => roomNb = value; }
        public SqlCommand Command { get => command; set => command = value; }

        public Hotel()
        {
            
        }

        public Hotel(string name)
        {
            command = new SqlCommand("SELECT * FORM Hotel WHERE Name = @name", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", name));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Name = reader.GetString(1);
                RoomNb = reader.GetInt32(2);

            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
 
        }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand("INSERT INTO Hotel(Name,RoomNb) OUTPUT INSETED.ID VALUE(@name,@roomNb)",Connection.Instance);
            command.Parameters.Add(new SqlParameter("@name", Name));
            command.Parameters.Add(new SqlParameter("@roomNb", RoomNb));
            Connection.Instance.Open();
            Id = (int) command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            return result;
        }
    }
}
