using System;
using System.Collections.Generic;

class Program
{
    


    static void Main()
    {
      

        while (true)
        {
            Console.WriteLine("Choose:");
            Console.WriteLine("1. List of rooms");
            Console.WriteLine("2. Booked rooms");
            Console.WriteLine("3. Book room");
            Console.WriteLine("4. View calendar");
            Console.WriteLine("5. Administrator mode");
            Console.WriteLine("6. Exit");

            int choice = GetChoice(1, 6);

            switch (choice)
            {
                case 1:
                    ViewMeetingRooms();
                    break;
                case 2:
                    ViewBookedMeetings();
                    break;
                case 3:
                    BookMeeting();
                    break;
                case 4:
                    ViewCalendar();
                    break;
                case 5:
                    ToggleReadWriteMode();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
            }
        }
    }
    static Calendar calendar = new Calendar();
    static bool readWriteMode = false;
    static void ViewMeetingRooms()
    {
        Console.WriteLine("Rooms:");
        foreach (var room in calendar.MeetingRooms)
        {
            Console.WriteLine($"Room {room.RoomNumber}");
        }
    }

    static void ViewBookedMeetings()
    {
        Console.WriteLine("Type number of room:");
        int roomNumber = GetIntInput();

        MeetingRoom room = calendar.GetMeetingRoom(roomNumber);
        if (room != null)
        {
            Console.WriteLine($"Booked rooms {room.RoomNumber}:");
            foreach (var meeting in room.BookedMeetings)
            {
                Console.WriteLine($"Meeting {meeting}");
            }
        }
        else
        {
            Console.WriteLine("Room didn`t find.");
        }
    }

    static void BookMeeting()
    {
        if (readWriteMode)
        {
            Console.WriteLine("Type number of room:");
            int roomNumber = GetIntInput();

            MeetingRoom room = calendar.GetMeetingRoom(roomNumber);
            if (room != null)
            {
                Console.WriteLine("Name of meeting:");
                string meetingName = Console.ReadLine();

                Console.WriteLine("Date and type of meeting:");
                DateTime meetingDateTime = GetDateTimeInput();

                if (calendar.BookMeeting(roomNumber, meetingName, meetingDateTime))
                {
                    Console.WriteLine($"Meeting '{meetingName}'. Number of room: {roomNumber}. See you on {meetingDateTime}!");
                }
                else
                {
                    Console.WriteLine("Room is busy on this time.");
                }
            }
            else
            {
                Console.WriteLine("We can`t find this room.");
            }
        }
        else
        {
            Console.WriteLine("Turn on administrator mode, before you do this.");
        }
    }

    static void ViewCalendar()
    {
        Console.WriteLine("Room number:");
        int roomNumber = GetIntInput();

        MeetingRoom room = calendar.GetMeetingRoom(roomNumber);
        if (room != null)
        {
            Console.WriteLine($"In {room.RoomNumber}:");

            foreach (var meeting in room.BookedMeetings)
            {
                Console.WriteLine($"Meeting '{meeting.MeetingName}' on {meeting.MeetingDateTime}");
            }
        }
        else
        {
            Console.WriteLine("We can`t find this room.");
        }
    }

    static void ToggleReadWriteMode()
    {
        readWriteMode = !readWriteMode;
        Console.WriteLine($"Administrator mode {(readWriteMode ? "included" : "switched off")}");
    }

    static int GetChoice(int min, int max)
    {
        int choice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
            {
                return choice;
            }
            else
            {
                Console.WriteLine($"Please enter a number from {min} to {max}.");
            }
        }
    }

    static int GetIntInput()
    {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Type correct number.");
        }
        return number;
    }

    static DateTime GetDateTimeInput()
    {
        DateTime dateTime;
        while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
        {
            Console.WriteLine("Type correct date or time.");
        }
        return dateTime;
    }
}

class Calendar
{
    public List<MeetingRoom> MeetingRooms { get; }

    public Calendar()
    {
        MeetingRooms = new List<MeetingRoom>
        {
            new MeetingRoom(1),
            new MeetingRoom(2),
            new MeetingRoom(3),
        };
    }

    public MeetingRoom GetMeetingRoom(int roomNumber)
    {
        return MeetingRooms.Find(room => room.RoomNumber == roomNumber);
    }

    public bool BookMeeting(int roomNumber, string meetingName, DateTime meetingDateTime)
    {
        MeetingRoom room = GetMeetingRoom(roomNumber);

        if (room != null)
        {
            if (room.IsAvailable(meetingDateTime))
            {
                room.BookMeeting(new Meeting(meetingName, meetingDateTime));
                return true;
            }
        }

        return false;
    }
}
class Meeting
    {
        public string MeetingName { get; }
        public DateTime MeetingDateTime { get; }

        public Meeting(string meetingName, DateTime meetingDateTime)
        {
            MeetingName = meetingName;
            MeetingDateTime = meetingDateTime;
        }
    }
class MeetingRoom
{
    public int RoomNumber { get; }
    public List<Meeting> BookedMeetings { get; }

    public MeetingRoom(int roomNumber)
    {
        RoomNumber = roomNumber;
        BookedMeetings = new List<Meeting>();
    }

    public void BookMeeting(Meeting meeting)
    {
        BookedMeetings.Add(meeting);
    }

    public bool IsAvailable(DateTime meetingDateTime)
    {
        return !BookedMeetings.Any(meeting => meeting.MeetingDateTime == meetingDateTime);
    }
}

