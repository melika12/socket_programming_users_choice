using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace challengeExerciseUsersChoice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("What would you like to logon? Server or client?: ");
            string answer = Console.ReadLine().Trim().ToLower();
            switch (answer)
            {
                case "server":
                    Server server = new Server();
                    break;
                case "client":
                    Client client = new Client();
                    break;
                default:
                    Console.WriteLine("You have not chosen between server or client. Try again :)");
                    break;
            }
        }
    }
}
