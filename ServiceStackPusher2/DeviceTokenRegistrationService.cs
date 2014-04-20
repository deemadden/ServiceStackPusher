using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace ServiceStackPusher2
{
    public class DeviceTokenRegistrationService : Service
    {
        public object Any(DeviceTokenRequest request)
        {
            ResolveNamePath(request);

            var logger = LogManager.GetLogger(GetType());
            logger.InfoFormat("Received Request from Device {0}", request.Name);

            var tokenSaved = SaveDevicePushToken(request);

            return new DeviceTokenRegistrationResponse
            {
                Result = tokenSaved ? "success" : "error"
            };
        }

        // Helper methods
        private bool SaveDevicePushToken(DeviceTokenBase request)
        {
            var rawJson = Request.GetRawBody();
            var wasDeviceTokensaved = false;

            if (!rawJson.IsNullOrEmpty())
            {
                var jsonObject = JsonObject.Parse(rawJson);

                string pushTokenForDevice;

                if (jsonObject.TryGetValue("deviceToken", out pushTokenForDevice))
                {
                    // Token should be saved here
                    var pusherDataStore = new PusherOrmLiteDataStore();

                    using (var pusherDataStoreContext = pusherDataStore.Open())
                    {
                        wasDeviceTokensaved = pusherDataStore.SaveDeviceToken(pusherDataStoreContext, request, pushTokenForDevice);
                    }
                }
            }

            return wasDeviceTokensaved;
        }

        private void ResolveNamePath(DeviceTokenBase request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (!string.IsNullOrEmpty(request.Name))
                return;

            const int namePathIndex = 1;
            var pathParts = Request.PathInfo.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            request.Name = pathParts.Length > 1 ? pathParts[namePathIndex] : string.Empty;
        }
    }
}
