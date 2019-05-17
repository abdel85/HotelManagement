using System;
using System.Data.SqlClient;
using static HotelMangement.Classes.Invoice;

namespace HotelMangement.Classes
{
    public class Booking
    {
        private int id;
        private string code;
        private int customerId;
        private int roomId;
        private int occupatedNumber;
        private BookingStatus status;
        private InvoiceStatus statusInvoice;

        private static SqlCommand command;

        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int RoomId { get => roomId; set => roomId = value; }
        public int OccupatedNumber { get => occupatedNumber; set => occupatedNumber = value; }
        public BookingStatus Status { get => status; set => status = value; }
        public InvoiceStatus statusInvoice { get => statusInvoice; set => statusInvoice = value; }

        public Booking()
        {
            Code = Guid.NewGuid().ToString();
        }

        public Booking(string code)
        {
            command = new SqlCommand("SELECT *FROM Booking WHERE Code = @c", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", c));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Code = reader.GetString(1);
                CustomerId = reader.GetInt32(2);
                RoomId = reader.GetInt32(3);
                OccupatedNumber = reader.GetInt32(4);
                Status = (BookingStatus)reader.GetInt32(5);
                StatusInvoice = (InvoiceStatus)reader.GetInt32(6);
 
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand("INSERT INTO Booking(Code,CustomerId,RoomId,OccupatedNumber,Status) OUTPUT INSERTED.ID values(@c,@cu,@r,@o,@s)",Connection.Instance);
            command.Parameters.Add(new SqlParameter("@c", Code));
            command.Parameters.Add(new SqlParameter("@cu", CustomerId));
            command.Parameters.Add(new SqlParameter("@r", RoomId));
            command.Parameters.Add(new SqlParameter("@o", OccupatedNumber));
            command.Parameters.Add(new SqlParameter("@s", Status));

            Connection.Instance.Open();
            Id = (int) command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            result = (Id > 0);
            return result;

        }
        public enum BookingStatus
        {
            Cancelled,
            Validated
        }


        
    }

     
}
