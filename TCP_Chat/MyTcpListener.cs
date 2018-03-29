using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace TCP_Chat
{
    class MyTcpListener
    {
        public IPAddress IP = null;
        public Int32 Port = 0;
        private TcpListener Server = null;
        private TcpClient Client = null;
        private Byte[] bytes = new Byte[1024];
        private String data = null;
        private Byte[] Nachricht = null;
        private NetworkStream stream = null;
        private String GanzeNachricht = null;

        public MyTcpListener()
        {
            // Set the TcpListener on local Ip and port 13000.
            IP = IPAddress.Parse(GetLocalIPAddress());
            Port = Convert.ToInt32(ValidPort());
            Server = new TcpListener(IP, Port);
            Server.Start();
        }

        public MyTcpListener(string _IP, string _PORT)
        {
            // Set the TcpListener on custom IP and Port
            IP = IPAddress.Parse(_IP);
            Port = Convert.ToInt32(_PORT);
            Server = new TcpListener(IP, Port);
            Server.Start();
        }

        private static string GetLocalIPAddress()
        // https://stackoverflow.com/questions/6803073/get-local-ip-address
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public string WaitForConnectionAndMessage()
        {
            try
            {
                Console.Write("[1]Wait..   ");
                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                Client = Server.AcceptTcpClient();
                Console.Write("[2]Connected   ");

                data = null;
                Nachricht = null;
                GanzeNachricht = null;

                byte[] msg = null;

                // Get a stream object for reading and writing
                stream = Client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.Write("[3]Received: {0}   ", data);

                    msg = System.Text.Encoding.ASCII.GetBytes(data);
                    if (Nachricht != null && Nachricht.Length > 0)
                    {
                        Nachricht = Combine(Nachricht, msg);
                    }
                    else
                    {
                        Nachricht = msg;
                    }


                    GanzeNachricht = GanzeNachricht + data;

                    // Send back a response.
                    stream.Write(Nachricht, 0, Nachricht.Length);
                    Console.WriteLine("[4]pushed back");
                }

                //Console.WriteLine(Server.LocalEndpoint.ToString());
                // Shutdown and end connection
                Client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExceptionWait: {0}", e);
            }
            return GanzeNachricht;
        }

        public void Stop()
        {
            // Stop listening for new clients.
            Server.Stop();
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }


        public static int ValidPort()
        {
            int PortNumber = 13000;
            while (PortInUse(PortNumber))
            {
                PortNumber++;
            }
            return PortNumber;
        }


        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }


            return inUse;
        }
    }
}
