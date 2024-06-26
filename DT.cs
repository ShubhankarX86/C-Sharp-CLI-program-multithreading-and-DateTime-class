using multithreading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace datentime
{
    public enum Months
    {
        January = 1,
        Feburary,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public partial class DT
    {
        public static void display(string pat)
        {
            DateTime curr = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("Upcoming Birthdays: ");
            Console.WriteLine();

            foreach (string line in File.ReadLines(generateDir2(pat)))
            {
                string[] parts = line.Split(',');
                string name = parts[0];
                DateTime birthday = DateTime.Parse(parts[1]);

                
                DateTime thisYearBirthday = new DateTime(curr.Year, birthday.Month, birthday.Day);
                DateTime nextYearBirthday = thisYearBirthday.AddYears(1);

                
                int diff = (thisYearBirthday - curr).Days;
                if (diff < 0)
                {
                    diff = (nextYearBirthday - curr).Days;
                }

                if (diff <= 10 && diff >= 0)
                {
                    Console.WriteLine($"{name}: {birthday.Day} {(Months)birthday.Month} ({diff} days from now)");
                }
            }
            Console.WriteLine();
        }

        public static string generateDir2(string pat)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(path).Parent.Parent.Parent;
            string myloc = Path.Combine(Dinfo.FullName, pat);

            return myloc;
        }
    }
}
  