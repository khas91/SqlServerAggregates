using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] values = new String[] { "2.3", "343", "343" };

            foreach (string item in String.Join(",", values).Split(new char[] {',' }))
            {
                Console.WriteLine(item);
            }
        }
    }
}
