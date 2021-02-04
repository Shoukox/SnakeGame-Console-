using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using YoutubeExtractor;
using YoutubeExplode;
using System.Net;
using System.Linq;

namespace codingsmth
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakeGame SnakeGame = new SnakeGame(25,20);
            SnakeGame.StartGame();
        }
    }

    class SnakeGame
    {
        int fieldW { get; set; }
        int fieldH { get; set; }
        Position apple { get; set; }
        List<Position> sneak = new List<Position>();
        List<List<int>> field = new List<List<int>>();

        private void Print(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        private void Print()
        {
            Console.Clear();
            if(sneak.FirstOrDefault(m=>m.posX == apple.posX && m.posY == apple.posY) != default)
            {
                if (sneak.Count >= 2)
                    sneak.Add(new Position()
                    {
                        posX = sneak[sneak.Count - 1].posX + (sneak[sneak.Count - 2].posX - sneak[sneak.Count - 1].posX),
                        posY = sneak[sneak.Count - 1].posY + (sneak[sneak.Count - 2].posY - sneak[sneak.Count - 1].posY)
                    });
                else sneak.Add(new Position()
                {
                    posX = sneak[sneak.Count - 1].posX + 1,
                    posY = sneak[sneak.Count - 1].posY + 1
                });
                GenerateApple();
            }
            for (int i = 0; i <= fieldH - 1; i++)
            {
                for (int j = 0; j <= fieldW - 1; j++)
                {
                    if (sneak.FirstOrDefault(m => m.posX == j && m.posY == i) != default)
                    {
                        field[i][j] = 1;
                        Print($"{field[i][j]}", ConsoleColor.Green);
                    }
                    else if (apple.posX == j && apple.posY == i)
                    {
                        field[i][j] = 2;
                        Print($"{field[i][j]}", ConsoleColor.Red);
                    }
                    else
                    {
                        field[i][j] = 0;
                        Print($"{field[i][j]}", ConsoleColor.Yellow);
                    }
                }
                Console.WriteLine();
            }
        }
        public SnakeGame(int fieldW, int fieldH)
        {
            this.fieldH = fieldH;
            this.fieldW = fieldW;
            apple = new Position() {posX = fieldW / 2 - 2, posY = fieldH / 2 - 3};
            sneak.Add(new Position() {posX = fieldW / 2, posY = fieldH / 2});
            for (int i = 0; i <= fieldH - 1; i++)
            {
                field.Add(new List<int>());
                for (int j = 0; j <= fieldW - 1; j++)
                {
                    if (sneak.FirstOrDefault(m=>m.posX == j && m.posY == i) != default)
                        field[i].Add(1);
                    else if (apple.posX == j && apple.posY == i)
                        field[i].Add(2);
                    else field[i].Add(0);
                }
                Console.WriteLine(string.Join("", field[i]));
            }
        }
        private string getPressedButton()
        {
            return Console.ReadKey().KeyChar.ToString();
        }
        private void b()
        {
            void move()
            {
                for (int i = sneak.Count - 1; i >= 1; i--)
                {
                    sneak[i] = new Position() {posX = sneak[i - 1].posX, posY = sneak[i - 1].posY};
                }
            }
            string button = getPressedButton().ToLower();
            switch (button)
            {
                case "w":
                    move();
                    sneak[0].posY -= 1;
                    break;
                case "s":
                    move();
                    sneak[0].posY += 1;
                    break;
                case "d":
                    move();
                    sneak[0].posX += 1;
                    break;
                case "a":
                    move();
                    sneak[0].posX -= 1;
                    break;
            }

        }

        private void GenerateApple()
        {
            while (true)
            {
                int rnd = new Random().Next(0, fieldH);
                int rnd1 = new Random().Next(0, fieldH);
                if (field[rnd][rnd1] == 0)
                {
                    apple = new Position { posX = rnd1, posY = rnd };
                    break;
                }
            }
        }

        public void StartGame()
        {
            while (true)
            {
                Print();
                b();
            }
        }
        class Position
        {
            public int posX { get; set; }
            public int posY { get; set; }
        }
    }

}
//class Neuron
//{
//    public double weight = 0.5;
//    public double smoothing = 0.00001;
//    public double lastError { get; private set; }

//    public double Input(double input)
//    {
//        return input * weight;
//    }
//    public double Output(double output)
//    {
//        return output / weight;
//    }
//    public void Train(double input, double expectedResult)
//    {
//        double actualResult = Input(input);
//        lastError = expectedResult - actualResult;
//        weight += (lastError / actualResult) * smoothing;
//    }
//}
//static void Main(string[] args)
//{
//    double sum = 1;
//    double ruble = 2;
//    Neuron neuron = new Neuron();
//    int i = 0;
//    do
//    {
//        i++;
//        neuron.Train(sum, ruble);
//        if (i % 100000 == 0) Console.WriteLine($"Итерация: {i}, Ошибка: {neuron.lastError}");
//    }
//    while (neuron.lastError > neuron.smoothing || neuron.lastError < -neuron.smoothing);
//    Console.WriteLine($"{1} км = {neuron.Input(1)} миль");
//}
