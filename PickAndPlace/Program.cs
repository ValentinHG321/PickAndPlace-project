using System;
using System.Collections.Generic;
using System.Linq;

namespace PickAndPlace
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string buffer = string.Empty;
            while (true)
            {

                List<string> input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (input[0] == "end")
                {
                    return;
                }
                string[] pattern = new string[] { "0603", "0805", "10p", "100n", "22p", "33p", "47n", "1u", "470p", "100k" };
                for (int i = 0; i < input.Count; i++)
                {
                    if (input.Contains(pattern[0]))
                    {
                        buffer = "C0603";
                    }
                }





            }

        }
    }
}
