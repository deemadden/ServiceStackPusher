using System;
using ServiceStack.DataAnnotations;

namespace ServiceStackPusher2
{
    // ORMLite Entity
    public class DeviceTokenEntity : DeviceTokenBase
    {
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
