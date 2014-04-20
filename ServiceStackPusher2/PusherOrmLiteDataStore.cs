using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.Logging;
using ServiceStack.OrmLite;

namespace ServiceStackPusher2
{
    public class PusherOrmLiteDataStore : IPusherOrmLiteConnectionFactory
    {
        public static string SqliteFileDb = "db.sqlite";

        private readonly OrmLiteConnectionFactory _dbFactory;

        public PusherOrmLiteDataStore()
        {
            _dbFactory = new OrmLiteConnectionFactory(SqliteFileDb, SqliteDialect.Provider)
            {
                AutoDisposeConnection = false,
            };
        }

        public IDbConnection Open()
        {
            return _dbFactory.Open();
        }

        public bool SaveDeviceToken(IDbConnection pusherDataStoreContext, DeviceTokenBase request, string pushTokenForDevice)
        {
            var logger = LogManager.GetLogger(GetType());

            var deviceTokens = GetDeviceTokensForRequest(pusherDataStoreContext, request, pushTokenForDevice);

            if (deviceTokens.Count > 0)
                pusherDataStoreContext.DeleteByIds<DeviceTokenEntity>(
                    deviceTokens.Select(deviceToken => deviceToken.Id).ToArray());

            pusherDataStoreContext.Insert(
                new DeviceTokenEntity
                {
                    Name = request.Name,
                    PushToken = pushTokenForDevice,
                    LastUpdated = DateTime.Now
                });

            deviceTokens = GetDeviceTokensForRequest(pusherDataStoreContext, request, pushTokenForDevice);

            if (deviceTokens.Count > 0)
            {
                logger.Info("Token saved!");

                deviceTokens.ForEach(delegate(DeviceTokenEntity deviceTokenEntity)
                {
                    var strLastUpdated = deviceTokenEntity.LastUpdated.HasValue ? deviceTokenEntity.LastUpdated.Value.ToString("MM-dd-yy hh:mm:ss") : string.Empty;
                    logger.InfoFormat("Name: {0} PushToken: {1} DateSaved: {2}", deviceTokenEntity.Name, deviceTokenEntity.PushToken, strLastUpdated);
                });

                return true;
            }

            logger.Info("Something happened whilst saving the token.");
            
            return false;
        }

        private static List<DeviceTokenEntity> GetDeviceTokensForRequest(IDbConnection pusherDataStoreContext, DeviceTokenBase request, string pushTokenForDevice)
        {
            var deviceTokens =
                pusherDataStoreContext.Select<DeviceTokenEntity>(
                    q => q.Name == request.Name && q.PushToken == pushTokenForDevice);
            return deviceTokens;
        }
    }
}
