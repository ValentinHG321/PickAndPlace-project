using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string,List<int>> keyValuePairs = new Dictionary<string,List<int>>();
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            keyValuePairs.Add("kek", list);
            keyValuePairs["kek"].Add(10);
            int x = keyValuePairs["kek"][3];
            Console.WriteLine(x);
        }
    }
}
