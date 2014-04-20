using System.IO;
using ServiceStack.Web;

namespace ServiceStackPusher2
{
    public class DeviceTokenRequest : DeviceTokenBase, IRequiresRequestStream
    {
        public Stream RequestStream { get; set; }
    }
}
