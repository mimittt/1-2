using System;
using System.Collections.Generic;
using System.Text;

namespace PW_1_2.MyEntity
{
    public class Phone
    {
        public int Id;
        public int Company_id;
        public Company Company;

        //Обязательные поля
        public string Manufacturer;
        public double Screen_Diagonal;
        public double Memory;
        public int SIM_Num;
        public string Color;
        public double Price;
        public DateTime Date_Creation;

        //Поля на выбор
        public string Series = null;
        public string Camera = null;
        public string Front_Camera = null;
        public List<Network> NetworkList = new List<Network>();

        public Phone() {}
        public Phone Builder(int phone_id, int company_id, string manufacturer,  
            double screen_diagonal, double memory, int sim_num, string color, double price, 
            DateTime date_creation, Company company, string series, string camera, string front_camera)
        {
            Id = phone_id;
            Company_id = company_id;
            Manufacturer = manufacturer;
            Series = series;
            Screen_Diagonal = screen_diagonal;
            Memory = memory;
            SIM_Num = sim_num;
            Color = color;
            Date_Creation = date_creation;
            Camera = camera;
            Front_Camera = front_camera;
            Price = price;
            Company = company;
            return this;
        }

        public Phone SetId()
        {
            if (Manufacturer != null)
            {
                Console.WriteLine($"Id Телефона: {Id}");
                return this;
            }

            Console.Write("Введите id телефона: ");
            Id = int.Parse(Console.ReadLine());

            return this;
        }
        public Phone SetCompany_Id()
        {
            if (Manufacturer != null)
            {
                Console.WriteLine($"Id Компании: {Id}");
                return this;
            }

            Console.Write("Введите company id: ");
            Company_id = int.Parse(Console.ReadLine());
            return this;
        }
        public Phone SetSeries()
        {
            Console.Write("Введите серию(Необязательно): ");
            Series = Console.ReadLine();

            if (Series == "")
            {
                Series = null;
                return this;
            }

            return this;
        }
        public Phone SetSpecifications(Company company)
        {
            Manufacturer = company.Name;
            Console.WriteLine($"Производитель: {Manufacturer}");

            SetSeries();

            Console.Write("Введите диагональ(0,0): ");
            Screen_Diagonal = double.Parse(Console.ReadLine());

            Console.Write("Введите количество памяти(0,0): ");
            Memory = double.Parse(Console.ReadLine());

            Console.Write("Введите количество сим-карт: ");
            SIM_Num = int.Parse(Console.ReadLine());

            SetCamera();
            SetFrontCamera();

            Console.Write("Введите Цвет: ");
            Color = Console.ReadLine();

            Console.Write("Введите цену($): ");
            Price = double.Parse(Console.ReadLine()); ;

            return this;
        }
        public Phone SetDate()
        {
            Date_Creation = DateTime.Now;
            return this;
        }
        public Phone SetCamera()
        {
            Console.Write("Введите камеру(Необязательно): ");
            Camera = Console.ReadLine();

            if (Camera == "")
            {
                Camera = null;
                return this;
            }

            return this;
        }
        public Phone SetFrontCamera()
        {
            Console.Write("Введите фронтальную камеру(Необязательно): ");
            Front_Camera = Console.ReadLine();

            if (Front_Camera == "")
            {
                Front_Camera = null;
                return this;
            }

            return this;
        }
        public Phone SetNetwork(Network network)
        {
            NetworkList.Add(network);
            return this;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Phone id: {Id} | Company id(FK): {Company_id}");
            builder.AppendLine($"Manufacturer: {Manufacturer}");

            if (Series != null)
                builder.AppendLine($"Series: {Series}");

            builder.AppendLine($"Screen diagonal: {Screen_Diagonal}\"");
            builder.AppendLine($"Memory: {Memory}GB");
            builder.AppendLine($"SIM num: {SIM_Num}");
            builder.AppendLine($"Color: {Color}");

            if (Camera != null)
                builder.AppendLine($"Camera: {Camera}");
            if (Front_Camera != null)
                builder.AppendLine($"Front camera: {Front_Camera}");
            if (NetworkList.Count != 0)
            {
                builder.AppendLine("Internet:");
                foreach (Network i in NetworkList)
                    builder.AppendLine($"  - {i.Name}");
            }

            builder.AppendLine($"Date creation: {Date_Creation}");
            builder.AppendLine($"Price: {Price}$");

            return builder.ToString();
        }
    }
}
