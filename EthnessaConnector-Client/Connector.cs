using System.Net;
using Terraria;
using TerrariaApi.Server;

namespace EthnessaConnector_Client
{
    [ApiVersion(2, 1)]
    public class Connector : TerrariaPlugin
    {
        // move this to config
        public int connectorPort = 7777;
        public IPAddress connectorAddress = IPAddress.Parse("127.0.0.1");

        public override string Name => "Connector";
        public override string Description => "Facilitates cross-server compatibility.";
        public override string Author => "Average";
        public override Version Version => new(1, 0);
        public Connector(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            
        }
    }
}
