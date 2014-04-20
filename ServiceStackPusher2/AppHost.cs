using ServiceStack;
using ServiceStack.OrmLite;

namespace ServiceStackPusher2
{
    //Define the Web Services AppHost
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(DeviceTokenRegistrationService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            //Signal advanced web browsers what HTTP Methods you accept
            base.SetConfig(new HostConfig
            {
                GlobalResponseHeaders =
				    {
					    { "Access-Control-Allow-Origin", "*" },
					    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
				    },
            });

            Routes
                .Add<DeviceTokenRequest>("/register")
                .Add<DeviceTokenRequest>("/register/{Name}");

            // Take care of dependencies
            container.RegisterAs<PusherOrmLiteDataStore, IPusherOrmLiteConnectionFactory>();

            using (var pusherDataStore = container.Resolve<IPusherOrmLiteConnectionFactory>().Open())
            {
                pusherDataStore.CreateTableIfNotExists<DeviceTokenEntity>();
                pusherDataStore.DeleteAll<DeviceTokenEntity>();
            }
        }
    }
}