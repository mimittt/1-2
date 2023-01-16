using System;
using PW_1_2.MyEntity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace PW_1_2.Database
{
    public class NetworkDAO : EntityDAO
    {
        readonly private int? Phone_Id = null;

        public NetworkDAO()
        {
            if (!IsEmptyList(list.networks))
                return;
            
            list.networks = new Dictionary<int, Network>();

            // Запрос на создание таблицы "Network", если её нету
            string sql = "CREATE TABLE IF NOT EXISTS Network("
            + "Network_id INT NOT NULL AUTO_INCREMENT, "
            + "Phone_id INT NOT NULL, "
            + "Name VARCHAR(20) NOT NULL, "
            + "PRIMARY KEY(Network_id), "
            + "CONSTRAINT fk_Network_Phone "
            + "FOREIGN KEY (Phone_id) REFERENCES Phone (Phone_id) "
            + "ON DELETE CASCADE "
            + "ON UPDATE CASCADE) "
            + "COLLATE='utf8_general_ci' ENGINE=InnoDB;";


            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }


            // Запрос на получения данных таблицы "Network"
            sql = "SELECT * FROM Network";

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
                        int phone_id = reader.GetInt32(1);
                        string name = reader.GetString(2);

                        Network network = new Network(id, phone_id, name, list.phones[phone_id]);

                        list.networks.Add(id, network);
                        list.phones[phone_id].SetNetwork(network);
                    }
                }
            }
            
        }
        public NetworkDAO(int? phone_id)
        {
            Phone_Id = phone_id;
        }

        public override void Add()
        {
            if (IsEmptyList(list.phones))
            {
                Console.WriteLine("База данных телефонов пуста!");
                return;
            }

            string sql = "INSERT INTO Network (Network_id, Phone_id, Name) "
                + "VALUES (@network_id, @phone_id, @name)";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[+ Добавить Сеть]");

                    Network network = new Network().SetId().SetPhoneId(Phone_Id);
                    network.SetFullData(list.phones[network.Phone_id]);

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@network_id", network.Id);
                    cmd.Parameters.AddWithValue("@phone_id", network.Phone_id);
                    cmd.Parameters.AddWithValue("@name", network.Name);

                    cmd.ExecuteNonQuery();
                    list.phones[network.Phone_id].SetNetwork(network);
                    Update("Add", network, network.Id);

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
                Console.WriteLine("База данных сетей пуста! Добавьте!");
                return;
            }

            string sql = "UPDATE Network SET "
                         + "Name = @name "
                         + "WHERE Network_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                while (true)
                {
                    try
                    {
                        Console.WriteLine("[= Изменить данные Сети]");
                        GetList();

                        Console.Write("\nВведите id сети: ");
                        int Id = int.Parse(Console.ReadLine());
                        Console.Clear();

                        Console.Write(list.companies[Id]);

                        Network network = list.networks[Id].SetFullData(list.networks[Id].Phone);

                        MySqlCommand cmd = new MySqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@name", network.Name);
                        cmd.Parameters.AddWithValue("@id", Id);

                        cmd.ExecuteNonQuery();
                        Update("Change", network, Id);

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
                Console.WriteLine("База данных сетей пуста! Добавьте!");
                return;
            }

            string sql = $"SELECT * FROM Network WHERE concat(Phone_id, Name) LIKE @text";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                Console.WriteLine("[? Поиск Сети]");
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
                        int phone_id = reader.GetInt32(1);
                        string name = reader.GetString(2);

                        Network network = new Network(id, phone_id, name, list.phones[phone_id]);

                        Console.WriteLine(network);
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
                Console.WriteLine("База данных сетей пуста! Добавьте!");
                return;
            }

            string sql = "DELETE FROM Network WHERE Network_id = @id";

            using (MySqlConnection connection = Datebase.GetConnection())
            {
                connection.Open();

                try
                {
                    Console.WriteLine("[- Удалить Сеть]");
                    GetList();
                    Console.Write("\nВведите Id: ");
                    int Id = int.Parse(Console.ReadLine());

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();

                    Update("Remove", list.networks[Id], Id);
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
                Console.WriteLine("База данных сетей пуста! Добавьте!");
                return;
            }

            int count = 0;
            Console.WriteLine("[Список Сетей]\n");
            foreach (var network in list.networks)
            {
                count++;
                Console.WriteLine($"[{count}]");
                Console.WriteLine(network.Value);
            }
        }
        protected override void Update<T>(string operation, T entity, int Id)
        {
            Network network = entity as Network;

            if (operation == "Add")
            {
                list.networks.Add(Id, network);

                if (Phone_Id == null)
                    Console.WriteLine($"Сеть {Id} успешно добавлена в БД!\n");
                else
                    Console.WriteLine($"Успешно поставлена сеть:{network.Name} для телефона {Phone_Id} в БД!\n");
            }

            else if (operation == "Change")
            {
                list.networks[Id] = network;
                Console.WriteLine($"Сеть {network.Id} успешно изменена в БД!\n");
            }

            else if (operation == "Remove")
            {
                list.networks.Remove(Id);
                Console.WriteLine($"Сеть {network.Id} успешно удалена из БД!\n");
            }
        }
    }
}
