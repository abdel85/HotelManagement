using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMangement.Classes
{
   public class Room
   { 

       private int id;
       private int hotelId;
       private int number;
       private int occupatedMax;
       private decimal price;
       private RoomStatus status;

        private static SqlCommand command;

        public int Id { get => id; set => id = value; }
        public int HotelId { get => hotelId; set => hotelId = value; }
        public int Number { get => number; set => number = value; }
        public int OccupatedMax { get => occupatedMax; set => occupatedMax = value; }
        public decimal Price { get => price; set => price = value; }
        public RoomStatus Status { get => status; set => status = value; }
 
        public Room()
        {
            
        }

        public Room( int i)
        {
            command = new SqlCommand("SELECT *FROM room where id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@i", i));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Number = reader.GetInt32(1);
                HotelId = reader.GetInt32(2);
                OccupatedMax = reader.GetInt32(3);
                Price = reader.GetDecimal(4);
                Status = (RoomStatus) reader.GetInt32(5);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand("INSERT INTO Room (Number,HotelId,OcuupatedMax,price,Status) OUTOUT INSERTED.ID VALUES(@n,@h,@o,@s,@p)",Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", Number));
            command.Parameters.Add(new SqlParameter("@h", HotelId));
            command.Parameters.Add(new SqlParameter("@", OccupatedMax));
            command.Parameters.Add(new SqlParameter("@", Status));
            command.Parameters.Add(new SqlParameter("@", Price));
            Connection.Instance.Open();
            Id = (int) command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            result = (Id > 0);
            return result;
        }

        public bool UpdateStatus(RoomStatus s)
        {
            bool result = false;
            command = new SqlCommand("UPDATE Room  SET Status = @s WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s",s));
            command.Parameters.Add(new SqlParameter("@i",Id));
            Connection.Instance.Open();
            result = command.ExecuteNonQuery() > 0 ;
            command.Dispose();
            Connection.Instance.Close();
            return result;
        }

        public override string ToString()
        {
            string result = "Room number is " + Number;
            result += "Status" + status;
            result += "Occupated Maximum " + occupatedMax;
            result += $" Price : {Price}";
            return result;

        }

        public static List<Room> GetRoomByStatus(RoomStatus s)
        {
            List<Room> result = new List<Room>();
            command = new SqlCommand("SELECT * FROM Romm WHERE Status = @s", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", s));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Room r = new Room
                {
                    Id = reader.GetInt32(0),
                    Number = reader.GetInt32(1),
                    HotelId =  reader.GetInt32(2),
                    OccupatedMax = reader.GetInt32(3),
                    price = reader.GetDecimal(4),
                    status = s

                };
                result.Add(r);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return result;
        }
        public enum RoomStatus
       {
           Busy,
           Free
       }
   }
}
