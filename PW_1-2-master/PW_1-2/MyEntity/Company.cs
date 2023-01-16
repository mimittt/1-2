using System;
using System.Collections.Generic;
using System.Text;

namespace PW_1_2.MyEntity
{
    public class Company
    {
        public int Id;
        public int Director_id;
        public string Name;
        public string Address;
        public DateTime Date_Creation;
        public Director Director;
        public List<Phone> PhoneList = new List<Phone>();

        public Company()
        {
            Console.WriteLine("\n[Компания]");
        }
        public Company(int company_id, int director_id, string companyName, string address, DateTime creation, Director director)
        {
            Id = company_id;
            Director_id = director_id;
            Name = companyName;
            Address = address;
            Date_Creation = creation;
            Director = director;
        }

        public Company SetId()
        {
            if(Name != null)
            {
                Console.WriteLine($"Id компании: {Id}");
                return this;
            }

            Console.Write("Введите id: ");
            Id = int.Parse(Console.ReadLine());

            return this;
        }
        public Company SetDirectorId(int? id)
        {
            if (id != null) // Проверка id на пустоту для метода Add()
            {
                Director_id = (int)id;
                Console.WriteLine($"Id Директора: {Director_id}");
                return this;
            }

            if (Name != null) // Проверка Name на пустоту для метода Change()
            {
                Console.WriteLine($"Id Директора: {Director_id}");
                return this;
            }

            Console.Write("Введите director id: ");
            Director_id = int.Parse(Console.ReadLine());

            return this;
        }
        public Company SetFullData(Director director)
        {
            Director = director;

            Console.Write("Введите название: ");
            Name = Console.ReadLine();

            Console.Write("Введите адресс: ");
            Address = Console.ReadLine();
            Console.WriteLine();

            return this;
        }
        public Company SetDate()
        {
            Date_Creation = DateTime.Now;
            return this;
        }

        public override string ToString() =>
            new StringBuilder()
            .AppendLine($"Id: {Id}")
            .AppendLine($"Director id: {Director_id}")
            .AppendLine($"Company: {Name}")
            .AppendLine($"Address: {Address}")
            .AppendLine($"Owner: {Director.Surname} {Director.Name} {Director.Middle_name}")
            .AppendLine($"Creation: {Date_Creation}")
            .ToString();


    }
}
