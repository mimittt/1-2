using System;
using PW_1_2.MyEntity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace PW_1_2.Database
{
    public class PhoneDAO : EntityDAO
    {
        public PhoneDAO()
        {
            if (!IsEmptyList(list.phones))
                return;
           
            list.phones = new Dictionary<int, Phone>();

            // Запрос на создание таблицы "Phone", если её нету
            string sql = "CREATE TABLE IF NOT EXISTS Phone("
            + "Phone_id INT NOT NULL AUTO_INCREMENT, "
            + "Company_id INT NOT NULL, "
            + "Manufacturer VARCHAR(45) NOT NULL, "
            + "Series VARCHAR(45) NULL, "
            + "Screen_Diagonal DOUBLE NOT NULL, "
            + "Memory DOUBLE NOT NULL, "
            + "Sim_Num INT NOT NULL, "
            + "Color VARCHAR(45) NOT NULL, "
            + "Camera VARCHAR(45) NULL, "
            + "Front_Camera VARCHAR(45) NULL, "
            + "Price DOUBLE NOT NULL, "
            + "Date_Creation DATETIME NOT NULL, "
            + "PRIMARY KEY(Phone_id), "
            + "CONSTRAINT fk_Phone_Company "
            + "FOREIGN KEY (Company_id) REFERENCES Company (Company_id) "
            + "ON DELETE CASCADE "
            + "ON UPDATE CASCADE) "
            + "COLLATE='utf8_general_ci' ENGINE=InnoDB;";


            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }


            //Запрос на получения данных таблицы "Phone"
            sql = "SELECT * FROM Phone";

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
                        int company_id = reader.GetInt32(1);
                        string manufacturer = reader.GetString(2);
                        string series = reader.IsDBNull(3) ? null : reader.GetString(3);
                        double screen_diagonal = reader.GetDouble(4);
                        double memory = reader.GetDouble(5);
                        int sim_num = reader.GetInt32(6);
                        string color = reader.GetString(7);
                        string camera = reader.IsDBNull(8) ? null : reader.GetString(8);
                        string front_camera = reader.IsDBNull(9) ? null : reader.GetString(9);
                        double price = reader.GetDouble(10);
                        string date_creation = reader.GetString(11);

                        Phone phone = new Phone().Builder(id, company_id, manufacturer, screen_diagonal, memory, sim_num,
                            color, price, DateTime.Parse(date_creation), list.companies[company_id], series, camera, front_camera);

                        list.phones.Add(id, phone);

                    }
                }
            }
            
        }

        public override void Add()
        {
            if (IsEmptyList(list.companies))
            {
                Console.WriteLine("База данных компаний пуста! Добавьте!");
                return;
            }

            string sql = "INSERT INTO Phone (Phone_id, Company_id, Manufacturer, Series, Screen_Diagonal, Memory, SIM_Num, Color, Camera, Front_Camera, Price, Date_Creation) "
                + "VALUES (@phone_id, @company_id, @manufacturer, @series, @screen_diagonal, @memory, @sim_num, @color, @camera, @front_camera, @price, @date_creation)";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[+ Добавить Телефон]");

                    Phone phone = new Phone().SetId().SetCompany_Id();
                    phone.SetSpecifications(list.companies[phone.Company_id]).SetDate();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@phone_id", phone.Id);
                    cmd.Parameters.AddWithValue("@company_id", phone.Company_id);
                    cmd.Parameters.AddWithValue("@manufacturer", phone.Manufacturer);
                    cmd.Parameters.AddWithValue("@series", phone.Series);
                    cmd.Parameters.AddWithValue("@screen_diagonal", phone.Screen_Diagonal);
                    cmd.Parameters.AddWithValue("@memory", phone.Memory);
                    cmd.Parameters.AddWithValue("@sim_num", phone.SIM_Num);
                    cmd.Parameters.AddWithValue("@color", phone.Color);
                    cmd.Parameters.AddWithValue("@camera", phone.Camera);
                    cmd.Parameters.AddWithValue("@front_camera", phone.Front_Camera);
                    cmd.Parameters.AddWithValue("@price", phone.Price);
                    cmd.Parameters.AddWithValue("@date_creation", phone.Date_Creation.ToString("yyyy-MM-dd H:mm:ss"));

                    cmd.ExecuteNonQuery();
                    list.phones.Add(phone.Id, phone);
                    connection.Close();


                    Console.Write("Введите количество сетей: ");
                    int num = int.Parse(Console.ReadLine());

                    for (int i = 0; i < num || i < 0; i++)
                        new NetworkDAO(phone.Id).Add();

                    Update("Add", phone, phone.Id);

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
            if (IsEmptyList(list.phones))
            {
                Console.WriteLine("База данных телефонов пуста! Добавьте!");
                return;
            }

            string sql = "UPDATE Phone SET "
                         + "Manufacturer = @manufacturer, "
                         + "Series = @series, "
                         + "Screen_Diagonal = @screen_diagonal, "
                         + "Memory = @memory, "
                         + "SIM_Num = @sim_num, "
                         + "Color = @color, "
                         + "Camera = @camera, "
                         + "Front_Camera = @front_camera, "
                         + "Price = @price "
                         + "WHERE Phone_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                while (true)
                {
                    try
                    {
                        Console.WriteLine("[= Изменить данные Телефона]");
                        GetList();

                        Console.Write("\nВведите id телефона: ");
                        int Id = int.Parse(Console.ReadLine());
                        Console.Clear();

                        Console.Write(list.phones[Id]);

                        Phone phone = list.phones[Id].SetId().SetCompany_Id();
                        phone.SetSpecifications(list.companies[phone.Id]);

                        MySqlCommand cmd = new MySqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@manufacturer", phone.Manufacturer);
                        cmd.Parameters.AddWithValue("@series", phone.Series);
                        cmd.Parameters.AddWithValue("@screen_diagonal", phone.Screen_Diagonal);
                        cmd.Parameters.AddWithValue("@memory", phone.Memory);
                        cmd.Parameters.AddWithValue("@sim_num", phone.SIM_Num);
                        cmd.Parameters.AddWithValue("@color", phone.Color);
                        cmd.Parameters.AddWithValue("@camera", phone.Camera);
                        cmd.Parameters.AddWithValue("@front_camera", phone.Front_Camera);
                        cmd.Parameters.AddWithValue("@price", phone.Price);
                        cmd.Parameters.AddWithValue("@id", Id);

                        cmd.ExecuteNonQuery();
                        Update("Change", phone, Id);

                    }
                    catch (Exception ex)
                    {
                        var edi = ExceptionDispatchInfo.Capture(ex);
                        LogException(edi);
                    }
                }
            }
        }
        public override void Find()
        {
            if (IsEmptyList(list.phones))
            {
                Console.WriteLine("База данных телефонов пуста! Добавьте!");
                return;
            }

            string sql = "SELECT * FROM Phone WHERE concat("
                + "Manufacturer, "
                + "COALESCE(Series, ''), "
                + "Screen_Diagonal, "
                + "Memory, SIM_Num, "
                + "Color, "
                + "COALESCE(Camera, ''), "
                + "COALESCE(Front_Camera, ''), "
                + "Price, "
                + "Date_Creation) "
                + "Like @text";

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
                        int company_id = reader.GetInt32(1);
                        string manufacturer = reader.GetString(2);
                        string series = reader.IsDBNull(3) ? null : reader.GetString(3);
                        double screen_diagonal = reader.GetDouble(4);
                        double memory = reader.GetDouble(5);
                        int sim_num = reader.GetInt32(6);
                        string color = reader.GetString(7);
                        string camera = reader.IsDBNull(8) ? null : reader.GetString(8);
                        string front_camera = reader.IsDBNull(9) ? null : reader.GetString(9);
                        double price = reader.GetDouble(10);
                        string date_creation = reader.GetString(11);

                        Phone phone = new Phone().Builder(id, company_id, manufacturer, screen_diagonal, memory, sim_num, color,
                            price, DateTime.Parse(date_creation), list.companies[company_id], series, camera, front_camera);

                        Console.WriteLine(phone);
                    }
                }
                else
                    Console.WriteLine($"Нет результатов поиска!");
            }
        }
        public override void Remove()
        {
            if (IsEmptyList(list.phones))
            {
                Console.WriteLine("База данных телефонов пуста! Добавьте!");
                return;
            }

            string sql = $"DELETE FROM Phone WHERE Phone_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[- Удалить Телефон]");
                    GetList();
                    Console.Write("\nВведите Id: ");
                    int Id = int.Parse(Console.ReadLine());

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();

                    Update("Remove", list.phones[Id], Id);
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
            if (IsEmptyList(list.phones))
            {
                Console.WriteLine("База данных телефонов пуста! Добавьте!");
                return;
            }

            int count = 0;
            Console.WriteLine("[Список Телефонов]\n");
            foreach (var phone in list.phones)
            {
                count++;
                Console.WriteLine($"[{count}]");
                Console.WriteLine(phone.Value);
            }
        }
        protected override void Update<T>(string operation, T entity, int Id)
        {
            Phone phone = entity as Phone;

            if (operation == "Add")
            {
                Console.WriteLine($"\nТелефон({phone.Id}) из компании({phone.Company_id}) успешно добавлен в БД!\n");
            }

            else if (operation == "Change")
            {
                list.phones[Id] = phone;
                Console.WriteLine($"\nТелефон {phone.Id} из компании({phone.Company_id}) успешно изменён в БД!\n");
            }

            else if (operation == "Remove")
            {
                list.phones.Remove(Id);
                Console.WriteLine($"\nТелефон {phone.Id} из компании({phone.Company_id}) успешно удалён из БД!\n");
            }
        }
    }
}
