using System;
using System.Text;

namespace PW_1_2.MyEntity
{
    public class Director
    {
        public int Id;
        public string Name;
        public string Surname;
        public string Middle_name;
        public int Age;
        public string Phone_number;

        public Director()
        {
            Console.WriteLine("\n[Директор]");
        }

        public Director(int director_id, string name, string surname, string middle_name, int age, string phone_number)
        {
            Id = director_id;
            Name = name;
            Surname = surname;
            Middle_name = middle_name;
            Age = age;
            Phone_number = phone_number;

        }

        public Director SetId()
        {
            if (Name != null)
            {
                Console.WriteLine($"Id директора: {Id}");
                return this;
            }

            Console.Write("Введите id директора: ");
            Id = int.Parse(Console.ReadLine());

            return this;
        }

        public Director SetFullData()
        {
            Console.Write("Введите имя: ");
            Name = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            Surname = Console.ReadLine();

            Console.Write("Введите отчество: ");
            Middle_name = Console.ReadLine();

            Console.Write("Введите возраст: ");
            Age = int.Parse(Console.ReadLine());

            Console.Write("Введите номер: ");
            Phone_number = Console.ReadLine();
            Console.WriteLine();

            return this;
        }


        public override string ToString() =>
            new StringBuilder()
            .AppendLine($"Id: {Id}")
            .AppendLine($"Full name: {Surname} {Name} {Middle_name}")
            .AppendLine($"Age: {Age}")
            .AppendLine($"Phone number: {Phone_number}")
            .ToString();

    }
}
