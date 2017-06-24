using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testSet
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<KeyValuePair<string, string>> set = new HashSet<KeyValuePair<string, string>>();

            for(int i = 1; i < 10; i++)
            {

               KeyValuePair<string, string> kv = new KeyValuePair<string, string>(Convert.ToString(i), Convert.ToString(i));
               set.Add(kv);
            }
            Console.WriteLine(set.Count);

            for (int i = 1; i < 10; i++)
            {

                KeyValuePair<string, string> kv = new KeyValuePair<string, string>(Convert.ToString(i), Convert.ToString(i));
                set.Add(kv);
            }
            Console.WriteLine(set.Count);
            Console.Read();
        }
    }
}
