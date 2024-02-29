using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EthnessaConnector;

public class RealServerCommunication
{
    private TcpClient client;
    private NetworkStream stream;
    
    public RealServerCommunication(string ip, int port)
    {
        // create a tcp client, connect to the real server
        client = new TcpClient(ip, port);
        
        // send the packet to the REAL server
        // we are really just disguising the proxy server (connector) as the client
        stream = client.GetStream();
        
        // we want to send messages to the real server
        
                    
        
    }

    public void Write(byte[] data)
    {
        stream.Write(data, 0, data.Length);
        Console.WriteLine("Sent packet to real server.");
        
        // receive response
        
        Reader.ParsePacket(stream, out byte packetType, out byte[] msg);
        Console.WriteLine("Received response from real server.");

        switch (packetType)
        {
            case 3:
            {
                Console.WriteLine("Received packet 3, user slot");
                Program.PacketReceived?.Invoke(new PacketEvent(packetType, msg));
                break;
            }
            default:
            {
                Console.WriteLine("Received unimplemented packet type: " + packetType);
                break;
            }
        }
        
    }
    
}