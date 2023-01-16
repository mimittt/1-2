using System;
using PW_1_2.MyEntity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace PW_1_2.Database
{
    public class CompanyDAO : EntityDAO
    {
        readonly int? Director_Id = null;

        public CompanyDAO()
        {
            if (!IsEmptyList(list.companies))
                return;
            
            list.companies = new Dictionary<int, Company>();

            // Запрос на создание таблицы "Company", если её нету
            string sql = "CREATE TABLE IF NOT EXISTS Company"
                        + "(Company_id INT NOT NULL AUTO_INCREMENT, "
                        + "Director_id INT NOT NULL, "
                        + "Name VARCHAR(45) NOT NULL, "
                        + "Address VARCHAR(50) NOT NULL, "
                        + "Date_Creation DATETIME NOT NULL, "
                        + "PRIMARY KEY(Company_id, Director_id),"
                        + "CONSTRAINT fk_Company_Director "
                        + "FOREIGN KEY (Director_id) REFERENCES Director (Director_id) "
                        + "ON DELETE CASCADE "
                        + "ON UPDATE CASCADE) "
                        + "COLLATE='utf8_general_ci' ENGINE=InnoDB;";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }

            //Запрос на получения данных таблицы "Company"
            sql = "SELECT * FROM Company";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) // Если есть данные
                {

                    while (reader.Read()) // Построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        int director_id = reader.GetInt32(1);
                        string name = reader.GetString(2);
                        string address = reader.GetString(3);
                        string date = reader.GetString(4);

                        list.companies.Add(id, new Company(id, director_id, name, address,
                            DateTime.Parse(date), list.directors[director_id]));
                    }
                }
            }
            
        }
        public CompanyDAO(int director_id)
        {
            Director_Id = director_id;
        }

        public override void Add()
        {
            if (IsEmptyList(list.directors))
            {
                Console.WriteLine("База данных директоров пуста! Добавьте!");
                return;
            }

            string sql = "INSERT INTO Company (Company_id, Director_id, Name, Address, Date_Creation) "
                + "VALUES (@company_id, @director_id, @name, @address, @date_creation)";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[+ Добавить Компанию]");
                    Company company = new Company().SetId().SetDirectorId(Director_Id);
                    company.SetFullData(list.directors[company.Director_id]).SetDate();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@company_id", company.Id);
                    cmd.Parameters.AddWithValue("@director_id", company.Director_id);
                    cmd.Parameters.AddWithValue("@name", company.Name);
                    cmd.Parameters.AddWithValue("@address", company.Address);
                    cmd.Parameters.AddWithValue("@date_creation", company.Date_Creation.ToString("yyyy-MM-dd H:mm:ss"));

                    cmd.ExecuteNonQuery();
                    Update("Add", company, company.Id);

                    return;
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
            if (IsEmptyList(list.companies))
            {
                Console.WriteLine("База данных компаний пуста! Добавьте!");
                return;
            }

            string sql = "UPDATE Company SET "
                         + "Name = @name, "
                         + "Address=@address "
                         + "WHERE Company_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[= Изменить данные Компании]");
                    GetList();

                    Console.Write("\nВведите id компании: ");
                    int Id = int.Parse(Console.ReadLine());
                    Console.Clear();

                    Console.Write(list.companies[Id] + "\n");

                    Company company = list.companies[Id].SetId().SetDirectorId(null);
                    company.SetFullData(list.directors[company.Director_id]);

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@name", company.Name);
                    cmd.Parameters.AddWithValue("@address", company.Address);
                    cmd.Parameters.AddWithValue("@id", Id);

                    cmd.ExecuteNonQuery();
                    Update("Change", company, Id);

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
            if (IsEmptyList(list.companies))
            {
                Console.WriteLine("База данных компаний пуста! Добавьте!");
                return;
            }

            string sql = $"SELECT * FROM company WHERE concat(Director_id, Name, Address, Date_Creation) LIKE @text";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                Console.WriteLine("[? Поиск по параметрам Компании]");
                Console.Write("Поиск: ");
                string text = Console.ReadLine();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@text", $"%{text}%");

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) // Если есть данные
                {

                    while (reader.Read()) // Построчно считываем данные
                    {
                        int id = reader.GetInt32(0);
                        int director_id = reader.GetInt32(1);
                        string name = reader.GetString(2);
                        string address = reader.GetString(3);
                        string date = reader.GetString(4);

                        Console.WriteLine(new Company(id, director_id, name, address,
                            DateTime.Parse(date), list.directors[director_id]));
                    }

                    return;
                }
                else
                    Console.WriteLine($"Нет результатов поиска!");
            }
        }
        public override void Remove()
        {
            if (IsEmptyList(list.companies))
            {
                Console.WriteLine("База данных компаний пуста! Добавьте!");
                return;
            }

            string sql = $"DELETE FROM Company WHERE Company_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[- Удалить Компанию]");
                    GetList();
                    Console.Write("\nВведите Id: ");
                    int Id = int.Parse(Console.ReadLine());

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();

                    Update("Remove", list.companies[Id], Id);
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
            if (IsEmptyList(list.companies))
            {
                Console.WriteLine("База данных компаний пуста! Добавьте!");
                return;
            }

            int count = 0;
            Console.WriteLine("[Список Компаний]\n");
            foreach (var company in list.companies)
            {
                count++;
                Console.WriteLine($"[{count}]");
                Console.WriteLine(company.Value);
            }
        }
        protected override void Update<T>(string operation, T entity, int Id)
        {
            Company company = entity as Company;

            if (operation == "Add")
            {
                list.companies.Add(Id, company);

                if (Director_Id == null)
                    Console.WriteLine($"Компания \"{company.Name}\" успешно добавлена в БД!\n");
                else
                    Console.WriteLine($"Для директора({company.Director_id}) успешно добавлена Компания \"{company.Name}\" в БД!\n");
            }

            else if (operation == "Change")
            {
                list.companies[Id] = company;
                Console.WriteLine($"Компания({company.Id}) успешно изменена в БД!\n");
            }

            else if (operation == "Remove")
            {
                list.companies.Remove(Id);
                Console.WriteLine($"Компания \"{company.Name}\" успешно удалена из БД!\n");
            }
        }
    }
}
