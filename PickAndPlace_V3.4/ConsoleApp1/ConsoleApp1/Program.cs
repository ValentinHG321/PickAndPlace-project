using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] testArr = { 29.4, 123.3, 155, 5123, 321, 321, 21, 2, 5, 1 };

            double maxNum = GetMaxIndex(testArr);
            Console.WriteLine(maxNum);
        }

        static double GetMaxIndex(double[] input)
        {
            double number = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (number <= input[i])
                {
                    number = input[i];
                }
            }
            return number;
        }
    }

}

