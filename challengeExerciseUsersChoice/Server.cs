using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace challengeExerciseUsersChoice
{
    class Server
    {
        public static int defaPort = 1448;
        List<TcpClient> connectedClients = new List<TcpClient>();
        public Server()
        {
            Console.Write("Write which port you want to use [{0}]: ", defaPort);
            string s = Console.ReadLine();
            int port;
            if (s == "")
            {
                port = defaPort;
            }
            else
            {
                port = Convert.ToInt32(s);
            }

            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            listener.Start();
            Console.WriteLine("I'm ready!");

            AcceptClients(listener);
            string text = Console.ReadLine();
        }

       
        private async void AcceptClients(TcpListener listener)
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                connectedClients.Add(client);
                getMessage(client);
            }
           

        }
        public void BroadcastMessages(string msg)
        {
            TcpClient[] arrayOfClients = connectedClients.ToArray();
            for(int i = 0; i < arrayOfClients.Length; i++)
            {
                sendMessageToAnotherClient(arrayOfClients[i], msg);
            }
        }
        
        public async void sendMessageToAnotherClient(TcpClient client, string msg)
        {
            NetworkStream stream = client.GetStream();
            bool trueOrFalse = true;
            while(trueOrFalse)
            {
                if(msg == "")
                {
                    trueOrFalse = false;
                    break;
                }
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                await stream.WriteAsync(bytes, 0, msg.Length);
            }
        }

        public void getMessage(TcpClient client)
        {
            if (client.Client.Available > 0) return;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            bool doneReading = false;
            StringBuilder sb = new StringBuilder();

            while(!doneReading)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if(bytesRead > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    if(sb.ToString().IndexOf("<EOT>") > -1)
                    {
                        doneReading = true;
                    }
                }
            }

            string sendString = sb.ToString();
            BroadcastMessages(sendString.Substring(0, sendString.Length - "<EOT>".Length));
        }
    }
}
