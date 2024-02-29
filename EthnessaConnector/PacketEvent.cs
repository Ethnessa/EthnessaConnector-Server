namespace EthnessaConnector;

public class PacketEvent
{
    public byte packetType;
    public byte[] data;
    
    public PacketEvent(byte packetType, byte[] data)
    {
        this.packetType = packetType;
        this.data = data;
    }
}