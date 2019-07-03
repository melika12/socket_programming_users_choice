using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace challengeExerciseUsersChoice
{
    class Client
    {
        public Client()
        {
            Console.Write("Write the ip address you want to connect to [127.0.0.1]: ");
            string ip = Console.ReadLine();
            if(ip == "")
            {
                ip = "127.0.0.1";
            }
            IPAddress address = IPAddress.Parse(ip);
            Console.Write("Write which port you want to use [{0}]: ", Server.defaPort);
            string portNum = Console.ReadLine();
            int port;
            if (portNum == "")
            {
                port = Server.defaPort;
            }
            else
            {
                port = Convert.ToInt32(portNum);
            }
            
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(address, port));
            
            
            sendMesageToServer(client);
            while(true)
            {
                getMessage(client);
            }
        }

        public void getMessage(TcpClient client)
        {
            if (client.Client.Available > 0) return;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            bool doneReading = false;
            StringBuilder sb = new StringBuilder();
            while (!doneReading)
            {
                int LengthOfByteArray = stream.Read(buffer, 0, buffer.Length);
                if(LengthOfByteArray > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, LengthOfByteArray));
                    if(sb.ToString().IndexOf("<EOT>") > -1)
                    {
                        doneReading = true;
                    }
                }
                string recievedMessage = sb.ToString();
                Console.WriteLine(recievedMessage.Substring(0, recievedMessage.Length - "<EOT>".Length));
                Console.WriteLine(sb.ToString());
            }
        }

        public void sendMesageToServer(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            bool trueOrFalse = true;
            while (trueOrFalse)
            {
                string message = Console.ReadLine() + "<EOT>";
                if (message == "")
                {
                    trueOrFalse = false;
                    break;
                }
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes, 0, message.Length);
            }
        }

    }
}
