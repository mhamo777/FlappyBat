using System;
using System.Diagnostics.Metrics;
using System.Text;
using System.Threading;

namespace DoWalls;

class FlappyBatWalls
{
    static int batPosition = 10; //einstellen, in welcher zeile der Vogel starten soll
    static int gravity = 1;
    static int jump = -4; //Einstellen, wie viele Zeilen man springen möchte
    static bool gameRunning = true;
    static int previousBirdPosition;
    static Random random = new();

    public static void Wall()
    {
        Console.CursorVisible = false;

        const int maxWalls = 5;
        int[] wallPositions = new int[maxWalls];
        int[] holePositions = new int[maxWalls];

        Thread.Sleep(2000);

        int spawnInterval = 40;         // Zeitdifferenz in ms zwischen Wände
        int repetation = 0;             // Wiederholung
        int currentWallIndex = 0;

        Thread thread1 = new(Jump);
        thread1.Start();

        while (true)
        {
            if (repetation % spawnInterval == 0)
            {
                wallPositions[currentWallIndex] = Console.WindowWidth - 1;
                holePositions[currentWallIndex] = random.Next(0, Console.WindowHeight - 5);
                currentWallIndex = (currentWallIndex + 1) % maxWalls;
            }

            DrawWalls(wallPositions, holePositions);
            Thread.Sleep(50);
            ClearWalls(wallPositions);

            // Neue Positionen erzeugen
            for (int i = 0; i < maxWalls; i++)
            {
                if (wallPositions[i] >= 0)
                {
                    wallPositions[i]--;
                }
            }

            repetation++;
        }
    }
    static void Jump()
    {
        //Console.Clear();
        Console.CursorVisible = false;
        // Console.SetWindowSize(150, windowHeight);
        //Console.SetBufferSize(150, windowHeight);

        Thread inputThread = new Thread(HandleInput);
        inputThread.Start();

        while (gameRunning)
        {
            DrawBird();
            UpdateBirdPosition();
            Thread.Sleep(65);//wie lange er in der selben stelle bleiben soll
        }
        //Console.WriteLine("Game Over!");
        //Console.SetCursorPosition(0, windowHeight / 2);
    }

    /// <summary>
    /// Draws a wall with Unicode and avoids the saved hole positions so the bat can pass
    /// </summary>
    /// <param name="wallPositions"></param>
    /// <param name="holePositions"></param>
    static void DrawWalls(int[] wallPositions, int[] holePositions)
    {
        string wallSymbol = "\u2588";
        string upperWallSymbol = "\u259c";
        string underWallSymbol = "\u259f";

        int wallLength = Console.WindowHeight;

        for (int i = 0; i < wallPositions.Length; i++)
        {
            int yPosition = wallPositions[i];
            int holePosition = holePositions[i];

            if (yPosition >= 0)
            {
                for (int xPosition = 0; xPosition < wallLength; xPosition++)
                {
                    if(xPosition == holePosition - 1)
                    {
                        Console.SetCursorPosition(yPosition, xPosition);
                        Console.Write(upperWallSymbol);
                    }
                    else if (xPosition == holePosition + 9)
                    {
                        Console.SetCursorPosition(yPosition, xPosition);
                        Console.Write(underWallSymbol);
                    }
                    else if (xPosition < holePosition || xPosition >= holePosition + 10)
                    {
                        Console.SetCursorPosition(yPosition, xPosition);
                        Console.Write(wallSymbol);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Clears the walls on the Console
    /// </summary>
    /// <param name="wallPositions"></param>
    static void ClearWalls(int[] wallPositions)
    {
        int wallLength = Console.WindowHeight;

        // Führe das aus für jede y-Position im array wallPositions
        foreach (int yPosition in wallPositions)
        {
            if (yPosition >= 0)
            {
                for (int xPosition = 0; xPosition < wallLength; xPosition++)
                {
                    Console.SetCursorPosition(yPosition, xPosition);
                    Console.Write(" ");
                }
            }
        }
    }

    static void HandleInput()
    {
        while (gameRunning)
        {
            //springen
            if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                batPosition += jump;
            }
        }
    }

    static void DrawBird()
    {
        // löschen von alte stelle
        Console.SetCursorPosition(0, previousBirdPosition);
        Console.Write(new string(' ', 30));


        // an neue position zeichnen
        Console.SetCursorPosition(0, batPosition);
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("                          \U0001F987");
    }

    static void UpdateBirdPosition()
    {
        previousBirdPosition = batPosition;
        batPosition += gravity;

        // schauen, ob der vogel den Boden berührt hat
        if (batPosition >= 30)
        {
            gameRunning = false;
        }
    }
}