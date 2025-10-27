using System;

// Zad 1
public class Zwierze
{
    protected string nazwa;

    public Zwierze(string nazwa)
    {
        this.nazwa = nazwa;
    }

    public virtual void daj_glos()
    {
        Console.WriteLine("...");
    }
}

// Zad 2
public class Pies : Zwierze
{
    public Pies(string nazwa) : base(nazwa) { }

    public override void daj_glos()
    {
        Console.WriteLine($"{nazwa} robi woof woof!");
    }
}

// Zad 3
public class Kot : Zwierze
{
    public Kot(string nazwa) : base(nazwa) { }

    public override void daj_glos()
    {
        Console.WriteLine($"{nazwa} robi miau miau!");
    }
}

// Zad 4
public class Waz : Zwierze
{
    public Waz(string nazwa) : base(nazwa) { }

    public override void daj_glos()
    {
        Console.WriteLine($"{nazwa} robi ssssssss!");
    }
}

// Zad 5
public static class Pomocnik
{
    public static void powiedz_cos(Zwierze z)
    {
        z.daj_glos();
        Console.WriteLine($"Typ obiektu: {z.GetType().Name}\n");
    }
}

// Zad 8
public abstract class Pracownik
{
    public abstract void Pracuj();
}

// Zad 9
public class Piekarz : Pracownik
{
    public override void Pracuj()
    {
        Console.WriteLine("Trwa pieczenie...");
    }
}

// Zad12-14. Klasy A, B, C
public class A
{
    public A()
    {
        Console.WriteLine("To jest konstruktor A");
    }
}

public class B : A
{
    public B() : base()
    {
        Console.WriteLine("To jest konstruktor B");
    }
}

public class C : B
{
    public C() : base()
    {
        Console.WriteLine("To jest konstruktor C");
    }
}

// Zad 7, 10, 11, 15. Main
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Zadanie 7");
        Zwierze z = new Zwierze("Nieznane zwierzę");
        Pies p = new Pies("Bobik");
        Kot k = new Kot("Niawczyk");
        Waz w = new Waz("Python");

        Pomocnik.powiedz_cos(z);
        Pomocnik.powiedz_cos(p);
        Pomocnik.powiedz_cos(k);
        Pomocnik.powiedz_cos(w);

        Console.WriteLine("Zadanie 10");
        Piekarz piekarz = new Piekarz();
        piekarz.Pracuj();

        Console.WriteLine("Zadanie 11");
        Console.WriteLine("Nie można utworzyć obiektu klasy abstrakcyjnej Pracownik.");

        Console.WriteLine("Zadanie 15");
        A a = new A();
        B b = new B();
        C c = new C();
    }
}
