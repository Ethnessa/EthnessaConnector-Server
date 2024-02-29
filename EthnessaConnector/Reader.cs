using System.Net.Sockets;

namespace EthnessaConnector;

public class Reader
{
    public static bool ParsePacket(NetworkStream ns, out byte packetType, out byte[] data)
    {
        // Initialize out parameters
        packetType = 0;
        data = Array.Empty<byte>();

        // Buffer to read packet length and type
        byte[] header = new byte[3];
    
        // Read the header first to get the packet length and type
        int bytesRead = ns.Read(header, 0, header.Length);
        if (bytesRead < header.Length)
        {
            // Not enough data to read the complete header
            return false;
        }
    
        // Extract packet length and type from the header
        var packetLength = BitConverter.ToUInt16(header, 0);
        packetType = header[2];
    
        if (packetLength < 3)
        {
            // Packet length is too short to be valid
            return false;
        }

        // Calculate data length, excluding the size of the length and type fields
        int dataLength = packetLength - 3;

        // Allocate the data array to hold only the packet data
        data = new byte[dataLength];
    
        // If there's data to read beyond the header, read it into the data array
        if (dataLength > 0)
        {
            bytesRead = ns.Read(data, 0, data.Length);
            if (bytesRead < data.Length)
            {
                // Not enough data was read; handle this case as needed
                return false;
            }
        }
    
        return true;
    }

}