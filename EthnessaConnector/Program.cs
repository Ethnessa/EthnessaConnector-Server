using System.Net;
using System.Net.Sockets;
using System.Text;
using Terraria;

namespace EthnessaConnector
{
    internal class Program
    {
        // make this configurable
        static int[] ports = { 7776 };
        private static RealServerCommunication realServer;
        
        public static Action<PacketEvent> PacketReceived;
        private static TcpClient client;
        
        static void Main(string[] args)
        {
            // make this configurable
            int port = 7777;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            TcpListener server = new(localAddr, port);

            server.Start();

            byte[] bytes = new byte[256];
            string? data = null;
            PacketReceived += OnPacketReceived;

            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                
                client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                NetworkStream ns = client.GetStream();

                byte[] helloMsg = Encoding.ASCII.GetBytes("Connected to Ethnessa Proxy Server.");

                ns.Write(helloMsg, 0, helloMsg.Length);
                
                while (client.Connected)
                {
                    // we are going to read the packets sent from the client
                    Reader.ParsePacket(ns, out byte packetType, out byte[] msg);

                    int invalidAttempts = 0;
                    switch (packetType)
                    {
                        case 0:
                        {
                            invalidAttempts++;
                            if (invalidAttempts > 3)
                            {
                                Console.WriteLine("Too many invalid attempts. Disconnecting client.");
                                client.Close();
                            }

                            break;
                        }
                        case 1: // Connect Request
                        {
                            // returns a string with the terraria internal version, ex: 'Terraria279'
                            var terrariaVersionBytes = Encoding.ASCII.GetString(msg, 0, msg.Length);
                            Console.WriteLine("Connect Request received. Player attempted to connect with Terraria version: " + terrariaVersionBytes);

                            realServer = new RealServerCommunication("127.0.0.1", ports[0]);
                            
                            realServer.Write(Assembler.AssemblePacket(1, msg));
                            break;
                        }
                        case 3: // set user slot
                        {
                            break;
                        }
                        case 4: // player info
                        {
                            Console.WriteLine("Received player info packet. Forwarding to real server.");
                            realServer.Write(Assembler.AssemblePacket(4, msg));
                            break;
                        }
                        default:
                        {
                            realServer.Write(Assembler.AssemblePacket(packetType, msg));
                            Console.WriteLine($"Packet received: {packetType}");
                            break;
                        }
                    }

                    
                    
                    
                }

                Console.WriteLine("The client has disconnected.");
                client.Close();
            }

        }

        private static void OnPacketReceived(PacketEvent obj)
        {
            switch(obj.packetType)
            {
                case 0:
                {
                    break;
                }
                case 1:
                {
                    break;
                }
                case 3:
                {
                    // get singular byte from data
                    byte userSlot = obj.data[0];
                    Console.WriteLine("Received user slot: " + userSlot);
                    
                    // send the user slot to the client
                    // we want to send the userSlot, and a false boolean
                    byte[] data = { userSlot, 0 };
                    
                    client.GetStream().Write(Assembler.AssemblePacket(3, data));
                    break;
                }
                default:
                {
                    client.GetStream().Write(Assembler.AssemblePacket(obj.packetType, obj.data));
                    break;
                }
            }
        }
    }
}
