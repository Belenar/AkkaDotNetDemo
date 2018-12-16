using System.Configuration;
using System.Data.SqlClient;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;
using Dapper;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    class HourlyConsumptionStorageActor : ReceiveActor, IHandle<HourCompleted>
    {
        public void Handle(HourCompleted message)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                connection.Execute(@"INSERT INTO [dbo].[HourlyConsumption] ([DeviceId], [Timestamp], [Consumption], [Unit])
                                     VALUES (@DeviceId, @Timestamp, @Consumption, @Unit)", message);
            }
        }
    }
}
