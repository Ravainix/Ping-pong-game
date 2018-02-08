using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        //-------------------------------------| Zmienne |--------------------------------------------------

        static int index = 0;

        static int playerPadSize = 10;
        static int aiPadSize = 5;

        static int ballX = 0;
        static int ballY = 0;

        static bool ballDirectionUp = true;
        static bool ballDirectionRight = false;

        static int playerPositionY = 0;
        static int playerPositionX = 1;

        static int aiPositionY = 0;
        static int aiPositionX = Console.WindowWidth - 1;

        static int playerResult = 0;
        static int aiResult = 0;

        static Random RandomNumberGenerator = new Random();

        //------------------------------------------------| Main |---------------------------------------------

        static void Main(string[] args)
        {

            List<string> menuItems = new List<string>() {
                "Start Game",
                "Exit"
            };
            Console.CursorVisible = false;

            while (true) //menu
            {
                string selectedMenuItem = DrawMenu(menuItems);
                if (selectedMenuItem == "Start Game") // 
                {
                    Console.Clear();

                    InitialSetup();
                    SetStartPositions();

                    while (true) //gra
                    {
                        Console.Clear();

                        DrawPaddle(playerPositionX, playerPositionY, playerPadSize, '#'); //paletka gracza
                        DrawPaddle(aiPositionX, aiPositionY, aiPadSize, '#'); //paletka ai

                        DrawBall('O');
                        MoveBall();

                        KeyDetection();
                        AiMove();

                        PrintResult();

                        Thread.Sleep(1000 / 25); //odswiezanie planszy
                    }
                }
                else if (selectedMenuItem == "Exit")
                {
                    Environment.Exit(0);
                }
            }

        }



        //------------------------------------------------------| MENU |-----------------------------------------



        static string DrawMenu(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.SetCursorPosition(0, Console.WindowHeight / 2);
                    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + items[i].Length / 2) + "}", items[i]));
                }
                else
                {
                    Console.SetCursorPosition(0, (Console.WindowHeight / 2) + 2);
                    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + items[i].Length / 2) + "}", items[i]));
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                if (index == items.Count - 1)
                {
                    index = 0; //Remove the comment to return to the topmost item in the list
                }
                else { index++; }
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                if (index <= 0)
                {
                    //index = menuItems.Count - 1; //Remove the comment to return to the item in the bottom of the list
                }
                else { index--; }
            }
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return items[index];
            }
            else
            {
                return "";
            }

            Console.Clear();
            return "";
        }




        //------------------------------------------| Game |------------------------------------------------------




        static void InitialSetup()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        static void DrawAt(int x, int y, char symbol)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
        }

        static void DrawPaddle(int _posX, int _posY, int _size, char _symbol)
        {
            for (int i = _posY; i < _posY + _size; i++)
            {
                DrawAt(_posX, i, _symbol);
            }
        }

        static void SetStartPositions()
        {
            playerPositionY = Console.WindowHeight / 2 - playerPadSize / 2;
            aiPositionY = Console.WindowHeight / 2 - aiPadSize / 2;
            ballX = Console.WindowWidth / 2;
            ballY = Console.WindowHeight / 2;
        }

        static void DrawBall(char _symbol)
        {
            DrawAt(ballX, ballY, _symbol);
        }

        static void PrintResult() //tabela wynikow
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 1, 0);
            Console.Write("{0}-{1}", playerResult, aiResult);
        }

        static void KeyDetection() //wykrywanie ruchu gracza
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    //przesuwanie paletki w gore
                    if (playerPositionY > 0)
                    {
                        playerPositionY--;
                    }
                }
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    //przesuwanie paletki w dol
                    if (playerPositionY < Console.WindowHeight - playerPadSize)
                    {
                        playerPositionY++;
                    }
                }
            }
        }

        //-----------------------------------------| Move Ai |--------------------------

        static void AiMove()
        {
            int randomNumber = RandomNumberGenerator.Next(1, 101);

            if (randomNumber <= 70)
            {
                if (ballDirectionUp == true)
                {
                    if (aiPositionY > 0)
                    {
                        aiPositionY--;
                    }
                }
                else
                {
                    if (aiPositionY < Console.WindowHeight - aiPadSize)
                    {
                        aiPositionY++;
                    }
                }
            }
        }

        private static void MoveBall()
        {
            if (ballY == 0)
            {
                ballDirectionUp = false;
            }

            if (ballY == Console.WindowHeight - 1)
            {
                ballDirectionUp = true;
            }

            if (ballX == Console.WindowWidth - 1)
            {
                SetStartPositions();
                ballDirectionRight = false;
                ballDirectionUp = true;
                playerResult++;

                Console.SetCursorPosition((Console.WindowWidth / 2) - 10, Console.WindowHeight / 2);
                Console.WriteLine("First player wins!");
                Console.ReadKey();
            }
            if (ballX == 0)
            {
                SetStartPositions();
                ballDirectionRight = true;
                ballDirectionUp = true;
                aiResult++;

                Console.SetCursorPosition((Console.WindowWidth / 2) - 10, Console.WindowHeight / 2);
                Console.WriteLine("Second player wins!");
                Console.ReadKey();
            }

            if (ballX < 3)
            {
                if (ballY >= playerPositionY && ballY < playerPositionY + playerPadSize)
                {
                    ballDirectionRight = true;
                }
            }

            if (ballX >= Console.WindowWidth - 3 - 1)
            {
                if (ballY >= aiPositionY && ballY < aiPositionY + aiPadSize)
                {
                    ballDirectionRight = false;
                }
            }

            if (ballDirectionUp)
            {
                ballY--;
            }
            else
            {
                ballY++;
            }

            if (ballDirectionRight)
            {
                ballX++;
            }
            else
            {
                ballX--;
            }
        }
    }
}
