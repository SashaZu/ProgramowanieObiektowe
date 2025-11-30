using System;
using System.Collections.Generic;
using System.Linq;

// Zad 1
public interface IModular
{
    public double Module();
}


public class ComplexNumber : ICloneable, IEquatable<ComplexNumber>, IModular, IComparable<ComplexNumber>
{
    private double re;
    private double im;

    public double Re { get => re; set => re = value; }
    public double Im { get => im; set => im = value; }

    public ComplexNumber(double re, double im)
    {
        this.re = re;
        this.im = im;
    }

    public override string ToString()
    {
        string sign = im >= 0 ? "+" : "-";
        return $"{re} {sign} {Math.Abs(im)}i (Moduł: {Module():F2})";
    }

    public int CompareTo(ComplexNumber other)
    {
        if (other == null) return 1;
        return this.Module().CompareTo(other.Module());
    }


    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        => new ComplexNumber(a.re + b.re, a.im + b.im);
    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        => new ComplexNumber(a.re - b.re, a.im - b.im);
    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        => new ComplexNumber(a.re * b.re - a.im * b.im, a.re * b.im + a.im * b.re);
    public static ComplexNumber operator -(ComplexNumber a)
        => new ComplexNumber(a.re, -a.im);

    public object Clone() => new ComplexNumber(re, im);

    public bool Equals(ComplexNumber other)
    {
        if (other == null) return false;
        return re == other.re && im == other.im;
    }

    public override bool Equals(object obj)
        => obj is ComplexNumber other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(re, im);

    public static bool operator ==(ComplexNumber a, ComplexNumber b)
        => a?.Equals(b) ?? b is null;
    public static bool operator !=(ComplexNumber a, ComplexNumber b)
        => !(a == b);

    public double Module()
        => Math.Sqrt(re * re + im * im);
}

// Zad 2

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== ZADANIE 2: TABLICE ===");
        ComplexNumber[] tablica = new ComplexNumber[]
        {
            new ComplexNumber(3, 4),   
            new ComplexNumber(1, -1), 
            new ComplexNumber(0, 10), 
            new ComplexNumber(2, 2),
            new ComplexNumber(5, -5)  
        };

        // 2a
        Console.WriteLine("\n--- Tablica: Foreach ---");
        foreach (var c in tablica) Console.WriteLine(c);

        // 2b
        Array.Sort(tablica);
        Console.WriteLine("\n--- Tablica: Posortowana wg modułu ---");
        foreach (var c in tablica) Console.WriteLine(c);

        // 2c
        Console.WriteLine($"\nMinimum (wg modułu): {tablica.Min()}");
        Console.WriteLine($"Maksimum (wg modułu): {tablica.Max()}");

        // 2d
        Console.WriteLine("\n--- Tablica: Filtrowanie (Im < 0) ---");
        var przefiltrowane = tablica.Where(c => c.Im < 0);
        foreach (var c in przefiltrowane) Console.WriteLine(c);


        Console.WriteLine("\n\n=== ZADANIE 3: LISTY ===");
        List<ComplexNumber> lista = new List<ComplexNumber>
        {
            new ComplexNumber(1, 1),
            new ComplexNumber(10, 20), 
            new ComplexNumber(0.5, 0.5),
            new ComplexNumber(3, -3),
            new ComplexNumber(4, 4)
        };

        Console.WriteLine($"Max z listy: {lista.Max()}");
        lista.Sort(); 

        // Zad 3
        // 3a
        if (lista.Count > 1) lista.RemoveAt(1);
        Console.WriteLine("\n--- Lista po usunięciu elementu o indeksie 1 ---");
        lista.ForEach(Console.WriteLine);

        // 3b
        var minVal = lista.Min();
        lista.Remove(minVal);
        Console.WriteLine($"\n--- Lista po usunięciu minimum ({minVal}) ---");
        lista.ForEach(Console.WriteLine);

        // 3c
        lista.Clear();
        Console.WriteLine($"\nCzy lista jest pusta? {lista.Count == 0}");


        Console.WriteLine("\n\n=== ZADANIE 4: HASHSET (Zbiór) ===");
        var z1 = new ComplexNumber(6, 7);
        var z2 = new ComplexNumber(1, 2);
        var z3 = new ComplexNumber(6, 7); 
        var z4 = new ComplexNumber(1, -2);
        var z5 = new ComplexNumber(-5, 9);

        HashSet<ComplexNumber> zbior = new HashSet<ComplexNumber>();
        zbior.Add(z1);
        zbior.Add(z2);
        zbior.Add(z3); 
        zbior.Add(z4);
        zbior.Add(z5);

        // Zad 4
        // 4a
        Console.WriteLine("--- Zawartość HashSet (z1 i z3 zredukowane do jednego) ---");
        foreach (var z in zbior) Console.WriteLine(z);

        // 4b
        Console.WriteLine($"\nMin w zbiorze: {zbior.Min()}");
        Console.WriteLine("--- Zbiór posortowany (przez LINQ) ---");
        foreach (var z in zbior.OrderBy(x => x)) Console.WriteLine(z);


        Console.WriteLine("\n\n=== ZADANIE 5: SŁOWNIK ===");
        Dictionary<string, ComplexNumber> slownik = new Dictionary<string, ComplexNumber>
        {
            {"z1", z1},
            {"z2", z2},
            {"z3", z3}, 
            {"z4", z4},
            {"z5", z5}
        };

        // Zad 5
        // 5a
        Console.WriteLine("--- Słownik: Klucz -> Wartość ---");
        foreach (var kvp in slownik) Console.WriteLine($"{kvp.Key} -> {kvp.Value}");

        // 5b
        Console.WriteLine($"\nKlucze: {string.Join(", ", slownik.Keys)}");
        Console.WriteLine("Wartości:");
        foreach (var v in slownik.Values) Console.WriteLine(v);

        // 5c
        Console.WriteLine($"\nCzy zawiera klucz 'z6'? {slownik.ContainsKey("z6")}");

        // 5d
        Console.WriteLine($"Max ze słownika: {slownik.Values.Max()}");
        var ujemneImSlownik = slownik.Values.Where(v => v.Im < 0);

        // 5e
        slownik.Remove("z3");
        Console.WriteLine("\nUsunięto 'z3'.");

        // 5f
        if (slownik.Count >= 2)
        {
            var kluczDoUsuniecia = slownik.Keys.ElementAt(1);
            slownik.Remove(kluczDoUsuniecia);
            Console.WriteLine($"Usunięto drugi element (klucz: {kluczDoUsuniecia}).");
        }

        // 5g
        slownik.Clear();


        Console.WriteLine("\n\n=== ZADANIE 6: KOLEJKA I STOS ===");
       
        Stack<ComplexNumber> stos = new Stack<ComplexNumber>();
        stos.Push(new ComplexNumber(1, 1));
        stos.Push(new ComplexNumber(2, 2));
        Console.WriteLine($"Stos Peek (podgląd): {stos.Peek()}"); 
        Console.WriteLine($"Stos Pop (zdejmij): {stos.Pop()}");  

        Queue<ComplexNumber> kolejka = new Queue<ComplexNumber>();
        kolejka.Enqueue(new ComplexNumber(1, 1));
        kolejka.Enqueue(new ComplexNumber(2, 2));
        Console.WriteLine($"Kolejka Peek (podgląd): {kolejka.Peek()}"); 
        Console.WriteLine($"Kolejka Dequeue (zdejmij): {kolejka.Dequeue()}"); 
    }
}