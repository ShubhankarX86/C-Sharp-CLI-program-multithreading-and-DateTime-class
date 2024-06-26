using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using datentime;
using Microsoft.Win32;

namespace multithreading
{
    internal class Program
    {      
        static void Main(string[] args)
        {
            int loginCounter = 0;
            int registerCounter = 0;
            bool state = true;

            while (state)
            {
               
                if (loginCounter == 0)
                {
                    Console.WriteLine("Press 1 to login \nPress 2 to create new \nPress 3 to Exit");
                }
                else
                {
                    Console.WriteLine("Press 1 to login again \nPress 2 to create new \nPress 3 to Exit");
                }

                char option = char.Parse(Console.ReadLine());

                switch (option)
                {
                    case '1':
                        login();
                        loginCounter++;
                        break;
                    case '2':
                        Thread registerThread = new Thread(register);
                        registerThread.Start();
                        registerThread.Join();

                        Thread loginThread = new Thread(login);
                        loginThread.Start();
                        loginThread.Join();
                        break;
                    case '3':
                        state = false;
                        Console.WriteLine("BYE BYE!! :)");
                        break;

                    default:
                        Console.WriteLine("Kindly enter one of the above options: ");
                        break;
                }
            }
        }

        public static void login()
        {
            Dictionary<String, String> userMap = new Dictionary<String, String>();
            string ID = " ";

            foreach (string line in File.ReadLines(generateDir()))
            { 
                userMap.Add(line.Split(',')[0], line.Split(',')[1]);
                ID = line.Split(',')[0];
            }

            if(userMap.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("DataBase empty!! User not found!!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Welcome " + ID);
                Console.WriteLine();
                Console.WriteLine("Employee DB directory: ");
                try
                {
                    string path = Console.ReadLine(); //file format: employeeName,datetime
                    DT.display(path);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("FileNotFound! {0}", ex.Message);
                    Console.WriteLine();
                }
            }
        }
        public static void register()
        {
            string str = "";

            foreach (string line in File.ReadLines(generateDir()))
            {
                str = line.Split(',')[0];
            }
            char registerCounter;

            if (str.Length == 0)
            {
                registerCounter = '0';
            }
            else
            {
                registerCounter = (char)(str[12]+1);
            }

            string ID = "Shubhankar00" + registerCounter + "@gmail.com";
            string password = registerCounter + "1" + registerCounter + "6";
            Program.writeLog(ID + "," + password);

            Console.WriteLine();
            Console.WriteLine("A guest ID has been created, you're now Logged-in!");
        }

        public static string generateDir()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(path).Parent.Parent.Parent;
            string finPath = Path.Combine(Dinfo.FullName, "userDB.txt");

            return finPath;
        }

        public static void writeLog(string message)
        {
            FileStream fs = new FileStream(Program.generateDir(), FileMode.Open, FileAccess.Write);

           
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            fs.Write(bytes, 0, message.Length);
            fs.Close();
        }
    }
}
