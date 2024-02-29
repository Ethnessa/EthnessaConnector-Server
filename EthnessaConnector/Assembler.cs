namespace EthnessaConnector;

public class Assembler
{
    //header includes the packet length and type, so we need to add 3 to the length of the data
    // packet length is uint16
    // packet type is uint8
    
    // packet type: 3
    // data: 1 byte of user slot
    // should look like this: Data: 0500030000
    public static byte[] AssemblePacket(byte packetType, byte[] data)
    {
        // Correctly calculate the total packet size
        ushort totalLength = (ushort)(data.Length + 3);
        
        byte[] packet = new byte[totalLength];

        // Write the packet length and type to the packet
        BitConverter.GetBytes(totalLength).CopyTo(packet, 0);
        packet[2] = packetType;
        
        // Write the data to the packet
        data.CopyTo(packet, 3);
        
        return packet;
    }
}