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
        public int HotelId { get => HotelId; set => HotelId = value; }
        public int Number { get => Number; set => Number = value; }
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
                id = reader.GetInt32(0);
                number = reader.GetInt32(1);
                hotelId = reader.GetInt32(2);
                occupatedMax = reader.GetInt32(3);
                price = reader.GetDecimal(4);
                status = (RoomStatus) reader.GetInt32(5);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand("INSERT INTO Room (Number,HotelId,OcuupatedMax,price,Status) OUTOUT INSERTED.ID VALUES(@n,@h,@o,@p,@s)",Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", Number));
            command.Parameters.Add(new SqlParameter("@", hotelId));
            command.Parameters.Add(new SqlParameter("@", occupatedMax));
            command.Parameters.Add(new SqlParameter("@", price));
            Connection.Instance.Open();
            Id = (int) command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            result = (id > 0);
            return result;
        }

        public enum RoomStatus
       {
           Busy,
           Free
       }
   }
}
