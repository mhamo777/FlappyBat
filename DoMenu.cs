﻿using CreateUsers;
using DoWalls;
using System.Text;

namespace Menue
{
    class DoMenu
    {
        public static int Points = 0;
        static int UpAndDown = 0;

        public static void Main()
        {
            Console.CursorVisible = false;
            Title();

            string text = """
                          Welcome to our Game, FlappyBat.
                          The game was programmed by Metehan, David and Hamlet.
                          It's a simple game, it's similar to FlappyBird, only with a bat.
                          We hope you enjoy our game . . .
                          """;

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(40);
            }

            Thread.Sleep(2000);
            Console.Clear();
            Title();
            Options();
        }

        public static void Title()
        {
            Console.WriteLine("███████╗██╗      █████╗ ██████╗ ██████╗ ██╗   ██╗██████╗  █████╗ ████████╗");
            Console.WriteLine("██╔════╝██║     ██╔══██╗██╔══██╗██╔══██╗╚██╗ ██╔╝██╔══██╗██╔══██╗╚══██╔══╝");
            Console.WriteLine("█████╗  ██║     ███████║██████╔╝██████╔╝ ╚████╔╝ ██████╔╝███████║   ██║   ");
            Console.WriteLine("██╔══╝  ██║     ██╔══██║██╔═══╝ ██╔═══╝   ╚██╔╝  ██╔══██╗██╔══██║   ██║   ");
            Console.WriteLine("██║     ███████╗██║  ██║██║     ██║        ██║   ██████╔╝██║  ██║   ██║   ");
            Console.WriteLine("╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝        ╚═╝   ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ");
        }

        public static void Options()
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool pressedEnter = false;
            string arrow = "\U0001F8A1";

            string t = "User;Start Game;End Game";
            string[] parts = t.Split(';');

            while (!pressedEnter)
            {
                Console.SetCursorPosition(0, 7);
                for (int i = 0; i < parts.Length; i++)
                {
                    if (UpAndDown == i)
                    {
                        Console.Write(arrow + " ");
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(parts[i]);
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("  " + parts[i]);
                    }
                }

                Keys(ref pressedEnter);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Entered();
        }

        public static void Keys(ref bool e)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.DownArrow)
            {
                UpAndDown++;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                UpAndDown--;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                e = true;
            }
        }

        public static void Entered()
        {
            Console.Clear();

            switch (UpAndDown)
            {
                case 0:
                    Users.TheUsers();
                    Title();
                    Options();
                    break;
                case 1:
                    FlappyBatWalls.WallMain();
                    Users.WriteAllInLines(Points);
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Error . . .");
                    break;
            }
        }
    }
}
