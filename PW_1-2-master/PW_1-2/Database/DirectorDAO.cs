using System;
using PW_1_2.MyEntity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace PW_1_2.Database
{
    public class DirectorDAO : EntityDAO
    {
        public DirectorDAO()
        {
            if (!IsEmptyList(list.directors))
                return;
            
            list.directors = new Dictionary<int, Director>();

            // Запрос на создание таблицы "Director", если её нету
            string sql = "CREATE TABLE IF NOT EXISTS Director"
                            + "(Director_id INT NOT NULL AUTO_INCREMENT, "
                            + "Name VARCHAR(20) NOT NULL, "
                            + "Surname VARCHAR(20) NOT NULL, "
                            + "Middle_name VARCHAR(20) NOT NULL, "
                            + "Age INT NOT NULL, "
                            + "Phone_number VARCHAR(20) NOT NULL, "
                            + "PRIMARY KEY(Director_id)) "
                            + "COLLATE='utf8_general_ci' ENGINE=InnoDB;";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

            }

            // Запрос на получения данных таблицы "Director"
            sql = "SELECT * FROM Director";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) // Если есть данные
                    while (reader.Read()) // Построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string middle_name = reader.GetString(3);
                        int age = reader.GetInt32(4);
                        string phone_number = reader.GetString(5);

                        list.directors.Add(id, new Director(id, name, surname, middle_name, age, phone_number));
                    }
            }
            
        }

        public override void Add()
        {
            string sql = "INSERT INTO Director (Director_id, Name, Surname, Middle_name, Age, Phone_number) " +
                "VALUES (@director_id, @name, @surname, @middle_name, @age, @phone_number)";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[+ Добавить Директора]");

                    Director director = new Director().SetId().SetFullData();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@director_id", director.Id);
                    cmd.Parameters.AddWithValue("@name", director.Name);
                    cmd.Parameters.AddWithValue("@surname", director.Surname);
                    cmd.Parameters.AddWithValue("@middle_name", director.Middle_name);
                    cmd.Parameters.AddWithValue("@age", director.Age);
                    cmd.Parameters.AddWithValue("@phone_number", director.Phone_number);

                    cmd.ExecuteNonQuery();
                    list.directors.Add(director.Id, director);
                    connection.Close();

                    new CompanyDAO(director.Id).Add();

                    Update("Add", director, director.Id);
                }
                catch (Exception ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    LogException(edi);
                }
            }
        }
        public override void Change()
        {
            if (IsEmptyList(list.directors))
            {
                Console.WriteLine("База данных директоров пуста! Добавьте!");
                return;
            }

            string sql = "UPDATE Director SET Name = @name, "
                         + "Surname = @surname, "
                         + "Middle_name = @middle_name, "
                         + "Age = @age, "
                         + "Phone_number = @phone_number "
                         + "WHERE Director_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[= Изменить данные Директора]");
                    GetList();

                    Console.Write("\nВведите id директора: ");
                    int Id = int.Parse(Console.ReadLine());
                    Console.Clear();

                    Console.Write(list.directors[Id] + "\n");

                    Director director = list.directors[Id].SetId().SetFullData();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@name", director.Name);
                    cmd.Parameters.AddWithValue("@surname", director.Surname);
                    cmd.Parameters.AddWithValue("@middle_name", director.Middle_name);
                    cmd.Parameters.AddWithValue("@age", director.Age);
                    cmd.Parameters.AddWithValue("@phone_number", director.Phone_number);
                    cmd.Parameters.AddWithValue("@id", Id);

                    cmd.ExecuteNonQuery();
                    Update("Change", director, Id);
                }
                catch (Exception ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    LogException(edi);
                }

            }
        }
        public override void Find()
        {
            if (IsEmptyList(list.directors))
            {
                Console.WriteLine("База данных директоров пуста! Добавьте!");
                return;
            }

            string sql = "SELECT * FROM director WHERE concat(Name, Surname, Middle_name, Age) like @text";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                Console.WriteLine("[? Поиск по параметрам Директоров]");
                Console.Write("Поиск: ");
                string text = Console.ReadLine();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@text", $"%{text}%");

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) // Если есть данные
                {
                    while (reader.Read()) // Построчно считываем данные
                    {
                        int director_id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string middle_name = reader.GetString(3);
                        int age = reader.GetInt32(4);
                        string phone_number = reader.GetString(5);

                        Console.WriteLine();
                        Console.WriteLine(new Director(director_id, name, surname, middle_name, age, phone_number));
                    }
                }
                else
                    Console.WriteLine($"Нет результатов поиска!");
            }
        }
        public override void Remove()
        {
            if (IsEmptyList(list.directors))
            {
                Console.WriteLine("База данных директоров пуста! Добавьте!");
                return;
            }

            string sql = $"DELETE FROM Director WHERE Director_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[- Удалить Директора]");
                    GetList();
                    Console.Write("\nВведите Id: ");
                    int Id = int.Parse(Console.ReadLine());

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();

                    Update("Remove", list.directors[Id], Id);
                }
                catch (Exception ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    LogException(edi);
                }
            }
        }
        public override void GetList()
        {
            if (IsEmptyList(list.directors))
            {
                Console.WriteLine("База данных директоров пуста! Добавьте!");
                return;
            }

            int count = 0;
            Console.WriteLine("<Список Директоров>]\n");
            foreach (var director in list.directors)
            {
                count++;
                Console.WriteLine($"[{count}]");
                Console.WriteLine(director.Value);
            }

        }
        protected override void Update<T>(string operation, T entity, int Id)
        {
            Director director = entity as Director;

            if (operation == "Add")
            {
                Console.WriteLine($"Директор({Id}) и его компания успешно добавлен в БД!\n");
            }

            else if (operation == "Change")
            {
                list.directors[Id] = director;
                Console.WriteLine("Директор успешно изменён в БД!\n");
            }

            else if (operation == "Remove")
            {
                list.directors.Remove(Id);
                Console.WriteLine("Директор успешно удалён из БД!\n");
            }
        }
    }
}
