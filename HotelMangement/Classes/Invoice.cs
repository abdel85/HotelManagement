using System;
using System.Collections.Generic;
using System.Data.SqlClient;
 using System.Text;
 
namespace HotelMangement.Classes
{
   public class Invoice
   {
       private int customerId;
       private int id;
       private decimal price;
       private InvoiceStatus status;
       private static SqlCommand command;

        public int CustomerId { get => customerId; set => customerId = value; }
        public int Id { get => id; set => id = value; }
        public decimal Price { get => price; set => price = value; }
        public InvoiceStatus Status { get => status; set => status = value; }
        public static SqlCommand Command { get => command; set => command = value; }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand("INSERT INTO invoice (customerId, price, satuts) OUTPUT INSERTED.ID VALUES (@c,@pc@s)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@", customerId));
            command.Parameters.Add(new SqlParameter("@", price));
            command.Parameters.Add(new SqlParameter("@", Status));

            Connection.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            result = (id > 0);
            return result;
        }

    }

   public enum InvoiceStatus
   {
       paid,
       notPaid
   }
}
