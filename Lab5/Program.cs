using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace FileOperationsLab
{
    // Zad 5
    public class Student
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public List<int> Oceny { get; set; }

        public Student() { }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko}, Oceny: [{string.Join(", ", Oceny)}]";
        }
    }

    public class IrisRecord
    {
        public double SepalLength { get; set; }
        public double SepalWidth { get; set; }
        public double PetalLength { get; set; }
        public double PetalWidth { get; set; }
        public string Class { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string textFile = "dane.txt";
            string jsonFile = "studenci.json";
            string xmlFile = "studenci.xml";
            string csvFile = "Iris.csv";

            GenerateDummyIrisCsv(csvFile);

            Console.WriteLine("--- ZADANIE 2: Zapis tekstu ---");
            Zadanie2_ZapiszTekst(textFile);

            Console.WriteLine("\n--- ZADANIE 3: Odczyt tekstu ---");
            Zadanie3_OdczytajTekst(textFile);

            Console.WriteLine("\n--- ZADANIE 4: Dopisywanie tekstu ---");
            Zadanie4_DopiszTekst(textFile);
            Zadanie3_OdczytajTekst(textFile);

            Console.WriteLine("\n--- ZADANIE 6: Serializacja JSON ---");
            var studenci = StworzStudentow();
            Zadanie6_ZapiszJSON(studenci, jsonFile);

            Console.WriteLine("\n--- ZADANIE 7: Deserializacja JSON ---");
            Zadanie7_OdczytajJSON(jsonFile);

            Console.WriteLine("\n--- ZADANIE 8: Serializacja XML ---");
            Zadanie8_ZapiszXML(studenci, xmlFile);

            Console.WriteLine("\n--- ZADANIE 9: Deserializacja XML ---");
            Zadanie9_OdczytajXML(xmlFile);

            Console.WriteLine("\n--- ZADANIE 10: Odczyt CSV (Iris) ---");
            var irisData = Zadanie10_OdczytajCSV(csvFile);

            Console.WriteLine("\n--- ZADANIE 11: Statystyki CSV ---");
            Zadanie11_StatystykiCSV(irisData);

            Console.WriteLine("\n--- ZADANIE 12: Filtrowanie CSV ---");
            Zadanie12_FiltrujCSV(irisData, "iris_filtered.csv");

            Console.WriteLine("\nKoniec programu. Naciśnij klawisz.");
            Console.ReadKey();
        }

        static List<Student> StworzStudentow()
        {
            return new List<Student>
            {
                new Student { Imie = "Jan", Nazwisko = "Kowalski", Oceny = new List<int> { 3, 4, 5 } },
                new Student { Imie = "Anna", Nazwisko = "Nowak", Oceny = new List<int> { 5, 5, 4, 5 } },
                new Student { Imie = "Piotr", Nazwisko = "Wiśniewski", Oceny = new List<int> { 2, 3 } }
            };
        }

        // Zad 2
        static void Zadanie2_ZapiszTekst(string sciezka)
        {
            Console.WriteLine("Podaj 3 linie tekstu do zapisania:");
            using (StreamWriter sw = new StreamWriter(sciezka))
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.Write($"Linia {i + 1}: ");
                    string tekst = Console.ReadLine();
                    sw.WriteLine(tekst);
                }
            }
            Console.WriteLine($"Zapisano dane do pliku: {sciezka}");
        }

        // Zad 3
        static void Zadanie3_OdczytajTekst(string sciezka)
        {
            if (File.Exists(sciezka))
            {
                Console.WriteLine($"Zawartość pliku {sciezka}:");
                using (StreamReader sr = new StreamReader(sciezka))
                {
                    string linia;
                    while ((linia = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(linia);
                    }
                }
            }
            else
            {
                Console.WriteLine("Plik nie istnieje.");
            }
        }

        // Zad 4
        static void Zadanie4_DopiszTekst(string sciezka)
        {
            Console.WriteLine("Podaj linię do dopisania:");
            string tekst = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter(sciezka, append: true))
            {
                sw.WriteLine(tekst);
            }
            Console.WriteLine("Dopisano treść.");
        }

        // Zad 6
        static void Zadanie6_ZapiszJSON(List<Student> lista, string sciezka)
        {
            // Opcje dla ładnego formatowania w pliku
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(lista, options);
            File.WriteAllText(sciezka, jsonString);
            Console.WriteLine($"Zapisano listę studentów do JSON: {sciezka}");
        }

        // Zad 7
        static void Zadanie7_OdczytajJSON(string sciezka)
        {
            if (!File.Exists(sciezka)) return;

            string jsonString = File.ReadAllText(sciezka);
            List<Student> lista = JsonSerializer.Deserialize<List<Student>>(jsonString);

            Console.WriteLine("Odczytani studenci z JSON:");
            foreach (var s in lista)
            {
                Console.WriteLine(s.ToString());
            }
        }

        // Zad 8
        static void Zadanie8_ZapiszXML(List<Student> lista, string sciezka)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
            using (StreamWriter sw = new StreamWriter(sciezka))
            {
                serializer.Serialize(sw, lista);
            }
            Console.WriteLine($"Zapisano listę studentów do XML: {sciezka}");
        }

        // Zad 9
        static void Zadanie9_OdczytajXML(string sciezka)
        {
            if (!File.Exists(sciezka)) return;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
            using (StreamReader sr = new StreamReader(sciezka))
            {
                List<Student> lista = (List<Student>)serializer.Deserialize(sr);
                Console.WriteLine("Odczytani studenci z XML:");
                foreach (var s in lista)
                {
                    Console.WriteLine(s.ToString());
                }
            }
        }

        // Zad 10
        static List<IrisRecord> Zadanie10_OdczytajCSV(string sciezka)
        {
            var records = new List<IrisRecord>();
            if (!File.Exists(sciezka))
            {
                Console.WriteLine("Brak pliku CSV.");
                return records;
            }

            string[] lines = File.ReadAllLines(sciezka);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var columns = line.Split(',');

                records.Add(new IrisRecord
                {
                    SepalLength = double.Parse(columns[0], CultureInfo.InvariantCulture),
                    SepalWidth = double.Parse(columns[1], CultureInfo.InvariantCulture),
                    PetalLength = double.Parse(columns[2], CultureInfo.InvariantCulture),
                    PetalWidth = double.Parse(columns[3], CultureInfo.InvariantCulture),
                    Class = columns[4]
                });
            }

            Console.WriteLine($"Wczytano {records.Count} wierszy. Oto 5 pierwszych:");
            foreach (var r in records.Take(5))
            {
                Console.WriteLine($"{r.SepalLength}, {r.SepalWidth}, {r.PetalLength}, {r.PetalWidth}, {r.Class}");
            }

            return records;
        }

        // Zad 11
        static void Zadanie11_StatystykiCSV(List<IrisRecord> dane)
        {
            if (dane.Count == 0) return;

            double avgSepalLen = dane.Average(x => x.SepalLength);
            double avgSepalWid = dane.Average(x => x.SepalWidth);
            double avgPetalLen = dane.Average(x => x.PetalLength);
            double avgPetalWid = dane.Average(x => x.PetalWidth);

            Console.WriteLine("Średnie wartości:");
            Console.WriteLine($"Sepal Length: {avgSepalLen:F2}");
            Console.WriteLine($"Sepal Width:  {avgSepalWid:F2}");
            Console.WriteLine($"Petal Length: {avgPetalLen:F2}");
            Console.WriteLine($"Petal Width:  {avgPetalWid:F2}");
        }

        // Zad 12
        static void Zadanie12_FiltrujCSV(List<IrisRecord> dane, string sciezkaWynikowa)
        {
            var filteredData = dane
                .Where(x => x.SepalLength < 5.0)
                .Select(x => new { x.SepalLength, x.SepalWidth, x.Class })
                .ToList();

            using (StreamWriter sw = new StreamWriter(sciezkaWynikowa))
            {
                sw.WriteLine("sepal_length,sepal_width,class");

                foreach (var item in filteredData)
                {
                    string line = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}",
                        item.SepalLength, item.SepalWidth, item.Class);
                    sw.WriteLine(line);
                }
            }
            Console.WriteLine($"Zapisano przefiltrowane dane ({filteredData.Count} wierszy) do pliku: {sciezkaWynikowa}");
        }

        static void GenerateDummyIrisCsv(string path)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("sepal_length,sepal_width,petal_length,petal_width,class");
                    sw.WriteLine("5.1,3.5,1.4,0.2,Iris-setosa");
                    sw.WriteLine("4.9,3.0,1.4,0.2,Iris-setosa");
                    sw.WriteLine("4.7,3.2,1.3,0.2,Iris-setosa");
                    sw.WriteLine("7.0,3.2,4.7,1.4,Iris-versicolor");
                    sw.WriteLine("6.4,3.2,4.5,1.5,Iris-versicolor");
                    sw.WriteLine("6.3,3.3,6.0,2.5,Iris-virginica");
                }
                Console.WriteLine("Utworzono przykładowy plik Iris.csv");
            }
        }
    }
}