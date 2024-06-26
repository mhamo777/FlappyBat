using System;
using System.Text;
using System.Threading;
using Menue;

namespace DoWalls
{
    class FlappyBatWalls
    {
        static int batPosition = 10;
        static int gravity = 1;
        static int jump = -4;
        static bool gameRunning = true;
        static int previousBirdPosition;
        static Random random = new();
        static object lockObject = new();
        static int score = 0;

        public static void Wall()
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            const int maxWalls = 5;
            int[] wallPositions = new int[maxWalls];
            int[] holePositions = new int[maxWalls];

            int spawnInterval = 40;
            int repetation = 0;
            int currentWallIndex = 0;

            Thread thread1 = new(Jump);
            thread1.Start();

            while (gameRunning)
            {
                lock (lockObject)
                {
                    if (repetation % spawnInterval == 0)
                    {
                        wallPositions[currentWallIndex] = Console.WindowWidth - 1;
                        holePositions[currentWallIndex] = random.Next(0, Console.WindowHeight - 10);
                        currentWallIndex = (currentWallIndex + 1) % maxWalls;
                    }

                    DrawWalls(wallPositions, holePositions);
                    Thread.Sleep(50);
                    ClearWalls(wallPositions);

                    for (int i = 0; i < maxWalls; i++)
                    {
                        if (wallPositions[i] >= 0)
                        {
                            wallPositions[i]--;
                        }
                    }

                    repetation++;
                    CheckCollision(wallPositions, holePositions);
                }
            }
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.WriteLine("Game Over!");
            Thread.Sleep(2000);
            DoMenu.Points = score;
            score = 0;
            Console.Clear();
            DoMenu.Title();
            DoMenu.Options();
        }

        static void Jump()
        {
            Console.CursorVisible = false;

            Thread inputThread = new Thread(HandleInput);
            inputThread.Start();

            while (gameRunning)
            {
                lock (lockObject)
                {
                    DrawBird();
                    UpdateBirdPosition();
                }
                Thread.Sleep(65);
            }
        }

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
                        if (xPosition == holePosition - 1)
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

        static void ClearWalls(int[] wallPositions)
        {
            int wallLength = Console.WindowHeight;

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
                if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                {
                    lock (lockObject)
                    {
                        batPosition += jump;
                        if (batPosition < 0)
                        {
                            batPosition = 0;
                        }
                        score++;
                    }
                }
            }
        }

        static void DrawBird()
        {
            Console.SetCursorPosition(0, previousBirdPosition);
            Console.Write(" ".PadRight(30));

            Console.SetCursorPosition(0, batPosition);
            Console.Write("                          \U0001F987");
        }

        static void UpdateBirdPosition()
        {
            previousBirdPosition = batPosition;
            batPosition += gravity;

            if (batPosition >= Console.WindowHeight || batPosition < 0)
            {
                gameRunning = false;
            }
        }

        static void CheckCollision(int[] wallPositions, int[] holePositions)
        {
            for (int i = 0; i < wallPositions.Length; i++)
            {
                if (wallPositions[i] == 0)
                {
                    if (batPosition < holePositions[i] || batPosition > holePositions[i] + 9)
                    {
                        gameRunning = false;
                    }
                }
            }
        }

        public static void WallMain()
        {
            Wall();
        }
    }
}
