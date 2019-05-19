using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HotelMangement.Classes;

namespace HotelMangement
{
    class Program
    {
        static Hotel hotel;
        static void Main(string[] args)
        {
            Init();
            Console.ReadLine();
        }

        static void Menu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("Choose a Number to select an option");
                Console.WriteLine("1-Costumer's List");
                Console.WriteLine("2-Booking List");
                Console.WriteLine("3-Add Booking");
                Console.WriteLine("0-to Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            GetCostumer();
                            break;
                        case 2:
                            GetBooking();
                            break;
                        case 3:
                            AddBooking();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
            while (choice != 0);
        }

        static void Init()
        {
            int choice = 3;
            do
            {
                Console.WriteLine("1-New Hotel");
                Console.WriteLine("2-Choose Hotel");
                Console.WriteLine("0-Exit");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            NewHotel();
                            break;
                        case 2:
                            ChooseHotel();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;

                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                  
                }

            }
            while (choice != 3);
        }

        static void NewHotel()
        {
            Console.WriteLine("Hotel Name:");
            string name = Console.ReadLine();
            int roomNb;
            do
            {
                Console.WriteLine("Rooms Number :");
                Int32.TryParse(Console.ReadLine(), out roomNb);
            } while (roomNb == 0);
            Hotel hotel = new Hotel() {Name = name, RoomNb = roomNb};
            if (hotel.Save())
            {
                Random r = new Random();
                 for (int i = 1 ; i <= hotel.RoomNb; i++)
                {
                    Room room = new Room()
                    {
                        Number = i,
                        HotelId = hotel.Id,
                        OccupatedMax = r.Next(2, 4),
                        Status = Room.RoomStatus.Free,
                    };
                    room.Price = room.OccupatedMax == 3 ? 50 : 40;
                    room.Save();
                }
            }

            Console.WriteLine("Hotel Successfully Added");

        }

        static void ChooseHotel()
        {
            Console.WriteLine("Hotel Name :");
            string name = Console.ReadLine();
            Hotel h = new Hotel();
            if (hotel.Id > 0)
            {
                hotel = h;
                Console.Clear();
                Menu();

            }
            else
            {
                Console.WriteLine("Hotel not found");
            }
        }

        static void GetCustomers()
        {
            Console.Clear();
            List<Customer> list = Customer.GetCustomers(hotel.Id);
            if (list.Count>0)
            {
                Console.WriteLine("No custome found");
            }

            CustomerMenu();
        }

        public void CustomerMenu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("1-New  customer");
                Console.WriteLine("2-Customer Information");
                Console.WriteLine("3-Add booking");
                Console.WriteLine("0-Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            NewCustomer();
                            break;
                        case 2:
                            InformationCustomer();
                            break;
                        case 3:
                            AddBooking();
                            break;
                    }
                }
                catch (Exception c)
                {
                    Console.WriteLine(c.Message);
                  
                }
            }
            while (choice != 0);
        }

        static void NewCustomer()
        {
            Customer customer = new Customer();
            Console.WriteLine("First Name: ");
            customer.Firstname = Console.ReadLine();
            Console.WriteLine("Last Name : ");
            customer.Lastname = Console.ReadLine();
            Console.WriteLine("Phone :");
            customer.Address = Console.ReadLine();
            Console.WriteLine("Adress :");
            customer.Address = Console.ReadLine();
            customer.HotelId = hotel.Id;

            if (customer.Save())
            {
                Console.WriteLine("Customer has been added with the Id : "+customer.Id);
            }
            else
            {
                Console.WriteLine("Database error");
            }
        }


        #region InformationCustomer method

        static void InformationCustomer()
        {
            Console.WriteLine("Customer phone :");
            string phone = Console.ReadLine();

            Customer c = new Customer(phone);
            if (c.Id > 0)
            {
                Console.WriteLine(c);
                Console.WriteLine("***--BOOKING--***");
                foreach (Booking b in Booking.GetBookings(c.Id))
                {
                    Console.WriteLine(b);
                }

                List<Booking> bookingUnPaid = Booking.GetBookings(c.Id)
                .FindAll(x => x.StatusInvoice == Invoice.InvoiceStatus.notPaid);
                decimal total = 0;
                foreach (Booking b in bookingUnPaid)
                {
                    Room r =new Room(b.RoomId);
                    total += r.Price;

                }

                Console.WriteLine($" You have to pay : {total}");
                Console.WriteLine("1-Pay your bill");
                Console.WriteLine("2-Pay later");
                int number;
                Int32.TryParse(Console.ReadLine(), out number);
                if (number == 1)
                {
                    Paid(c.Id, total, bookingUnPaid);
                }
                else
                {
                    Console.WriteLine("Customer not found");
                }
            }
            

            #endregion
        }

        static public void Paid(int idCustomer, decimal total, List<Booking> bs)
        {
            Invoice i = new Invoice()
            {    Price =  total,
                CustomerId = idCustomer,
                Status = Invoice.InvoiceStatus.notPaid

            };

            if (i.Save())
            {
                foreach (Booking b in bs)
                {
                    b.UpdateStatus(Invoice.InvoiceStatus.paid);
                }
            }

        }
        static public  void AddBooking()
        {

        }
    }
}
