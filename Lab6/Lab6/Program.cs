using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

#region CONNECTION

string connectionString =
    "Data Source=10.200.2.28;" +
    "Initial Catalog=studenci_71476
    "Integrated Security=True;" +
    "Encrypt=True;" +
    "TrustServerCertificate=True";

using SqlConnection connection = new SqlConnection(connectionString);

try
{
    connection.Open();
    Console.WriteLine("Połączono z bazą danych.\n");

    ShowAllStudents(connection);
    ShowStudentById(connection, 1);

    var students = GetStudentsWithGrades(connection);
    AddStudent(connection, new Student { Imie = "Adam", Nazwisko = "Nowak" });

    AddGrade(connection, new Ocena
    {
        Wartosc = 4.5,
        Przedmiot = "matematyka",
        StudentId = 1
    });

    DeleteGeographyGrades(connection);
    UpdateGrade(connection, 1, 5.0);
}
catch (Exception ex)
{
    Console.WriteLine("Błąd: " + ex.Message);
}

#endregion

#region METHODS

// Zad 4
static void ShowAllStudents(SqlConnection connection)
{
    Console.WriteLine("Lista studentów:");
    string sql = "SELECT student_id, imie, nazwisko FROM student";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    using SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine($"{reader["student_id"]} | {reader["imie"]} | {reader["nazwisko"]}");
    }

    Console.WriteLine();
}

// Zad 5
static void ShowStudentById(SqlConnection connection, int studentId)
{
    string sql = "SELECT imie, nazwisko FROM student WHERE student_id = @id";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@id", studentId);

    using SqlDataReader reader = cmd.ExecuteReader();

    if (reader.Read())
        Console.WriteLine($"Student {studentId}: {reader["imie"]} {reader["nazwisko"]}\n");
    else
        Console.WriteLine("Student nie istnieje\n");
}

// Zad 6
static List<Student> GetStudentsWithGrades(SqlConnection connection)
{
    string sql =
        @"SELECT s.student_id, s.imie, s.nazwisko,
                 o.ocena_id, o.wartosc, o.przedmiot
          FROM student s
          LEFT JOIN ocena o ON s.student_id = o.student_id";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    using SqlDataReader reader = cmd.ExecuteReader();

    List<Student> students = new();

    while (reader.Read())
    {
        int id = reader.GetInt32(0);

        var student = students.FirstOrDefault(s => s.StudentId == id);
        if (student == null)
        {
            student = new Student
            {
                StudentId = id,
                Imie = reader.GetString(1),
                Nazwisko = reader.GetString(2)
            };
            students.Add(student);
        }

        if (!reader.IsDBNull(3))
        {
            student.Oceny.Add(new Ocena
            {
                OcenaId = reader.GetInt32(3),
                Wartosc = reader.GetDouble(4),
                Przedmiot = reader.GetString(5),
                StudentId = id
            });
        }
    }

    Console.WriteLine("Studenci z ocenami:");
    foreach (var s in students)
    {
        Console.WriteLine($"{s.Imie} {s.Nazwisko}");
        foreach (var o in s.Oceny)
            Console.WriteLine($"  {o.Przedmiot}: {o.Wartosc}");
    }
    Console.WriteLine();

    return students;
}

// Zad 7
static void AddStudent(SqlConnection connection, Student student)
{
    string sql = "INSERT INTO student(imie, nazwisko) VALUES (@i, @n)";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@i", student.Imie);
    cmd.Parameters.AddWithValue("@n", student.Nazwisko);
    cmd.ExecuteNonQuery();

    Console.WriteLine("Dodano studenta.\n");
}

// Zad 8
static void AddGrade(SqlConnection connection, Ocena ocena)
{
    if (!IsValidGrade(ocena.Wartosc))
    {
        Console.WriteLine("Nieprawidłowa ocena!\n");
        return;
    }

    string sql =
        "INSERT INTO ocena(wartosc, przedmiot, student_id) VALUES (@w, @p, @s)";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@w", ocena.Wartosc);
    cmd.Parameters.AddWithValue("@p", ocena.Przedmiot);
    cmd.Parameters.AddWithValue("@s", ocena.StudentId);
    cmd.ExecuteNonQuery();

    Console.WriteLine("Dodano ocenę.\n");
}

// Zad 9
static void DeleteGeographyGrades(SqlConnection connection)
{
    string sql = "DELETE FROM ocena WHERE przedmiot = 'geografia'";
    using SqlCommand cmd = new SqlCommand(sql, connection);
    int count = cmd.ExecuteNonQuery();

    Console.WriteLine($"Usunięto {count} ocen z geografii.\n");
}

// Zad 10
static void UpdateGrade(SqlConnection connection, int ocenaId, double value)
{
    if (!IsValidGrade(value))
    {
        Console.WriteLine("Nieprawidłowa ocena!\n");
        return;
    }

    string sql = "UPDATE ocena SET wartosc=@w WHERE ocena_id=@id";

    using SqlCommand cmd = new SqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@w", value);
    cmd.Parameters.AddWithValue("@id", ocenaId);
    cmd.ExecuteNonQuery();

    Console.WriteLine("Zaktualizowano ocenę.\n");
}

static bool IsValidGrade(double g)
{
    return g >= 2 && g <= 5 && g != 2.5 && g % 0.5 == 0;
}

#endregion

#region MODELS

class Student
{
    public int StudentId { get; set; }
    public string Imie { get; set; } = "";
    public string Nazwisko { get; set; } = "";
    public List<Ocena> Oceny { get; set; } = new();
}

class Ocena
{
    public int OcenaId { get; set; }
    public double Wartosc { get; set; }
    public string Przedmiot { get; set; } = "";
    public int StudentId { get; set; }
}

#endregion
