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
            string input = Console.ReadLine();
            int roomNb;
            do
            {
                Console.WriteLine("Rooms Number :");
                Int32.TryParse(Console.ReadLine(), out roomNb);
            } while (roomNb == 0);


        }

        static void ChooseHotel()
        {

        }
    }
}
