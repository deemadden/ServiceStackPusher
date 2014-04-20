using System.Data;

namespace ServiceStackPusher2
{
    interface IPusherOrmLiteConnectionFactory
    {
        IDbConnection Open();
    }
}
