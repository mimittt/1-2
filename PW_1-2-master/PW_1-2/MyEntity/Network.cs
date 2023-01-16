using System;

namespace PW_1_2.MyEntity
{
    public class Network
    {
        public int Id;
        public int Phone_id;
        public string Name;
        public Phone Phone;

        public Network()
        {
            Console.WriteLine("\n[Сеть]");
        }
        public Network(int network_id, int phone_id, string name, Phone phone)
        {
            Phone_id = phone_id;
            Id = network_id;
            Name = name;
            Phone = phone;
        }

        public Network SetId()
        {
            if(Name != null)
            {

                Console.WriteLine($"Id сети: {Id}");
                return this;
            }

            Console.Write("Введите id: ");
            Id = int.Parse(Console.ReadLine());

            return this;
        }
        public Network SetPhoneId(int? id)
        {
            if (id != null || Name != null)
            {
                Phone_id = (int)id;
                Console.WriteLine($"Id телефона: {Phone_id}");
                return this;
            }

            Console.Write("Введите id телефона: ");
            Phone_id = int.Parse(Console.ReadLine());

            return this;
        }
        public Network SetFullData(Phone phone)
        {
            Console.Write("Введите название: ");
            Name = Console.ReadLine();

            Phone = phone;
            return this;
        }

        public override string ToString() =>
            ($"Name: {Name} Internet id: {Id} | Phone id(FK): {Phone_id}");
    }
}
