using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;Trust Server Certificate=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        Console.Clear();
                        Console.WriteLine("Showing all rooms");
                        Console.WriteLine();
                        List<Room> rooms = roomRepo.GetAll();  
                        foreach (Room r in rooms) 
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.WriteLine();    
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Clear();
                        Console.WriteLine("Search for a room");
                        Console.WriteLine();
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());
                        Room room = roomRepo.GetById(id);
                        Console.Clear();
                        Console.WriteLine("Room found");
                        Console.WriteLine();
                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy ({room.MaxOccupancy})");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Clear();
                        Console.WriteLine("Add a room");
                        Console.WriteLine();
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();
                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max,
                        };

                        roomRepo.Insert(roomToAdd);
                        Console.Clear();
                        Console.WriteLine("Room has succesfully been added");
                        Console.WriteLine();
                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}.");
                        Console.WriteLine(); 
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("List all chores"):
                        Console.Clear();
                        Console.WriteLine("Listing all chores");
                        Console.WriteLine();
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");         
                        }
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a chore"):
                        Console.Clear();
                        Console.WriteLine("Search for a chore");
                        Console.WriteLine();
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse (Console.ReadLine());
                        Chore chore = choreRepo.GetById(choreId);
                        Console.Clear();
                        Console.WriteLine("Chore found");
                        Console.WriteLine();
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Clear();
                        Console.WriteLine("Add a chore");
                        Console.WriteLine();
                        Console.Write("Chore Name: ");
                        string choreName = Console.ReadLine();
                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(choreToAdd);
                        Console.Clear();
                        Console.WriteLine("Chore has succesfully been added");
                        Console.WriteLine();
                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}.");
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a roommate"):
                        Console.Clear();
                        Console.WriteLine("Search for a roommate");
                        Console.WriteLine();
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.Clear();
                        Console.WriteLine("Roommate found");
                        Console.WriteLine();
                        Console.WriteLine($"{roommate.FirstName} - {roommate.Room.Name}: Rent Portion = {roommate.RentPortion} ");
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("List unassigned chores"):
                        Console.Clear();
                        Console.WriteLine("Listing all unassigned chores");
                        Console.WriteLine();
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name} - unassigned");
                        }

                        //List<Roommate> roommates = roommateRepo.GetAll();
                        //foreach (Roommate r in roommates)
                        //{
                        //    Console.WriteLine($"{r.FirstName}{r.LastName} is renting {r.Room.Name} for {r.RentPortion}.");
                        //}


                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        Console.Clear();
                        Console.WriteLine("Assigning a chore. Please select a Roommate and chore.");
                        Console.WriteLine();
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName}{r.LastName} - {r.Room.Name}");
                        }
                        Console.Write("Please selece a roomate number: ");
                        int rmateId = int.Parse(Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Assigning a chore. Roommate selected. Please select a chore.");
                        Console.WriteLine();
                        List<Chore> toBeAssigned = choreRepo.GetUnassignedChores();
                        foreach (Chore c in toBeAssigned)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name} - unassigned");
                        }
                        Console.WriteLine() ;
                        Console.Write("Please select a chore number: ");
                        int toBeAssignedId = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(rmateId, toBeAssignedId);
                        Console.WriteLine("Chore has successfully been assigned!");
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();

                        break;
                    case ("Get chore counts"):
                        Console.Clear();
                        Console.WriteLine("Getting chore counts");
                        Console.WriteLine();
                        choreRepo.GetChoreCounts();
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }





        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "List all chores",
                "Search for a chore",
                "Add a chore",
                "Search for a roommate",
                "List unassigned chores",
                "Assign chore to roommate",
                "Get chore counts",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}