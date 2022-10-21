using System;

namespace Random_number
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();


            while (true)
            {
                double low = 0;
                double high = 0;
                double n = 0;
                double z = 0;
                string input = Console.ReadLine();


                if (input == "stop")
                {
                    Environment.Exit(0);
                }

                int count = int.Parse(input);

                int max = int.Parse(Console.ReadLine());
                int[] number = new int[count];
                for (int i = 0; i < count; i++)
                {
                    number[i] = random.Next(0, max);
                    Console.Write("random num: ");
                    Console.WriteLine(number[i]);
                }
                foreach (int num in number)
                {

                    if (num <= max / 2)
                    {
                        n++;
                    }
                    else if (num > max / 2)
                    {
                        z++;
                    }

                }
                low = (n / count) * 100;
                high = (z / count) * 100;
                Console.WriteLine($"low nums:{n}|{low:F2}%");
                Console.WriteLine($"high nums:{z}|{high:F2}%");
            }


        }
    }
}
