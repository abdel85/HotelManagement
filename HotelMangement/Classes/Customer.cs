using System;
using System.Collections.Generic;
using System.Data.SqlClient;
 using System.Text;


namespace HotelMangement.Classes
{
    public class Customer
    {
        private int id;
        private string firstname;
        private string lastname;
        private string phone;
        private string address;
        private int hotelId;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Firstname
        {
            get => firstname;
            set => firstname = value;
        }

        public string Lastname
        {
            get => lastname;
            set => lastname = value;
        }

        public string Phone
        {
            get => phone;
            set => phone = value;
        }

        public string Address
        {
            get => address;
            set => address = value;
        }

        public int HotelId
        {
            get => hotelId;
            set => hotelId = value;
        }

        private static SqlCommand command;

        public Customer()
        {

        }

        public Customer(string phone)
        {
            command = new SqlCommand("SELECT  * FROM Customer WHERE Phone = @p", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@p", phone));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Firstname = reader.GetString(1);
                Lastname = reader.GetString(2);
                Phone = reader.GetString(3);
                Address = reader.GetString(4);
                HotelId = reader.GetInt32(5);

            }

            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public Customer(int i)
        {
            command = new SqlCommand("SELECT  * FROM Customer WHERE Id = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@i", i));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.GetInt32(0);
                Firstname = reader.GetString(1);
                Lastname = reader.GetString(2);
                Phone = reader.GetString(3);
                Address = reader.GetString(4);
                HotelId = reader.GetInt32(5);

            }

            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
        }

        public bool Save()
        {
            bool result = false;
            command = new SqlCommand(
                "INSERT INTO Customer (firstname,lastname,phone,address,hotelId) OUTOUT INSERTED.ID VALUES(@f,@l,@p,@a,@h)",
                Connection.Instance);
            command.Parameters.Add(new SqlParameter("@n", Firstname));
            command.Parameters.Add(new SqlParameter("@h", Lastname));
            command.Parameters.Add(new SqlParameter("@", Phone));
            command.Parameters.Add(new SqlParameter("@", Address));
            command.Parameters.Add(new SqlParameter("@", HotelId));
            Connection.Instance.Open();
            Id = (int) command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
            result = (Id > 0);
            return result;
        }

        public static List<Customer> GetCustomers(int hotelId)
        {
            List<Customer> customers = new List<Customer>();
            command = new SqlCommand("SELECT * FROM Costumer WHERE hotelId = @i", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@s", hotelId));
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Customer c = new Customer
                {
                    Id = reader.GetInt32(0),
                    Firstname = reader.GetString(1),
                    Lastname = reader.GetString(2),
                    Phone = reader.GetString(3),
                    Address = reader.GetString(4),
                    HotelId = reader.GetInt32(5)

                };
                customers.Add(c);
            }

            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return customers;
        }

        public override string ToString()
        {
            string result = "Name;  " + Firstname + " " + Lastname;
            result += " Phone :" + Phone;
            result += " Adresse : " + Address;
            result += " \nCustomer Id : " + Id;

            return result;

        }

    }
}

 