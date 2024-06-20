using Menue;
using NAudio.Wave;

namespace CreateUsers;

class Users
{
    static IWavePlayer? waveOutDevice;
    static AudioFileReader? audioFileReader;
    static int y = 0;
    public struct User
    {
        public string UserName;
        public string Password;
        public int Points;
    }
    public static void TheUsers()
    {
        Console.CursorVisible = false;

        //File.Delete("usernames.csv");

        User user = new();
        string n = string.Empty;

        if (!File.Exists(@"Assets\usernames.csv"))
        {
            StartMusic(@"Assets\typing.mp3");
            string text = "Sie haben noch keinen Account, erstellen Sie einen um fortzufahren. . .\n";
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }

            n = Regist(user, n);
            n += "\n";
            File.WriteAllText(@"Assets\usernames.csv", n);
            EndMusic();
        }
        else
        {
            StartMusic(@"Assets\typing.mp3");
            string text = "Wollen Sie einen neuen Account erstellen? ";
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            EndMusic();
            string inp = Console.ReadLine()!;

            if (inp == "ja")
            {
                n = Regist(user, n);
                n += "\n";
                File.AppendAllText(@"Assets\usernames.csv", n);
            }
        }

        int input = 0;
        User[] users = new User[100];
        users = ReadFile(users, ref input);

        string[] lines = new string[File.ReadAllLines(@"Assets\usernames.csv").Length];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = users[i].UserName + ";" + users[i].Password + ";" + users[i].Points;
        }

        //DoMenue.Name = 

        File.WriteAllLines(@"Assets\usernames.csv", lines);

        Console.Clear();

        Console.WriteLine("Gewählter User: " + users[input].UserName);

        Thread.Sleep(2000);

        Console.Clear();

    }
    static User[] Sort(User[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if (a[i].Points < a[j].Points)
                {
                    var temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                }
            }
        }

        return a;
    }
    static string Regist(User u, string t)
    {
        Console.Write("Username: ");
        u.UserName = Console.ReadLine()!;

        Console.Write("Password: ");
        u.Password = Console.ReadLine()!;

        t = u.UserName + ";" + u.Password + ";" + u.Points;

        int count = 0;
        while (count != 3)
        {

            Console.Clear();
            Console.Write("Account erfolgreich erstellt ");
            string text = ". . .";

            foreach (char item in text)
            {
                Console.Write(item);
                Thread.Sleep(300);
            }

            count++;
        }

        return t;
    }
    static User[] ReadFile(User[] u, ref int keep)
    {
        string[] lines = File.ReadAllLines(@"Assets\usernames.csv");
        bool check = false;


        while (!check)
        {
            Console.Clear();
            Console.WriteLine("      User\tPassword\tPoints\n");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] spl = lines[i].Split(';');

                u[i].UserName = spl[0];
                u[i].Password = spl[1];
                u[i].Points = Convert.ToInt32(spl[2]);
            }

            u = Sort(u);

            for (int i = 0; i < lines.Length; i++)
            {
                if (y == i)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Print(u[i].UserName, u[i].Password, u[i].Points);
                    keep = i;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Print(u[i].UserName, u[i].Password, u[i].Points);
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;

            StartMusic(@"Assets\click.mp3");
            Keys(ref check);
            EndMusic();

            if (check)
            {
                StartMusic(@"Assets\typing.mp3");
                PasswordIsValid(u, ref keep);
                EndMusic();
            }

        }
        return u;
    }
    static void PasswordIsValid(User[] u, ref int taken)
    {
        Console.Clear();
        string text = "Password eingeben: ";
        foreach (char item in text)
        {
            Console.Write(item);
            Thread.Sleep(50);
        }
        string input = Console.ReadLine()!;

        if (input != u[taken].Password)
        {
            Console.WriteLine("Password ist falsch . . .");
            Thread.Sleep(1000);
            Console.Clear();
            ReadFile(u, ref taken);
        }
    }
    static void Print(string a, string b, int p)
    {
        b = "********";
        Console.WriteLine($"{a,12}{b,12}{p,11}");
    }
    static void Design(int index, User[] u)
    {
        Console.Clear();

        Console.WriteLine($" User: {u[index].UserName}");

        //GameStart.Start();
    }
    public static void Keys(ref bool ch)
    {

        ConsoleKeyInfo key = Console.ReadKey();

        if (key.Key == ConsoleKey.DownArrow)
        {
            y++;
        }
        else if (key.Key == ConsoleKey.UpArrow)
        {
            y--;
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            ch = true;
        }
    }
    public static void StartMusic(string fileName)
    {
        waveOutDevice = new WaveOutEvent();
        audioFileReader = new AudioFileReader(fileName);
        waveOutDevice.Init(audioFileReader);
        waveOutDevice.Play();
    }
    public static void EndMusic()
    {
        waveOutDevice?.Stop();
        audioFileReader?.Dispose();
        waveOutDevice?.Dispose();
    }
}