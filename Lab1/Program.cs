// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World1!");
Console.WriteLine("Hello, World2!");

using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("To jest cwiczenie 1"); 

            Zwierze[] animals = new Zwierze[3];

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Enter data for animal #{i+1}:");
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Species: ");
                string species = Console.ReadLine();

                Console.Write("Number of legs (integer): ");
                int legs = 0;
                while (!int.TryParse(Console.ReadLine(), out legs))
                {
                    Console.Write("Please enter a valid integer for number of legs: ");
                }

                animals[i] = new Zwierze(name, species, legs);
                Console.WriteLine();
            }

            Zwierze clone = new Zwierze(animals[0]); 
            clone.Nazwa = clone.Nazwa + "_clone";

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Animal #{i+1}: Name={animals[i].Nazwa}, Species={animals[i].Gatunek}, Legs={animals[i].LiczbaNog}");
                animals[i].DajGlos();
            }

            Console.WriteLine($"Cloned animal: Name={clone.Nazwa}, Species={clone.Gatunek}, Legs={clone.LiczbaNog}");
            clone.DajGlos();

            Console.WriteLine($"Total animals created: {Zwierze.GetLiczbaZwierzat()}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    class Zwierze
    {
        private string nazwa;
        private string gatunek;
        private int liczbaNog;

        private static int liczbaZwierzat = 0;

        public string Nazwa
        {
            get { return nazwa; }
            set { nazwa = value; }
        }

        public string Gatunek
        {
            get { return gatunek; }
            set { gatunek = value; }
        }

        public int LiczbaNog
        {
            get { return liczbaNog; }
            set { liczbaNog = value; }
        }

        public Zwierze()
        {
            nazwa = "Rex";
            gatunek = "Pies";
            liczbaNog = 4;
            liczbaZwierzat++;
        }

        public Zwierze(string nazwa, string gatunek, int liczbaNog)
        {
            this.nazwa = nazwa;
            this.gatunek = gatunek;
            this.liczbaNog = liczbaNog;
            liczbaZwierzat++;
        }

        public Zwierze(Zwierze other)
        {
            this.nazwa = other.nazwa;
            this.gatunek = other.gatunek;
            this.liczbaNog = other.liczbaNog;
            liczbaZwierzat++;
        }

        public void DajGlos()
        {
            string lower = gatunek?.ToLower() ?? "";
            switch (lower)
            {
                case "kot":
                case "cat":
                    Console.WriteLine("Miau");
                    break;
                case "krowa":
                case "cow":
                    Console.WriteLine("Muuu!");
                    break;
                case "pies":
                case "dog":
                    Console.WriteLine("Hau hau!");
                    break;
                default:
                    Console.WriteLine("[No specific sound for this species]");
                    break;
            }
        }

        public static int GetLiczbaZwierzat()
        {
            return liczbaZwierzat;
        }

        ~Zwierze()
        {
        }
    }
}
