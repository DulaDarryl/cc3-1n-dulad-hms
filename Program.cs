using System;
using System.Collections.Generic;

namespace CC3_1N_HMS
{
    public enum RoomStyle
    {
        TwinRoom,
        KingRoom,
        QueenRoom
    }

    public class HotelRoom
    {
        public int RoomNumber { get; }
        public RoomStyle Style { get; }
        public int Price { get; }
        public bool IsAvailable { get; private set; }

        public HotelRoom(int roomNumber, RoomStyle style, int price)
        {
            RoomNumber = roomNumber;
            Style = style;
            Price = price;
            IsAvailable = true;
        }

        public void Book()
        {
            IsAvailable = false;
        }

        public void Release()
        {
            IsAvailable = true;
        }
    }

    public class Hotel
    {
        public string Name { get; }
        public string Address { get; }
        public List<HotelRoom> Rooms { get; }

        public Hotel(string name, string address, List<HotelRoom> rooms)
        {
            Name = name;
            Address = address;
            Rooms = rooms;
        }

        public void DisplayAvailableRooms()
        {
            Console.WriteLine($"Hotel {Name} - Available Rooms");
            foreach (var room in Rooms)
            {
                if (room.IsAvailable)
                {
                    Console.WriteLine($"  Room {room.RoomNumber}, Style: {room.Style}, Price: {room.Price}");
                }
            }
        }

        public void DisplayBookedRooms()
        {
            Console.WriteLine($"Hotel {Name} - Booked Rooms");
            foreach (var room in Rooms)
            {
                if (!room.IsAvailable)
                {
                    Console.WriteLine($"  Room {room.RoomNumber}, Style: {room.Style}, Price: {room.Price}");
                }
            }
        }
    }

    public class Guest
    {
        public string Name { get; }
        public string Address { get; }
        public string Email { get; }
        public int PhoneNumber { get; }
        public List<Reservation> Reservations { get; }

        public Guest(string name, string address, string email, int phoneNumber)
        {
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Reservations = new List<Reservation>();
        }

        public void DisplayReservations()
        {
            Console.WriteLine($"List of Reservations of {Name}:");
            int index = 1;
            foreach (var reservation in Reservations)
            {
                Console.WriteLine($"  {index++}: Start Time: {reservation.StartTime.ToShortDateString()} {reservation.StartTime.ToShortTimeString()}, End Time: {reservation.EndTime.ToShortDateString()} {reservation.EndTime.ToShortTimeString()}, Duration: {reservation.Duration}, Total: {reservation.Total}");
            }
        }
    }

    public class Reservation
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public HotelRoom Room { get; }
        public int Duration { get; }
        public int Total { get; }

        public Reservation(DateTime startTime, DateTime endTime, HotelRoom room)
        {
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            Duration = (endTime - startTime).Days + 1;
            Total = Duration * room.Price;
            room.Book();
        }
    }

    public class Receptionist : Guest
    {
        public Receptionist(string name, string address, string email, int phoneNumber) : base(name, address, email, phoneNumber)
        {
        }

        public void BookReservation(Guest guest, Reservation reservation)
        {
            guest.Reservations.Add(reservation);
        }
    }

    public class HotelManagementSystem
    {
        public List<Hotel> Hotels { get; }

        public HotelManagementSystem()
        {
            Hotels = new List<Hotel>();
        }

        public void AddHotel(Hotel hotel)
        {
            Hotels.Add(hotel);
        }

        public void DisplayHotels()
        {
            Console.WriteLine("List of Hotels:");
            int index = 1;
            foreach (var hotel in Hotels)
            {
                Console.WriteLine($"  {index++}: {hotel.Name}, {hotel.Address}");
            }
        }

        public void DisplayReservationDetails(int reservationId)
        {
            var reservation = Hotels[0].Reservations[reservationId - 1];
            Console.WriteLine($"Reservation {reservationId}: Start Time: {reservation.StartTime.ToShortDateString()} {reservation.StartTime.ToShortTimeString()}, End Time: {reservation.EndTime.ToShortDateString()} {reservation.EndTime.ToShortTimeString()}, Duration: {reservation.Duration}, Total: {reservation.Total}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<HotelRoom> yananRooms = new List<HotelRoom>();
            HotelRoom room1 = new HotelRoom(101, RoomStyle.TwinRoom, 1500);
            HotelRoom room2 = new HotelRoom(102, RoomStyle.KingRoom, 3000);
            yananRooms.Add(room1);
            yananRooms.Add(room2);
            Hotel hotelYanan = new Hotel("Hotel Yanan", "123 GStreet, Takaw City", yananRooms);

            List<HotelRoom> hotel456Rooms = new List<HotelRoom>();
            HotelRoom hotel456Room1 = new HotelRoom(101, RoomStyle.QueenRoom, 2000);
            HotelRoom hotel456Room2 = new HotelRoom(102, RoomStyle.QueenRoom, 2000);
            hotel456Rooms.Add(hotel456Room1);
            hotel456Rooms.Add(hotel456Room2);
            Hotel hotel456 = new Hotel("Hotel 456", "Session Road, Baguio City", hotel456Rooms);

            HotelManagementSystem hms = new HotelManagementSystem();
            hms.AddHotel(hotelYanan);
            hms.AddHotel(hotel456);

            hms.DisplayHotels();

            hotelYanan.DisplayAvailableRooms();

            Guest terry = new Guest("Terry", "Addr 1", "terry@email.com", 63919129);
            hms.RegisterUser(terry);

            hms.BookReservation(hotelYanan, room1, terry, DateTime.Now, new DateTime(2024, 04, 16));

            hotelYanan.DisplayBookedRooms();

            terry.DisplayReservations();

            Receptionist anna = new Receptionist("Anna", "Addr 2", "anna@email.com", 67890);
            hms.RegisterUser(anna);

            Reservation res = new Reservation(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06), hotel456Room2);
            anna.BookReservation(terry, res);

            terry.DisplayReservations();

            hms.DisplayReservationDetails(1234567890);
        }
    }
}