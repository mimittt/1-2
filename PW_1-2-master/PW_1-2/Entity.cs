using PW_1_2.Database;
using PW_1_2.MyEntity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace PW_1_2
{
    public struct EntityList
    {
        public Dictionary<int, Director> directors;
        public Dictionary<int, Company> companies;
        public Dictionary<int, Phone> phones;
        public Dictionary<int, Network> networks;
    }

    public class Entity
    {
        protected static EntityList list;

        protected bool IsEmptyList<N,T>(Dictionary<N, T> list)
        {
            if (list == null)
                return true;

            if (list.Count == 0)
                return true;

            return false;
        }
        protected void LogException(ExceptionDispatchInfo edi) // Лог возможных Ошибок
        {
            try
            {
                edi.Throw();
            }
            catch (FormatException ex)
            {
                Console.WriteLine("\n[Неккоректный ввод!]");
                Console.WriteLine(ex.Message);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("\n[Ошибка в базе данных!]");
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("\n[Ошибка в вводе Id!]");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[Ошибка!]");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
