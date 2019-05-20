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
                            GetCustomers();
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
            if (list.Count > 0)
            {
                foreach (Customer c in   list)
                {
                    Console.WriteLine(c);
                    Console.WriteLine("---------------");
                }
            }
            else
            {
               
                Console.WriteLine("No custome found");
              
            }
           

            CustomerMenu();
        }

        static void CustomerMenu()
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
                .FindAll(x => x.StatusInvoice == InvoiceStatus.notPaid);
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

        static void Paid(int idCustomer, decimal total, List<Booking> bs)
        {
            Invoice i = new Invoice()
            {    Price =  total,
                CustomerId = idCustomer,
                Status = InvoiceStatus.paid

            };

            if (i.Save())
            {
                foreach (Booking b in bs)
                {
                    b.UpdateStatus(InvoiceStatus.paid);
                }
            }
        }


        static void AddBooking()
        {
            Console.WriteLine("Customer Phone :");
            string phone = Console.ReadLine();
            Customer c = new Customer();
            if (c.Id > 0)
            {
                if (Room.GetRoomByStatus(Room.RoomStatus.Free).Count > 0)
                {
                    foreach (Room r in Room.GetRoomByStatus(Room.RoomStatus.Free))
                    {
                        Console.WriteLine(r);
                        Console.WriteLine("----------------");
                    }

                    int number;
                    do
                    {
                        Console.WriteLine("How many people? : ");
                        Int32.TryParse(Console.ReadLine(), out number);
                    } while (number == 0);

                    do
                    {
                        Console.WriteLine("Room's number");
                        int n = Convert.ToInt32(Console.ReadLine());
                        Room room = Room.GetRoomByStatus(Room.RoomStatus.Free).Find(x => x.Number == n);
                        Booking b = new Booking()
                        {
                            CustomerId = c.Id,
                            RoomId = room.Id,
                            OccupatedNumber = (number > room.OccupatedMax) ? room.OccupatedMax : number,
                            Status = BookingStatus.Validated
                        };

                        if (b.Save())
                        {
                            room.UpdateStatus(Room.RoomStatus.Busy);
                            number -= room.OccupatedMax;
                        }

                    }
                    while (number > 0);
                }
                else
                {
                    Console.WriteLine(" No free room available");
                }
            }
                else
                {
                    Console.WriteLine("Customer not found");
                }


            }

        static void GetBooking()
        {
            Console.Clear();
            List<Booking> list = Booking.GetBookingsByHotelId(hotel.Id);
            if (list.Count > 0)
            {
                foreach (Booking b in list)
                {
                    Console.WriteLine(b);
                    Room r = new Room(b.RoomId);
                    Console.WriteLine(r);
                    Customer c = new Customer(b.CustomerId);
                    Console.WriteLine(c);
                    Console.WriteLine("----------------------");
                }
            }
            else
            {
                Console.WriteLine("No booking foud for this hotel");
            }

            BookingMenu();
        }

        static void BookingMenu()
        {
            int choice = 4;
            do
            {
                Console.WriteLine("1-New  Booking");
                Console.WriteLine("2-Change status");
                 Console.WriteLine("0-Exit");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            AddBooking();
                            break;
                        case 2:
                            ChangeBookingStatus();
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

          static void ChangeBookingStatus()
        {
            Console.WriteLine("Booking Code: ");
            string code = Console.ReadLine();
            Booking b = new Booking(code);
            if (b.Id > 0)
            {
                if (b.Status == BookingStatus.Cancelled)
                {
                    b.UpdateStatus(BookingStatus.Cancelled);
                    Room r = new Room(b.RoomId);
                    r.UpdateStatus(Room.RoomStatus.Free);

                }

                Console.WriteLine("Status Updated");
            }
            else
            {
                Console.WriteLine("No Booking found");
            }
        }
    }
}

