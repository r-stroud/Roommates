using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository (string connectionString) : base(connectionString) { }

        public List<Chore>GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);

                        
                    }
                    reader.Close();

                    return chores;
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM CHORE WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };

                    }
                    reader.Close();

                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                     OUTPUT INSERTED.Id
                     VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                    
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT C.Id, C.Name FROM Chore C
                    LEFT JOIN RoommateChore RC
                    ON RC.ChoreId = C.Id
                    WHERE RC.Id IS NULL";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = null;

                    while (reader.Read())
                    {
                        Chore chore = new Chore()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        chores.Add(chore);
                    }

                    reader.Close();

                    return chores;

                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId)
                    VALUES (@RoommateId, @ChoreId)";

                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", choreId);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void GetChoreCounts()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT R.FirstName, COUNT(RC.ChoreId) AS ChoreCount 
                    FROM RoommateChore RC
                    RIGHT JOIN Roommate R
                    ON R.Id = RC.RoommateId
                    GROUP BY R.FirstName
                    ORDER BY COUNT(RC.ChoreId) DESC";

                    SqlDataReader reader = cmd.ExecuteReader();

                    string output = "";

                    while (reader.Read())
                    {
                        int numberOfChores = reader.GetInt32(reader.GetOrdinal("ChoreCount"));
                        string roommate = reader.GetString(reader.GetOrdinal("FirstName"));
                        output = $"{roommate}: {Convert.ToString(numberOfChores)}";
                        Console.WriteLine(output);
                    }


                    reader.Close();

                }
            }
        }

    }
}
