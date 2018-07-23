using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            for (int i = 1; i < 10; i++)
            {
                int ran = r.Next(10);
                Console.WriteLine(ran);
            }
            Console.ReadLine();
        }
    }
}
