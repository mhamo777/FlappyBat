using System;
using System.Threading;

namespace DoWalls;

class FlappyBatWalls
{
    static Random random = new();

    public static void Wall()
    {
        Console.CursorVisible = false;

        const int maxWalls = 5;
        int[] wallPositions = new int[maxWalls];
        int[] holePositions = new int[maxWalls];
        //abc

        Thread.Sleep(2000);

        int spawnInterval = 40;         // Zeitdifferenz in ms zwischen Wände
        int repetation = 0;             // Wiederholung
        int currentWallIndex = 0;       // 

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

    /// <summary>
    /// Draws a wall with Unicode and avoids the saved hole positions so the bat can pass
    /// </summary>
    /// <param name="wallPositions"></param>
    /// <param name="holePositions"></param>
    static void DrawWalls(int[] wallPositions, int[] holePositions)
    {
        int wallLength = Console.WindowHeight;

        for (int i = 0; i < wallPositions.Length; i++)
        {
            int yPosition = wallPositions[i];
            int holePosition = holePositions[i];

            if (yPosition >= 0)
            {
                for (int xPosition = 0; xPosition < wallLength; xPosition++)
                {
                    if (xPosition < holePosition || xPosition >= holePosition + 10)
                    {
                        Console.SetCursorPosition(yPosition, xPosition);
                        Console.Write("*");
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
}