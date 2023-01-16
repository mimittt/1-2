using MySql.Data.MySqlClient;
using System;

namespace PW_1_2.Database
{
    public class Datebase
    {
        private static Datebase _instance = null;
        private static MySqlConnection _connection = null;

        // Свои данные для подключение к БД
        private readonly string Server = "localhost";
        private readonly string User = "root";
        private readonly string Password = "test12345";
        private readonly string Database = "phone";

        private Datebase()
        { 

            try
            {
                _connection = new MySqlConnection($"server={Server};user={User};password={Password};database={Database};");
                _connection.Open();
            }
            catch
            {
                Console.WriteLine("Ошибка при подключение к БД!");
            }
            finally
            {
                _connection.Close();
            }

            LoadingTable();
        }


        public static Datebase GetInstance()
        {
            if (_instance == null)
                _instance = new Datebase();
            return _instance;
        }

        public static MySqlConnection GetConnection() => _connection;

        private void LoadingTable()
        {
            new DirectorDAO();
            new CompanyDAO();
            new PhoneDAO();
            new NetworkDAO();
        }


    }

}

