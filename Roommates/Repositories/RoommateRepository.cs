using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //roommate first name, rent portion, and name of the room they occupy
                    cmd.CommandText = @"
                SELECT RM.FirstName, RM.RentPortion, R.Name AS RoomName
                FROM Roommate RM
                LEFT JOIN Room R
                ON RM.RoomId = R.Id
                WHERE RM.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room room = null;

                    if(reader.Read())
                    {
                        room = new Room 
                        {
                            Name = reader.GetString(reader.GetOrdinal("RoomName"))
                        };

                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),  
                            Room = room
                        };

                       
                    }

                    reader.Close();

                    return roommate;

                }
            }
        }

        public List<Roommate> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"SELECT RM.Id, RM.FirstName, RM.LastName, RM.RentPortion, RM.MoveInDate, RM.RoomId,
                    R.Name
                    FROM Roommate RM
                    LEFT JOIN ROOM R
                    ON RM.RoomId = R.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates= new List<Roommate>();

                    while(reader.Read())
                    {
                        Room room = new Room
                        {
                           Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        Roommate roommate = new Roommate 
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = room
                        };

                        roommates.Add(roommate);

                    }

                    reader.Close();
                    return roommates;
                }

            }
        }
    }
}
